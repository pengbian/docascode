namespace Microsoft.DxBuild.Components
{
    using Microsoft.Content.Build.Core;
    using Microsoft.Content.BuildEngine.DataAccessor.Azure;
    using Microsoft.Content.BuildEngine.DataAccessor.Extensions;
    using Microsoft.Content.BuildEngine.DataAccessorInterface;
    using Microsoft.Content.BuildEngine.StatusReport;
    using Microsoft.Content.BuildService;
    using Microsoft.Content.ServiceBus;
    using Microsoft.Content.WebService;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using StatusReport = Microsoft.Content.BuildEngine.StatusReport;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// Download assembly metadata and developer comments from corpnet through service bus.
    /// Upload metadata and comments to corresponding Azure blobs
    /// </summary>
    public class MRefFilesUploading : MRefComponent
    {
        private int _uploadFilesToBlobTimeout = 600000; //Default value is 600000 milliseconds. Can be configured from pipeline file.
        private int retryCount = 3;
        private int minRetryStrategyBackoff = 5; //Default value in seconds
        private int maxRetryStrategyBackoff = 600; //Default value in seconds
        private int deltaRetryStrategyBackoff = 300; //Default value in seconds
        private string serviceBusUrl;
        private List<string> mrefAssemblySourcePaths;
        private List<string> mrefAssemblyDependencyPaths;
        private List<string> mrefDeveloperCommentsPaths;
        private List<FileContent> sourceAssemblyMetadatas;
        private List<FileContent> dependencyAssemblyMetadatas;
        private List<FileContent> developerComments;
        private IBlobAccessor blobAccessor;

        // Define your retry policy using the retry strategy and the Windows Azure service bus transient fault detection strategy.
        private RetryStrategy _retryStrategy = null;
        private RetryPolicy _retryPolicy = null;

        #region Properties
        public int UploadFilesToBlobTimeout
        {
            get
            {
                return _uploadFilesToBlobTimeout;
            }
            set
            {
                _uploadFilesToBlobTimeout = value;
            }
        }

        public int RetryCount
        {
            get
            {
                return retryCount;
            }
            set
            {
                retryCount = value;
            }
        }

        public int MinRetryStrategyBackoff
        {
            get
            {
                return minRetryStrategyBackoff;
            }
            set
            {
                minRetryStrategyBackoff = value;
            }
        }

        public int MaxRetryStrategyBackoff
        {
            get
            {
                return maxRetryStrategyBackoff;
            }
            set
            {
                maxRetryStrategyBackoff = value;
            }
        }

        public int DeltaRetryStrategyBackoff
        {
            get
            {
                return deltaRetryStrategyBackoff;
            }
            set
            {
                deltaRetryStrategyBackoff = value;
            }
        }

        public int TotalAssmeblyNumber
        {
            get
            {
                return sourceAssemblyMetadatas.Count + dependencyAssemblyMetadatas.Count;
            }
        }

        public int SuccessAssemblyNumber
        {
            get
            {
                return sourceAssemblyMetadatas.Count(x => string.IsNullOrEmpty(x.FailureReason)) + dependencyAssemblyMetadatas.Count(x => string.IsNullOrEmpty(x.FailureReason));
            }
        }

        public int DeveloperCommentsNumber
        {
            get { return developerComments.Count; }
        }

        public int SuccessDeveloperCommentsNumber
        {
            get { return developerComments.Count(x => string.IsNullOrEmpty(x.FailureReason)); }
        }
        #endregion

        public MRefFilesUploading()
        {
            subPhaseNames = new List<string>
            {
                "Get Assembly Metadata And Developer Comments Initialization",
                "Get Assembly Metadata From Corpnet",
                "Upload Assembly Metadata",
                "Get Developer Comments From Corpnet",
                "Upload Develoer Comments"
            };
        }

        public override string PhaseName
        {
            get { return StatusReportConstants.MREF_PHASE_UPLOADINGFILES; }
        }

        /// <summary>
        /// Perform property validation and setup internals for Apply method
        /// </summary>
        /// <param name="context">build context</param>
        public override void Setup(BuildContext context)
        {
            base.Setup(context);

            mrefAssemblySourcePaths = null;
            mrefAssemblyDependencyPaths = null;
            mrefDeveloperCommentsPaths = null;
            sourceAssemblyMetadatas = new List<FileContent>();
            dependencyAssemblyMetadatas = new List<FileContent>();
            developerComments = new List<FileContent>();
            currentSubPhase = 0;
            blobAccessor = null;
            report = new StringBuilder();
        }

        /// <summary>
        /// Download assembly metadata from corpnet through service bus and upload to blob.
        /// </summary>
        /// <param name="context">The build context shared among components.</param>
        public override bool Process(BuildContext context)
        {
            try
            {
                sw.Restart();
                if (context == null) throw new ArgumentNullException("context");

                currentSubPhase = 0;
                TraceEx.WriteLine("Get assembly metadata and developer comments initialization starts.",
                    GenerateBuildStatus(ReflectionEventStates.InProgress, "Get assembly metadata and developer comments initialization starts."));
                Init(context);
                SubPhaseComplete(1, 1, 0);

                // 1. Get Metadata
                currentSubPhase = 1;
                TraceEx.WriteLine("Get assembly metadata starts.", GenerateBuildStatus(ReflectionEventStates.InProgress, "Get assembly metadata starts."));
                GetAssemblyMetadatas(context);
                ValidateAssemblyMetadata(context);
                SubPhaseComplete(TotalAssmeblyNumber, SuccessAssemblyNumber, 0);
                TraceEx.WriteLine("Get assembly metadata ends.",
                    GenerateBuildStatus(ReflectionEventStates.InProgress, string.Format("Get {0} assembly metadata.", TotalAssmeblyNumber)));

                // 2. Upload Metadata
                currentSubPhase = 2;
                TraceEx.WriteLine("Upload assembly metadata starts.", GenerateBuildStatus(ReflectionEventStates.InProgress, "Upload assembly metadata starts."));
                UploadMetadataToBlob(context);
                SubPhaseComplete(TotalAssmeblyNumber, SuccessAssemblyNumber, 0);
                TraceEx.WriteLine("Upload assembly metadata ends.",
                    GenerateBuildStatus(ReflectionEventStates.InProgress, string.Format("Upload {0} assembly metadata.", TotalAssmeblyNumber)));
                // Quick free metadata bytes which are no longer used
                FreeMetadataBuffer();

                // 3. Get Comments
                currentSubPhase = 3;
                TraceEx.WriteLine("Get developer comments starts.", GenerateBuildStatus(ReflectionEventStates.InProgress, "Get developer comments starts."));
                GetDeveloperCommentsFiles(context);
                ValidateDeveloperComments(context);
                ModifyCommentsFilesName();
                SubPhaseComplete(DeveloperCommentsNumber, SuccessDeveloperCommentsNumber, 0);
                TraceEx.WriteLine("Get developer comments ends.",
                    GenerateBuildStatus(ReflectionEventStates.InProgress, string.Format("Get {0} developer comments metadata.", DeveloperCommentsNumber)));

                // 4. Upload Comments
                currentSubPhase = 4;
                TraceEx.WriteLine("Upload developer comments starts.", GenerateBuildStatus(ReflectionEventStates.InProgress, "Upload developer comments starts."));
                UploadDeveloperCommentsToBlob(context);
                SubPhaseComplete(DeveloperCommentsNumber, SuccessDeveloperCommentsNumber, 0);
                TraceEx.WriteLine("Upload developer comments ends.",
                    GenerateBuildStatus(ReflectionEventStates.InProgress, string.Format("Upload {0} developer comments.", DeveloperCommentsNumber)));

                currentSubPhase = 5;
                TraceEx.WriteLine("Get assembly metadata and upload ends.", GenerateBuildStatus(ReflectionEventStates.InProgress, string.Empty, GenerateReport(null)));
                FreeMetadataAndDevCommentsObject();
            }
            catch (Exception ex)
            {
                TraceEx.WriteLine(string.Format("Failed to get assembly metadata. Exception: {0}", ex.Message), GenerateBuildStatus(ReflectionEventStates.Failed, ex.Message, GenerateReport(ex)));
                return false;
            }
            finally
            {
                sw.Stop();
            }

            return true;
        }

        private void Init(BuildContext context)
        {
            serviceBusUrl = context.XmlContext.LookupVariable("ServiceBusUri").ToString();
            if (string.IsNullOrEmpty(serviceBusUrl))
            {
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, "Servicebus url is not set.", (int)Error.InvalidUri);
            }

            mrefAssemblySourcePaths = context.XmlContext.LookupVariable(MRefReflectionCommon.AssemblySourcePaths) as List<string>;
            mrefAssemblyDependencyPaths = context.XmlContext.LookupVariable(MRefReflectionCommon.AssemblyDependencyPaths) as List<string>;
            mrefDeveloperCommentsPaths = context.XmlContext.LookupVariable(MRefReflectionCommon.DeveloperCommentsPaths) as List<string>;
            if (mrefAssemblySourcePaths == null || mrefAssemblySourcePaths.Count == 0)
            {
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, "Assembly source paths is not set.", (int)Error.InvalidMRefSourcePath);
            }

            IAccessorFactory accessorFactory = AccessorFactoryHelper.Factory;
            if (accessorFactory != null)
            {
                blobAccessor = accessorFactory.CreateBlobAccessor();
            }
            if (blobAccessor == null)
            {
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, "BlobAccessor is not set.", (int)Error.InvalidBlobAccessor);
            }
        }

        private void GetAssemblyMetadatas(BuildContext context)
        {
            try
            {
                foreach (var path in mrefAssemblySourcePaths)
                {
                    sourceAssemblyMetadatas.AddRange(CallServiceBus(serviceBusUrl, o => o.DownloadAssemblyMetadata(path)));
                }
                foreach (var path in mrefAssemblyDependencyPaths)
                {
                    dependencyAssemblyMetadatas.AddRange(CallServiceBus(serviceBusUrl, o => o.DownloadAssemblyMetadata(path)));
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error happened when downloading metadata from corpnet. InnerException: {0}", ex);
                BuildComponentUtility.HandleEngineExceptionAndLog(context, ex, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, errorMessage, (int)Error.DownloadMetadataError);
            }

            if (sourceAssemblyMetadatas.Count == 0)
            {
                string errorMessage = string.Format("Get zero assembly metadata from {0}", string.Join("; ", mrefAssemblySourcePaths.ToArray()));
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, errorMessage, (int)Error.DownloadMetadataError);
            }
        }

        private void GetDeveloperCommentsFiles(BuildContext context)
        {
            try
            {
                var assemblyNames = (from f in sourceAssemblyMetadatas.Concat(dependencyAssemblyMetadatas)
                                     select Path.GetFileNameWithoutExtension(f.FileName)).ToList();
                List<string> fileNames = new List<string>();

                foreach (var path in mrefDeveloperCommentsPaths)
                {
                    List<string> files = Directory.GetFiles(path).ToList();
                    foreach (string file in files)
                    {
                        if (Path.GetExtension(file) == ".xml")
                        {
                            fileNames.Add(Path.GetFileNameWithoutExtension(file));
                        }
                    }
//                    developerComments.AddRange(CallServiceBus(this.serviceBusUrl, o => o.DownloadDeveloperComments(path, assemblyNames)));
                    developerComments.AddRange(CallServiceBus(this.serviceBusUrl, o => o.DownloadDeveloperComments(path, fileNames)));
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error happened when downloading developer comments from corpnet. InnerException: {0}", ex);
                BuildComponentUtility.HandleEngineExceptionAndLog(context, ex, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, errorMessage, (int)Error.DownloadDeveloperCommentsError);
            }
        }

        private T CallServiceBus<T>(string serviceBusUrl, Func<IContentServiceBus, T> opeartion)
        {
            if (string.IsNullOrEmpty(serviceBusUrl)) throw new ArgumentException("serviceBusUrl");

            if (_retryStrategy == null)
            {
                _retryStrategy = new ExponentialBackoff(RetryCount, TimeSpan.FromSeconds(MinRetryStrategyBackoff), TimeSpan.FromSeconds(MaxRetryStrategyBackoff), TimeSpan.FromSeconds(DeltaRetryStrategyBackoff));
            }
            if (_retryPolicy == null)
            {
                _retryPolicy = new RetryPolicy<ServiceBusTransientErrorDetectionStrategy>(_retryStrategy);
            }

            var result = default(T);
            _retryPolicy.ExecuteAction(
            () =>
            {
                IContentServiceBus sbClient = null;
                try
                {
                    sbClient = SoapServiceHelper.GetSoapBasicHttpBindingClient<IContentServiceBus>(serviceBusUrl);

                    if (sbClient != null)
                    {
                        result = opeartion(sbClient);
                        ((ICommunicationObject)sbClient).Close();
                    }
                }
                catch (Exception)
                {
                    if (sbClient != null)
                    {
                        ((ICommunicationObject)sbClient).Abort();
                    }
                    throw;
                }
            });
            return result;
        }

        private void ValidateAssemblyMetadata(BuildContext context)
        {
            if (SuccessAssemblyNumber != TotalAssmeblyNumber)
            {
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, "Some metadata is invalid.", (int)Error.DownloadMetadataError);
            }

            // validate source and dependency
            var assemblyWithSameName = from f in sourceAssemblyMetadatas.Concat(dependencyAssemblyMetadatas)
                                       group f by Path.GetFileName(f.FileName) into g
                                       where g.Count() > 1
                                       select g;
            if (assemblyWithSameName.Any())
            {
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, "There are duplicate assembly file names.", (int)Error.DuplicateAssemblyName);
            }
        }

        private void ValidateDeveloperComments(BuildContext context)
        {
            if (DeveloperCommentsNumber != SuccessDeveloperCommentsNumber)
            {
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, "Some developer comments is invalid.", (int)Error.DownloadDeveloperCommentsError);
            }

            var developerCommentsWithSameName = from f in developerComments
                                                group f by Path.GetFileNameWithoutExtension(f.FileName) into g
                                                where g.Count() > 1
                                                select g;
            if (developerCommentsWithSameName.Any())
            {
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, "There are duplicate developer comments file names.", (int)Error.DuplicateDeveloperCommentsFileName);
            }
        }

        private StatusReport.BuildStatus GenerateBuildStatus(ReflectionEventStates eventState, string comment, string report = null)
        {
            return new StatusReport.BuildStatus
            {
                ProjectId = ProjectId,
                LanguageRegionSetId = LanguageRegionSetId,
                ReflectionSequenceId = Version,
                SoapServiceURI = SoapServiceUri,
                RestServiceURI = RestServiceUri,
                TrackingId = TrackingId,
                EventState = eventState,
                Phase = StatusReportConstants.MREF_PHASE_UPLOADINGFILES,
                CompletedCount = currentSubPhase,
                TotalCount = totalSubPhase,
                Comment = comment,
                Report = report
            };
        }

        private void UploadMetadataToBlob(BuildContext context)
        {
            var uploadTasks = new List<Task>();
            try
            {
                foreach (var metadata in sourceAssemblyMetadatas)
                {
                    string blobPath = Path.GetFileName(metadata.FileName);
                    uploadTasks.Add(blobAccessor.UploadAsync(Version, BlobFileType.MetadataSource, blobPath, new MemoryStream(metadata.Buffer)));
                }
                foreach (var metadata in dependencyAssemblyMetadatas)
                {
                    string blobPath = Path.GetFileName(metadata.FileName);
                    uploadTasks.Add(blobAccessor.UploadAsync(Version, BlobFileType.MetadataDependency, blobPath, new MemoryStream(metadata.Buffer)));
                }
                if (uploadTasks.Count > 0)
                {
                    if (!Task.WaitAll(uploadTasks.ToArray(), UploadFilesToBlobTimeout))
                    {
                        string errorMessage = string.Format("Not all upload task finished in {0} milliseconds.", UploadFilesToBlobTimeout);
                        throw new TimeoutException(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error happened when uploading metadata to Azure blob.";
                BuildComponentUtility.HandleEngineExceptionAndLog(context, ex, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, errorMessage, (int)Error.UploadToBlobError);
            }
        }

        private void UploadDeveloperCommentsToBlob(BuildContext context)
        {
            var uploadTasks = new List<Task>();
            try
            {
                foreach (var comments in developerComments)
                {
                    string blobPath = Path.GetFileName(comments.FileName);
                    uploadTasks.Add(blobAccessor.UploadAsync(Version, BlobFileType.DeveloperComments, blobPath, new MemoryStream(comments.Buffer)));
                }
                if (uploadTasks.Count > 0)
                {
                    if (!Task.WaitAll(uploadTasks.ToArray(), UploadFilesToBlobTimeout))
                    {
                        string errorMessage = string.Format("Not all upload task finished in {0} milliseconds.", UploadFilesToBlobTimeout);
                        throw new TimeoutException(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error happened when uploading developer comments to Azure blob.";
                BuildComponentUtility.HandleEngineExceptionAndLog(context, ex, StatusReportConstants.MREF_PHASE_UPLOADINGFILES, errorMessage, (int)Error.UploadToBlobError);
            }
        }

        private string GenerateReport(Exception ex)
        {
            switch (currentSubPhase)
            {
                case 0:
                    report.Insert(0, string.Format("{0}{1}", StatusReport.ReportHelper.GenerateReportTitle(PhaseName, 1, 0, sw.Elapsed), Environment.NewLine));
                    report.AppendLine(StatusReport.ReportHelper.GenerateSubPhaseSummary(subPhaseNames[currentSubPhase], 1, 0, 1));
                    report.AppendLine(StatusReport.ReportHelper.GenerateCLiXInternalError(ex));
                    break;
                case 1:
                    report.Insert(0, string.Format("{0}{1}", StatusReport.ReportHelper.GenerateReportTitle(PhaseName, 1, 0, sw.Elapsed), Environment.NewLine));
                    report.AppendLine(StatusReport.ReportHelper.GenerateSubPhaseSummary(subPhaseNames[currentSubPhase], TotalAssmeblyNumber, SuccessAssemblyNumber, TotalAssmeblyNumber - SuccessAssemblyNumber));
                    AppendFailedMetadataInfoIfExists();

                    var duplicateFileNames = GetDuplicateFileNames(sourceAssemblyMetadatas.Concat(dependencyAssemblyMetadatas).Select(x => x.FileName).ToList());
                    if (duplicateFileNames.Count > 0) GetDuplicateFilesInfo(duplicateFileNames);

                    report.AppendLine(StatusReport.ReportHelper.GenerateCLiXInternalError(ex));
                    break;
                case 2:
                    report.Insert(0, string.Format("{0}{1}", StatusReport.ReportHelper.GenerateReportTitle(PhaseName, 1, 0, sw.Elapsed), Environment.NewLine));
                    report.AppendLine(StatusReport.ReportHelper.GenerateSubPhaseSummary(subPhaseNames[currentSubPhase], TotalAssmeblyNumber, 0, 1));
                    report.AppendLine(StatusReport.ReportHelper.GenerateCLiXInternalError(ex));
                    break;
                case 3:
                    report.Insert(0, string.Format("{0}{1}", StatusReport.ReportHelper.GenerateReportTitle(PhaseName, 1, 0, sw.Elapsed), Environment.NewLine));
                    report.AppendLine(StatusReport.ReportHelper.GenerateSubPhaseSummary(subPhaseNames[currentSubPhase], DeveloperCommentsNumber, SuccessDeveloperCommentsNumber, DeveloperCommentsNumber - SuccessDeveloperCommentsNumber));
                    AppendFailedDeveloperCommentsInfoIfExists();

                    var duplicateDevCommentsFileNames = GetDuplicateFileNames(developerComments.Select(x => x.FileName).ToList());
                    if (duplicateDevCommentsFileNames.Count > 0) GetDuplicateFilesInfo(duplicateDevCommentsFileNames);

                    report.AppendLine(StatusReport.ReportHelper.GenerateCLiXInternalError(ex));
                    break;
                case 4:
                    report.Insert(0, string.Format("{0}{1}", StatusReport.ReportHelper.GenerateReportTitle(PhaseName, 1, 0, sw.Elapsed), Environment.NewLine));
                    report.AppendLine(StatusReport.ReportHelper.GenerateSubPhaseSummary(subPhaseNames[currentSubPhase], DeveloperCommentsNumber, 0, 1));
                    report.AppendLine(StatusReport.ReportHelper.GenerateCLiXInternalError(ex));
                    break;
                case 5:
                    report.Insert(0, string.Format("{0}{1}", StatusReport.ReportHelper.GenerateReportTitle(PhaseName, 1, 1, sw.Elapsed), Environment.NewLine));

                    if (sourceAssemblyMetadatas.Count > 0)
                    {
                        report.AppendLine("    Source assembly names:");
                        report.Append(string.Join(Environment.NewLine, sourceAssemblyMetadatas.Select(x => "      " + x.FileName).ToArray()));
                        report.AppendLine();
                    }
                    if (dependencyAssemblyMetadatas.Count > 0)
                    {
                        report.AppendLine("    Dependency assembly names:");
                        report.Append(string.Join(Environment.NewLine, dependencyAssemblyMetadatas.Select(x => "      " + x.FileName).ToArray()));
                        report.AppendLine();
                    }
                    if (developerComments.Count > 0)
                    {
                        report.AppendLine("    Developer comments names:");
                        report.Append(string.Join(Environment.NewLine, developerComments.Select(x => "      " + x.FileName).ToArray()));
                        report.AppendLine();
                    }
                    break;
                default:
                    report = new StringBuilder("Impossible stage.");
                    break;
            }

            return report.ToString();
        }

        private List<string> GetDuplicateFileNames(IEnumerable<string> filePaths)
        {
            var duplicateFileNames = new List<string>();
            duplicateFileNames = (from path in filePaths
                                  group path by Path.GetFileName(path).ToLower() into g
                                  where g.Count() > 1
                                  from f in g
                                  select f).ToList();
            return duplicateFileNames;
        }

        private void FreeMetadataBuffer()
        {
            foreach (var metadata in sourceAssemblyMetadatas.Concat(dependencyAssemblyMetadatas))
            {
                metadata.Buffer = null;
            }
        }

        private void FreeMetadataAndDevCommentsObject()
        {
            sourceAssemblyMetadatas = null;
            dependencyAssemblyMetadatas = null;
            developerComments = null;
        }

        private void AppendFailedMetadataInfoIfExists()
        {
            if (TotalAssmeblyNumber != SuccessAssemblyNumber)
            {
                var failedMetadata = sourceAssemblyMetadatas.FindAll(x => !string.IsNullOrEmpty(x.FailureReason));
                if (failedMetadata.Count > 0)
                {
                    GetFailToDownloadFilesInfo(BlobFileType.MetadataSource, failedMetadata);
                }
                failedMetadata = dependencyAssemblyMetadatas.FindAll(x => !string.IsNullOrEmpty(x.FailureReason));
                if (failedMetadata.Count > 0)
                {
                    GetFailToDownloadFilesInfo(BlobFileType.MetadataDependency, failedMetadata);
                }
            }
        }

        private void AppendFailedDeveloperCommentsInfoIfExists()
        {
            if (DeveloperCommentsNumber != SuccessDeveloperCommentsNumber)
            {
                var failedDevComments = developerComments.FindAll(x => !string.IsNullOrEmpty(x.FailureReason));
                if (failedDevComments.Count > 0)
                {
                    GetFailToDownloadFilesInfo(BlobFileType.DeveloperComments, failedDevComments);
                }
            }
        }

        private void GetDuplicateFilesInfo(List<string> fileNames)
        {
            var errorSb = new StringBuilder();
            errorSb.Append("Duplicate file names:");
            errorSb.Append(string.Join("; ", fileNames.ToArray()));
            report.AppendLine(StatusReport.ReportHelper.GenerateReportError(errorSb));
        }

        private void GetFailToDownloadFilesInfo(BlobFileType type, List<FileContent> files)
        {
            var errorSb = new StringBuilder();
            switch (type)
            {
                case BlobFileType.MetadataSource:
                    errorSb.Append("Failed source metadata assembly names:");
                    break;
                case BlobFileType.MetadataDependency:
                    errorSb.Append("Failed dependency metadata assembly names:");
                    break;
                case BlobFileType.DeveloperComments:
                    errorSb.Append("Failed developer comments names:");
                    break;
                default:
                    throw new ArgumentException("type");
            }
            errorSb.Append(string.Join("; ", files.Select(x => string.Format("File:{0}, Reason:{1}", x.FileName, x.FailureReason)).ToArray()));
            report.AppendLine(StatusReport.ReportHelper.GenerateReportError(errorSb));
        }

        private void ModifyCommentsFilesName()
        {
            if (developerComments == null || DeveloperCommentsNumber == 0) return;           

            var assemblyNamesWithoutExt = from f in sourceAssemblyMetadatas.Concat(dependencyAssemblyMetadatas)
                select Path.GetFileNameWithoutExtension(f.FileName);

            var metadataNameDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var assemblyName in assemblyNamesWithoutExt)
            {
                metadataNameDictionary.Add(assemblyName + ".xml", assemblyName + ".xml");
            }

            foreach (var developerComment in developerComments)
            {
                var commentsFileName = Path.GetFileName(developerComment.FileName);
                var directoryName = Path.GetDirectoryName(developerComment.FileName);
                string newFileName;
                if (metadataNameDictionary.TryGetValue(commentsFileName, out newFileName))
                {
                    developerComment.FileName = Path.Combine(directoryName, newFileName);
                }
            }
        }
    }
}

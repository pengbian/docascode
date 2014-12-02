namespace Microsoft.DxBuild.Components
{
    using Microsoft.Content.Build.Core;
    using Microsoft.Content.BuildEngine.DataAccessor.Azure;
    using Microsoft.Content.BuildEngine.DataAccessor.Extensions;
    using Microsoft.Content.BuildEngine.DataAccessorInterface;
    using Microsoft.Content.BuildEngine.ReflectionTables;
    using Microsoft.Content.BuildEngine.StatusReport;
    using Microsoft.Content.BuildService;
    using Microsoft.Content.WebService;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using StatusReport = Microsoft.Content.BuildEngine.StatusReport;

    /// <summary>
    /// Do initialization work for reflection pipeline.
    /// </summary>
    public class MRefReflectionInitialization : BuildComponent
    {
        private long projectId;
        private long languageRegionSetId;
        private string reflectionSequenceId;
        private string soapServiceUri;
        private string restServiceUri;
        private string trackingId;
        private StringBuilder report = new StringBuilder();
        private int currentSubPhase = 0;
        private const int totalSubPhase = 1;
        private Stopwatch sw;
        private ITableAccessor<ProjectSettings> projectSettingAccessor;

        public MRefReflectionInitialization()
        {
            sw = new Stopwatch();
        }

        /// <summary>
        /// Perform property validation and setup internals for Apply method.
        /// </summary>
        public override void Setup(BuildContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            projectId = default(long);
            languageRegionSetId = default(long);
            reflectionSequenceId = null;
            soapServiceUri = null;
            restServiceUri = null;
            trackingId = null;
            currentSubPhase = 0;
            report = new StringBuilder();
            projectSettingAccessor = AccessorFactoryHelper.Factory.CreateTableAccessor<ProjectSettings>();
        }

        /// <summary>
        /// Do initialization work for reflection pipeline.
        /// If users rerun a project which failed at stubbing, the project would fail at MRefInitialization after the rerun.
        /// In case of multiple reruns, the lastversion of the projectsettings table should always be the version of the failure at stubbing,
        /// so lastversion can't be updated if a project fails at MRefInitialization phase.
        /// </summary>
        /// <param name="document">Not used in this component.</param>
        /// <param name="context">The build context shared among components.</param>
        public override bool Apply(XmlDocument document, BuildContext context)
        {
            try
            {
                sw.Restart();
                if (context == null) throw new ArgumentNullException("context");

                currentSubPhase = 0;
                Init(context);
                CheckReflectionStatus();     
                UpdateVersion();
                UpdateStatus();
                string byUser = context.XmlContext.LookupVariable(MRefReflectionCommon.ByUser) as string;
                ReflectionInitializeResult reflectionInitializeResult = Util.CallSoapService(
                    soapServiceUri,
                    p => p.ReflectionInitialize(projectId, languageRegionSetId, reflectionSequenceId, ReflectionEventTypes.OnDemandReflection, byUser));

                TraceEx.WriteLine("MRef reflection initialization starts.", GenerateBuildStatus(ReflectionEventStates.InProgress, "MRef reflection initialization starts."));
                context.XmlContext.AddVariable(MRefReflectionCommon.Version, reflectionSequenceId);
                context.XmlContext.AddVariable(MRefReflectionCommon.AssemblySourcePaths, reflectionInitializeResult.SourcePaths.ToList());
                context.XmlContext.AddVariable(MRefReflectionCommon.AssemblyDependencyPaths, reflectionInitializeResult.DependencyPaths.ToList());
                context.XmlContext.AddVariable(MRefReflectionCommon.DeveloperCommentsPaths, reflectionInitializeResult.CommentsPaths.ToList());
                context.XmlContext.AddVariable(MRefReflectionCommon.ArticleTypes, reflectionInitializeResult.ArticleTypes);
                SubPhaseComplete("Initialization", 1, 1, 0);

                currentSubPhase = 1;
                TraceEx.WriteLine("MRef reflection initialization completed.", GenerateBuildStatus(ReflectionEventStates.InProgress, "MRef reflection initialization complete.", GenerateReport(null)));
            }
            catch (Exception ex)
            {
                TraceEx.WriteLine("MRef reflection initialization failed.", GenerateBuildStatus(ReflectionEventStates.Failed, "MRef reflection initialization failed.", GenerateReport(ex)));
                return false;
            }
            finally
            {
                sw.Stop();
            }

            return true;
        }

        private void CheckReflectionStatus()
        {
            ProjectSettings statusSetting = projectSettingAccessor.Get(projectId.ToString(), ProjectSettingType.ReflectionStatus.ToString());
            if (statusSetting == null) return;
            if (statusSetting.Value != ReflectionStatus.Failed.ToString())
            {
                //If a project has been built successfully or in progress , right now we don't support rerun.
                throw new NotSupportedException(string.Format("Right now we don't support update. Project {0} has already been built successfully or in progress.", projectId));
            }
            ProjectSettings versionSetting = projectSettingAccessor.Get(projectId.ToString(), ProjectSettingType.LastVersion.ToString());
            if (versionSetting == null) return;                        
            ITableAccessor<BuildTask> buildTaskAccessor = AccessorFactoryHelper.Factory.CreateTableAccessor<BuildTask>();
            BuildTask buildTask = buildTaskAccessor.Get(versionSetting.Value, "ChangesItem");
            if (buildTask != null)
            {
                //If stubbing failed we don't support build twice.                   
                throw new NotSupportedException(string.Format("Project {0} failed during the stubbing phase of previous builds.You are not allowed to rerun.", projectId));
            } 
        }

        private void UpdateVersion()
        {
            ProjectSettings versionSetting = new ProjectSettings
            {
                ProjectId = projectId.ToString(),
                ProjectSettingName = ProjectSettingType.LastVersion.ToString(),
                Value = reflectionSequenceId
            };
            projectSettingAccessor.InsertOrUpdate(versionSetting);
        }

        private void UpdateStatus()
        {
            ProjectSettings statusSetting = new ProjectSettings
            {
                ProjectId = projectId.ToString(),
                ProjectSettingName = ProjectSettingType.ReflectionStatus.ToString(),
                Value = ReflectionStatus.InProgress.ToString()
            };
            projectSettingAccessor.InsertOrUpdate(statusSetting);
        }

        private string GenerateReflectionVersionNumber(long projectId)
        {
            return string.Format("{0}-{1}", projectId, FormatDateTime(DateTime.Now));
        }

        private void Init(BuildContext context)
        {
            string projectIdStr = context.XmlContext.LookupVariable(MRefReflectionCommon.ProjectId).ToString();
            if (!long.TryParse(projectIdStr, out projectId))
            {
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_INITIALIZING, "Project id is not set or invalid.", (int)Error.ProjectIdNotFoundOrInvalid);
            }

            reflectionSequenceId = GenerateReflectionVersionNumber(projectId);

            string languageRegionSetIdStr = context.XmlContext.LookupVariable(MRefReflectionCommon.LanguageRegionSetId).ToString();
            if (!long.TryParse(languageRegionSetIdStr, out languageRegionSetId))
            {
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_INITIALIZING, "Project id is not set or invalid.", (int)Error.LanguageRegionSetIdNotFoundOrInvalid);
            }

            soapServiceUri = context.XmlContext.LookupVariable(MRefReflectionCommon.SoapServiceUri).ToString();
            restServiceUri = context.XmlContext.LookupVariable(MRefReflectionCommon.RestServiceUri).ToString();
            trackingId = context.XmlContext.LookupVariable(MRefReflectionCommon.TrackingId).ToString();
            List<string> nullProperties = new List<string>();
            if (string.IsNullOrEmpty(soapServiceUri)) nullProperties.Add(MRefReflectionCommon.SoapServiceUri);
            if (string.IsNullOrEmpty(restServiceUri)) nullProperties.Add(MRefReflectionCommon.RestServiceUri);
            if (string.IsNullOrEmpty(trackingId)) nullProperties.Add(MRefReflectionCommon.TrackingId);
            if (nullProperties.Count > 0)
            {
                string message = LogMessage.ErrorMessage(Error.RequiredParametersNotSet, string.Join(" ", nullProperties.ToArray<string>()));
                BuildComponentUtility.HandleEngineExceptionAndLog(context, StatusReportConstants.MREF_PHASE_INITIALIZING, message, (int)Error.RequiredParametersNotSet);
            }
        }

        private string FormatDateTime(DateTime dt)
        {
            return string.Format("{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        private StatusReport.BuildStatus GenerateBuildStatus(ReflectionEventStates eventState, string comment, string report = null)
        {
            return new StatusReport.BuildStatus
            {
                ProjectId = projectId,
                LanguageRegionSetId = languageRegionSetId,
                ReflectionSequenceId = reflectionSequenceId,
                SoapServiceURI = soapServiceUri,
                RestServiceURI = restServiceUri,
                TrackingId = trackingId,
                EventState = eventState,
                Phase = StatusReportConstants.MREF_PHASE_INITIALIZING,
                CompletedCount = currentSubPhase,
                TotalCount = totalSubPhase,
                Comment = comment,
                Report = report
            };
        }

        private void SubPhaseComplete(string subPhaseName, int totalNum, int successfulNum, int failedNum)
        {
            report.AppendLine(StatusReport.ReportHelper.GenerateSubPhaseSummary(subPhaseName, totalNum, successfulNum, failedNum));
        }

        private string GenerateReport(Exception ex)
        {
            switch (currentSubPhase)
            {
                case 0:
                    report.Insert(0, string.Format("{0}\r\n", StatusReport.ReportHelper.GenerateReportTitle(StatusReportConstants.MREF_PHASE_INITIALIZING, 1, 0, sw.Elapsed)));
                    report.AppendLine(StatusReport.ReportHelper.GenerateSubPhaseSummary(StatusReportConstants.MREF_PHASE_INITIALIZING, 1, 0, 1));
                    report.AppendLine(StatusReport.ReportHelper.GenerateCLiXInternalError(ex));
                    break;
                case 1:
                    report.Insert(0, string.Format("{0}\r\n", StatusReport.ReportHelper.GenerateReportTitle(StatusReportConstants.MREF_PHASE_INITIALIZING, 1, 1, sw.Elapsed)));
                    break;
                default:
                    report = new StringBuilder("Impossible stage.");
                    break;
            }

            return report.ToString();
        }
    }
}

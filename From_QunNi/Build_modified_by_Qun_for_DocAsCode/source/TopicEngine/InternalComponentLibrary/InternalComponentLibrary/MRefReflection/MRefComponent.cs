namespace Microsoft.DxBuild.Components
{
    using Microsoft.Content.BuildEngine.DataAccessorInterface;
    using Microsoft.Content.BuildEngine.ReflectionTables;
    using Microsoft.Content.BuildService;
    using Microsoft.Content.WebService;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Xml;
    using StatusReport = Microsoft.Content.BuildEngine.StatusReport;

    public abstract class MRefComponent : BuildComponent
    {
        protected long ProjectId;
        protected long LanguageRegionSetId;
        protected string Version;
        protected string SoapServiceUri;
        protected string RestServiceUri;
        protected string TrackingId;
        protected ICollection<ReflectionArticleType> ArticleTypes;
        public abstract string PhaseName { get; }
        protected List<string> subPhaseNames = new List<string>();
        protected int currentSubPhase;
        protected StringBuilder report = new StringBuilder();
        protected Stopwatch sw;
        protected int totalSubPhase
        {
            get
            {
                return subPhaseNames.Count;
            }
        }

        public MRefComponent()
        {
            sw = new Stopwatch();
        }

        /// <summary>
        /// Process logic in MRef component
        /// </summary>
        public abstract bool Process(BuildContext context);

        /// <summary>
        /// Perform property validation and setup internals for Apply method
        /// </summary>
        /// <param name="context">build context</param>
        public override void Setup(BuildContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            ProjectId = default(long);
            LanguageRegionSetId = default(long);
            Version = null;
            SoapServiceUri = null;
            RestServiceUri = null;
            TrackingId = null;
            ArticleTypes = null;
        }

        /// <summary>
        /// Initialize the shared fields and call component's process logic
        /// </summary>
        /// <param name="document">Not used in MRef components.</param>
        /// <param name="context">Build context shared among components.</param>
        public override bool Apply(XmlDocument document, BuildContext context)
        {
            ITableAccessor<ProjectSettings> tableAccessor = null;
            ProjectSettings failProjectSettings = null;

            try
            {
                // Initialize the shared fileds
                ProjectId = Convert.ToInt64(context.XmlContext.LookupVariable(MRefReflectionCommon.ProjectId).ToString());
                LanguageRegionSetId = Convert.ToInt64(context.XmlContext.LookupVariable(MRefReflectionCommon.LanguageRegionSetId).ToString());
                Version = context.XmlContext.LookupVariable(MRefReflectionCommon.Version).ToString();
                SoapServiceUri = context.XmlContext.LookupVariable(MRefReflectionCommon.SoapServiceUri).ToString();
                RestServiceUri = context.XmlContext.LookupVariable(MRefReflectionCommon.RestServiceUri).ToString();
                TrackingId = context.XmlContext.LookupVariable(MRefReflectionCommon.TrackingId).ToString();
                ArticleTypes = (context.XmlContext.LookupVariable(MRefReflectionCommon.ArticleTypes)) as ICollection<ReflectionArticleType>;

                tableAccessor = AccessorFactoryHelper.Factory.CreateTableAccessor<ProjectSettings>();
                failProjectSettings = tableAccessor.Get(Convert.ToString(ProjectId), ProjectSettingType.ReflectionStatus.ToString());
                failProjectSettings.Value = ReflectionStatus.Failed.ToString();
                
                // Call component's process logic
                bool result = Process(context);
                if (!result)
                {
                    tableAccessor.Update(failProjectSettings);
                    return false;
                }

            }
            catch (Exception ex)
            {
                TraceEx.WriteLine("", GenerateBuildStatus(ReflectionEventStates.Failed, ex.Message, PhaseName, 0, 1));

                // Only for internal trace debug
                TraceEx.TraceError("Unhandled Exception Occurs:", ex);

                if (tableAccessor != null && failProjectSettings != null)
                {
                    tableAccessor.Update(failProjectSettings);
                }
                return false;
            }

            return true;
        }

        public StatusReport.BuildStatus GenerateBuildStatus(ReflectionEventStates eventState, string report, 
            string phase, int completedCount, int totalCount, string comment = "")
        {
            return new StatusReport.BuildStatus
            {
                ProjectId = ProjectId,
                LanguageRegionSetId = LanguageRegionSetId,
                ReflectionSequenceId = Version,
                EventState = eventState,
                Report = report,
                Comment = comment,
                Phase = phase,
                CompletedCount = completedCount,
                TotalCount = totalCount,
                SoapServiceURI = SoapServiceUri,
                RestServiceURI = RestServiceUri,
                TrackingId = TrackingId
            };
        }

        protected void SubPhaseComplete(int totalNum, int successfulNum, int failedNum)
        {
            report.AppendLine(StatusReport.ReportHelper.GenerateSubPhaseSummary(subPhaseNames[currentSubPhase], totalNum, successfulNum, failedNum));
        }
    }

    public enum ReflectionStatus : short
    {
        None = 0,
        InProgress = 1,
        Succeeded = 2,
        Failed = 3
    }

    public enum ProjectSettingType : short
    {
        None = 0,
        ReflectionStatus = 1,
        LastSuccessVersion = 2,
        LastVersion = 3
    }
}

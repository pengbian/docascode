namespace Microsoft.DxBuild.Components
{
    using Microsoft.Content.Build.Core;
    using Microsoft.Content.BuildEngine.MRef.Stubbing;
    using Microsoft.Content.BuildEngine.StatusReport;
    using Microsoft.Content.BuildService;

    public class StubbingTopics : MRefComponent
    {
        public override string PhaseName
        {
            get { return StatusReportConstants.MREF_PHASE_STUBBINGTOPICS; }
        }

        public override bool Process(BuildContext context)
        {
            var stubbingContext = new StubbingContext
            {
                ProjectId = ProjectId,
                LanguageRegionSetId = LanguageRegionSetId,
                SoapUri = SoapServiceUri,
                RestUri = RestServiceUri,
                ArticleTypes = ArticleTypes,
                CurrentVersion = Version,
                TrackingId = TrackingId,
                AccessorFactory = AccessorFactoryHelper.Factory,
                BuildContext = new BuildContext()
                // TODO
                // BaseVersion
            };

            return StubbingFacade.StubHierarchy(stubbingContext);
        }
    }
}

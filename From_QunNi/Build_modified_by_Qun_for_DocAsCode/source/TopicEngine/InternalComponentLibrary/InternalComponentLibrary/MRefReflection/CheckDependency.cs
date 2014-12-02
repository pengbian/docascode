namespace Microsoft.DxBuild.Components
{
    using Microsoft.Content.Build.Core;
    using Microsoft.Content.BuildEngine.DependencyCheck;
    using Microsoft.Content.BuildEngine.StatusReport;

    public class CheckDependency : MRefComponent
    {
        public override string PhaseName
        {
            get { return StatusReportConstants.MREF_PHASE_CHECKINGDEPENDENCIES; }
        }

        public override bool Process(BuildContext context)
        {
            var dependencyCheck = new DependencyCheck(ProjectId, LanguageRegionSetId, Version,
                SoapServiceUri, RestServiceUri, TrackingId);

            return dependencyCheck.Check();
        }
    }
}

namespace Microsoft.DxBuild.Components
{
    using Microsoft.Content.BuildEngine.Reflection;
    using Microsoft.Content.BuildEngine.StatusReport;

    public class ReflectMetadata : MRefComponent
    {
        public override string PhaseName
        {
            get { return StatusReportConstants.MREF_PHASE_REFLECTINGMETADATA; }
        }

        public override bool Process(BuildContext context)
        {
            var reflection = new Reflection(ProjectId, LanguageRegionSetId, Version,
                                            SoapServiceUri, RestServiceUri, TrackingId);

            return reflection.Start();
        }
    }
}

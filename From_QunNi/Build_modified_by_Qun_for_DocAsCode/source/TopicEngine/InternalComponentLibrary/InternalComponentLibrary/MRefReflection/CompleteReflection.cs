namespace Microsoft.DxBuild.Components
{
    using Microsoft.Content.Build.Core;
    using Microsoft.Content.BuildEngine.ReflectionTables;
    using Microsoft.Content.BuildEngine.StatusReport;
    using Microsoft.Content.BuildService;
    using Microsoft.Content.QueueProvider;
    using Microsoft.Content.Services.Eventing;
    using Microsoft.Content.Services.Notification;
    using Microsoft.Content.WebService;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    public class CompleteReflection : MRefComponent
    {
        public override string PhaseName
        {
            get { return StatusReportConstants.MREF_PHASE_CLEANINGUP; }
        }

        public override bool Process(BuildContext context)
        {
            try
            {
                sw.Restart();

                TraceEx.WriteLine("Cleaning up started", GenerateBuildStatus(ReflectionEventStates.InProgress, "", PhaseName, 0, 1));

                var tableAccessor = AccessorFactoryHelper.Factory.CreateTableAccessor<ProjectSettings>();

                // Update reflection status to success in ProjectSettings table
                var successProjectSettings = tableAccessor.Get(Convert.ToString(ProjectId), ProjectSettingType.ReflectionStatus.ToString());
                successProjectSettings.Value = ReflectionStatus.Succeeded.ToString();
                tableAccessor.Update(successProjectSettings);

                // Insert last success version in ProjectSettings table
                var lastSuccessVersionProjectSettings = new ProjectSettings
                {
                    ProjectId = Convert.ToString(ProjectId),
                    ProjectSettingName = ProjectSettingType.LastSuccessVersion.ToString(),
                    Value = Version
                };
                tableAccessor.Insert(lastSuccessVersionProjectSettings);

                string message = new Message
                {
                    Content = new CmsMessage
                    {
                        EntityId = ProjectId,
                        EntityTypeId = (long)EntityTypes.Project,
                        LanguageRegionSetId = (int)LanguageRegionSetId,
                        SoapServiceURI = SoapServiceUri,
                        RestServiceURI = RestServiceUri,
                        ByUser = context.XmlContext.LookupVariable(MRefReflectionCommon.ByUser) as string,
                        EntityTypeName = EntityTypes.Project.ToString(),
                        EventName = ContentEvents.MREFPROJECT_REFLECTION_COMPLETED,
                        PostTime = DateTime.UtcNow,
                        TrackingId = TrackingId,
                        ExtendedInformation = new List<ExtendedInformationPair>
                        {
                            new ExtendedInformationPair
                            {
                                Name = Constants.CmsMessageExtendedInformation.Key_VersionNumber,
                                Value = base.Version,
                            }
                        },
                    }.ToXml(),
                    Event = new Event
                    {
                        EventName = ContentEvents.MREFPROJECT_REFLECTION_COMPLETED,
                    }
                }.ToXml();

                BuildQueues.BuildWorkflowQueue.AddMessages(new[] {new QueueMessage(message)}.ToList());

                report = new StringBuilder();
                report.Append(ReportHelper.GenerateReportTitle(PhaseName, 1, 1, sw.Elapsed)).AppendLine();
                report.Append(ReportHelper.GenerateSubPhaseSummary(PhaseName, 1, 1, 0));
                TraceEx.WriteLine("Cleaning up succeeded", GenerateBuildStatus(ReflectionEventStates.Successful, report.ToString(), PhaseName, 1, 1));

            }
            catch (Exception ex)
            {
                report.Append(ReportHelper.GenerateReportTitle(PhaseName, 1, 0, sw.Elapsed)).AppendLine();
                report.Append(ReportHelper.GenerateSubPhaseSummary(PhaseName, 1, 0, 1)).AppendLine();
                report.Append(ReportHelper.GenerateCLiXInternalError(ex));
                TraceEx.WriteLine("Cleaning up failed", GenerateBuildStatus(ReflectionEventStates.Failed, report.ToString(), PhaseName, 0, 1));
                return false;
            }
            finally
            {
                sw.Stop();
            }

            return true;
        }
    }
}

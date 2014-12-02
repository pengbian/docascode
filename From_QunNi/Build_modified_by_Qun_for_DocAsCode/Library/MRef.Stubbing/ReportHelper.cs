namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.StatusReport;
    using System.Diagnostics;

    public static class ReportHelper
    {
        public static void Report(
            this StubbingContext context,
            string content = null,
            string comment = null,
            bool isFailed = false)
        {
            var status = Create(context);
            status.Report = content;
            status.Comment = comment;
            if (isFailed)
            {
                status.EventState = WebService.ReflectionEventStates.Failed;
            }
            TraceEx.WriteLine("Stubbing topics", status);
        }

        private static BuildStatus Create(StubbingContext context)
        {
            var result = new BuildStatus
            {
                ProjectId = context.ProjectId,
                LanguageRegionSetId = context.LanguageRegionSetId,
                ReflectionSequenceId = context.CurrentVersion,
                EventState = WebService.ReflectionEventStates.InProgress,
                Phase = StatusReportConstants.MREF_PHASE_STUBBINGTOPICS,
                TrackingId = context.TrackingId,
                SoapServiceURI = context.SoapUri,
                RestServiceURI = context.RestUri,
                TotalCount = context.Progress.TopicCount + context.Progress.TotalOtherTaskCount,
                // progress = commited count + completed other substages (total 5)
                CompletedCount = context.Progress.CommitedCount + context.Progress.CompletedOtherTaskCount,
            };
            if (result.TotalCount < 0)
            {
                result.TotalCount = 1;
            }
            if (result.CompletedCount < 0)
            {
                result.CompletedCount = 0;
            }
            return result;
        }
    }
}

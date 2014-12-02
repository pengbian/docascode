namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.DataAccessor.Extensions;
    using Microsoft.Content.BuildEngine.DataAccessorInterface;
    using Microsoft.Content.BuildService;
    using Microsoft.Content.WebService;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    public class StubbingFacade
    {
        /// <summary>
        /// Magic number, limit the count of item in writing to xml.
        /// </summary>
        public const int MaxItemCountInXmlFile = 8000;
        /// <summary>
        /// Magic number, limit the count of item in submitting to db.
        /// </summary>
        public const int MaxItemCountInSubmit = 100;

        private static readonly IStubbingStep[] steps = new IStubbingStep[]
        {
            new PrepareStep(),
            new DiffStep(),
            new PreprocessStep(),
            new HierarchySubmitStep(),
            new DbValidationStep(),
            new DbCommitStep(),
            new DbCleanupStep(),
            new PostProcessStep(),
        };

        public static bool StubHierarchy(StubbingContext context)
        {
            // todo : verify context
            string reportContent;
            string comment = null;
            bool result = false;
            IStubbingStep currentStep = null;
            context.Progress = StubbingProgress.Load(context.AccessorFactory, context.CurrentVersion);
            try
            {
                TraceEx.TraceInformation("Starting Stubbing topics...");
                foreach (var step in steps)
                {
                    currentStep = step;
                    if (step.ShouldExec(context))
                    {
                        step.Exec(context);
                        if (step.ShouldSendReport)
                        {
                            context.Progress.AddReportLine();
                        }
                    }
                }
                TraceEx.TraceInformation("Stubbing topics completed.");
                reportContent = string.Format(
                    "Stubbing: Successed in {0}{1}{2}",
                    context.Progress.GetTimespan(),
                    Environment.NewLine,
                    context.Progress.GetReport());
                result = true;
            }
            catch (Exception ex)
            {
                TraceEx.TraceError("Stubbing failed", ex);
                const int CommentLengthInDb = 2048;
                comment = ex.ToString();
                if (comment.Length > CommentLengthInDb)
                {
                    comment = comment.Remove(CommentLengthInDb);
                }
                if (currentStep == null || !currentStep.ShouldSendReport)
                {
                    context.Progress.AddReportLines("Environment: Total 1, Succeeded 0, Ignored 0, Failed 1, Pending 0.");
                }
                else
                {
                    context.Progress.FailedCount = 1;
                    context.Progress.AddReportLine();
                }
                context.Progress.AddReportLines("CLiX Internal Error:", 2);
                context.Progress.AddReportLines(ex.ToString(), 3);
                reportContent = string.Format(
                    "Stubbing: Failed in {0}{1}{2}",
                    context.Progress.GetTimespan(),
                    Environment.NewLine,
                    context.Progress.GetReport());
            }
            AppendLogEntries(ref reportContent, context);
            context.Report(
                content: reportContent,
                comment: comment,
                isFailed: result == false);
            return result;
        }

        private static void AppendLogEntries(ref string reportContent, StubbingContext context)
        {
            var logEntries = context.BuildContext.Warnings;
            if (logEntries.Count != 0)
            {
                var builder = new StringBuilder();
                builder.AppendFormat("{0}Schema Validation Warnings:{1}", Environment.NewLine, Environment.NewLine);
                int no = 0;
                foreach (var entry in logEntries)
                {
                    no++;
                    builder.AppendFormat("{0}.{1} {2}:{3}{4}", no, entry.UserName, entry.Name, entry.Message, Environment.NewLine);
                }
                reportContent += builder.ToString();
            }
        }
    }
}

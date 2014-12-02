namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.WebService;

    internal sealed class DbValidationStep
        : DbOperationStep
    {
        public override bool ShouldExec(StubbingContext context)
        {
            return !context.Progress.IsValidationCompleted;
        }

        protected override void InitProgress(StubbingProgress stubbingProgress)
        {
            stubbingProgress.ReportStepName = SR.StepName_DbValidation;
            stubbingProgress.GetTotal = () => stubbingProgress.TopicCount - stubbingProgress.Ignored;
            stubbingProgress.GetSuccessed = () => stubbingProgress.ValidatedCount - stubbingProgress.Ignored;
            stubbingProgress.GetIgnored = () => 0;
        }

        protected override int GetCount(StubbingContext context, IContentSoapService client)
        {
            return client.ReflectionPreprocess(context.ProjectId, context.CurrentVersion);
        }

        protected override void UpdateProcess(StubbingContext context, int count)
        {
            context.Progress.ValidatedCount = context.Progress.TopicCount - count;
            context.Progress.Save();
        }

        protected override void ExecCompleted(StubbingContext context)
        {
            context.Progress.CompletedOtherTaskCount++;
            context.Progress.Save();
            context.Report(comment: "Db validation completed.");
        }
    }
}

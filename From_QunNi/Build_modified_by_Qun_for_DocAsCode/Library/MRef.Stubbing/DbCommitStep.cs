namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.WebService;

    internal sealed class DbCommitStep
        : DbOperationStep
    {
        public override bool ShouldExec(StubbingContext context)
        {
            return !context.Progress.IsCommitCompleted;
        }

        protected override void InitProgress(StubbingProgress stubbingProgress)
        {
            stubbingProgress.ReportStepName = SR.StepName_Commit;
            stubbingProgress.GetTotal = () => stubbingProgress.TopicCount - stubbingProgress.Ignored;
            stubbingProgress.GetSuccessed = () => stubbingProgress.CommitedCount - stubbingProgress.Ignored;
            stubbingProgress.GetIgnored = () => 0;
        }

        protected override int GetCount(StubbingContext context, IContentSoapService client)
        {
            return client.ReflectionProcess(context.ProjectId, context.CurrentVersion);
        }

        protected override void UpdateProcess(StubbingContext context, int count)
        {
            context.Progress.CommitedCount = context.Progress.TopicCount - count;
            context.Progress.Save();
            context.Report(comment: string.Format(
                "commiting {0}/{1}",
                context.Progress.CommitedCount,
                context.Progress.TopicCount));
        }
    }
}

namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using System;

    internal sealed class PostProcessStep
        : IStubbingStep
    {

        public bool ShouldExec(StubbingContext context)
        {
            return !context.Progress.IsPostprocessCompleted;
        }

        public void Exec(StubbingContext context)
        {
            context.Progress.Status = SR.TaskStatus_Successed;
            context.Progress.CompletedOtherTaskCount++;
            context.Progress.Save();
            context.DataCache.Dispose();
        }

        public bool ShouldSendReport { get { return false; } }
    }
}

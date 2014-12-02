namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    public interface IStubbingStep
    {
        bool ShouldExec(StubbingContext context);
        void Exec(StubbingContext context);
        bool ShouldSendReport { get; }
    }
}

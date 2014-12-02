namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildService;
    using Microsoft.Content.WebService;
    using System;
    using System.Collections.Generic;

    internal abstract class DbOperationStep
        : IStubbingStep
    {
        public abstract bool ShouldExec(StubbingContext context);

        public void Exec(StubbingContext context)
        {
            InitProgress(context.Progress);
            Util.CallSoapService(context.SoapUri, client =>
            {
                foreach (var count in CountDown(context))
                {
                    UpdateProcess(context, count);
                }
            });
            ExecCompleted(context);
        }

        public bool ShouldSendReport { get { return true; } }

        protected IEnumerable<int> CountDown(StubbingContext context)
        {
            int count = int.MaxValue;
            while (count > 0)
            {
                int preCount = count;
                Util.CallSoapService(context.SoapUri, client => count = GetCount(context, client));
                if (count >= preCount)
                    throw new ApplicationException("Count should decrease for every call!");
                yield return count;
            }
        }

        protected abstract void InitProgress(StubbingProgress stubbingProgress);

        protected abstract int GetCount(StubbingContext context, IContentSoapService client);

        protected abstract void UpdateProcess(StubbingContext context, int count);

        protected virtual void ExecCompleted(StubbingContext context) { }
    }
}

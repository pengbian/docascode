namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.Constants;
    using Microsoft.Content.BuildEngine.DataAccessor.Extensions;
    using Microsoft.Content.WebService;
    using System;
    using System.IO;

    internal sealed class DbCleanupStep
        : DbOperationStep
    {
        public override bool ShouldExec(StubbingContext context)
        {
            return !context.Progress.IsCleanupCompleted;
        }

        protected override void InitProgress(StubbingProgress stubbingProgress)
        {
            stubbingProgress.ReportStepName = SR.StepName_Cleanup;
            stubbingProgress.GetTotal = () => stubbingProgress.TopicCount -  stubbingProgress.Ignored;
            stubbingProgress.GetSuccessed = () => stubbingProgress.CleanUpCount - stubbingProgress.Ignored;
            stubbingProgress.GetIgnored = () => 0;
        }

        protected override int GetCount(StubbingContext context, IContentSoapService client)
        {
            var result = client.ReflectionPostprocess(context.ProjectId, context.CurrentVersion);
            foreach (var item in result.EntityReflectionMappings)
            {
                Guid guid = Guid.Parse(item.ReflectionMappingId);
                var mapItem = context.IdMap[guid];
                mapItem.ArticleId = item.EntityId;
                mapItem.ArticleGuid = item.EntityGuid.ToString();
            }
            using (var stream = context.AccessorFactory.CreateBlobAccessor().OpenWrite(
                context.CurrentVersion, BlobFileType.IdMap, MRefConstants.Storage_IdMap_FileName))
            using (var sw = new StreamWriter(stream))
            {
                context.IdMap.Save(sw);
            }
            return result.UndeletedCount;
        }

        protected override void UpdateProcess(StubbingContext context, int count)
        {
            context.Progress.CleanUpCount = context.Progress.TopicCount - count;
            context.Progress.Save();
        }

        protected override void ExecCompleted(StubbingContext context)
        {
            context.Progress.CompletedOtherTaskCount++;
            context.Progress.Save();
        }
    }
}

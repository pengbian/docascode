namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.DataAccessor.Extensions;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public class DiffStep
        : IStubbingStep
    {
        public bool ShouldExec(StubbingContext context)
        {
            return !context.Progress.IsChangesGenerated;
        }

        public void Exec(StubbingContext context)
        {
            context.Progress.ReportStepName = SR.StepName_Diff;
            context.Progress.GetTotal = () => 1;
            context.Progress.GetSuccessed = () => 0;
            context.Progress.GetIgnored = () => 0;
            var comparer = HierarchyComparer.GetComparer(
                context.AccessorFactory.CreateBlobAccessor(),
                context.CurrentVersion,
                context.BaseVersion);
            var changes = comparer.ScanHierarchy(context.IdMap);
            SaveChanges(context, changes);
            context.Progress.GetSuccessed = () => 1;
        }

        public bool ShouldSendReport { get { return true; } }

        /// <summary>
        /// Save the changes, id map to blob, and record the total count to azure table
        /// </summary>
        private void SaveChanges(StubbingContext context, IEnumerable<HierarchyChanges> changes)
        {
            int count = 0;
            int fileCount = 0;
            foreach (var buffer in changes.BlockBuffer(StubbingFacade.MaxItemCountInXmlFile))
            {
                fileCount++;
                count += buffer.Count;
                var root = new XStreamingElement(
                    SR.ChangesXml_RootElement_Name,
                    (from item in buffer
                     select item.ToXml()));
                using (var stream = context.AccessorFactory.CreateBlobAccessor().OpenWrite(
                    context.CurrentVersion,
                    BlobFileType.OtherIntermediateFile,
                    string.Format(SR.ChangesFileNameFormatter, fileCount)))
                {
                    root.Save(stream);
                }
            }
            context.Progress.FileCount = fileCount;
            context.Progress.FileCapacity = StubbingFacade.MaxItemCountInXmlFile;
            context.Progress.TopicCount = count;
            context.Progress.Preprocessed = 0;
            context.Progress.Submitted = 0;
            context.Progress.Ignored = 0;
            context.Progress.ValidatedCount = 0;
            context.Progress.CommitedCount = 0;
            context.Progress.CleanUpCount = 0;
            context.Progress.TotalOtherTaskCount = 6; // diff, preprocess, submit, validate, cleanup, postprocess
            context.Progress.CompletedOtherTaskCount = 1; // diff
            context.Progress.Save();
            context.Report(comment: "Diff completed, topic count:" + count.ToString());
        }
    }
}

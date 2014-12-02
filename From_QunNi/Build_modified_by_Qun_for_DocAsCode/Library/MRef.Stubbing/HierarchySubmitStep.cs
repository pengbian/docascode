namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.DataAccessor.Extensions;
    using Microsoft.Content.BuildEngine.MRef.Caching;
    using Microsoft.Content.BuildService;
    using Microsoft.Content.WebService;
    using Microsoft.DxBuild.Components;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Xsl;

    internal sealed class HierarchySubmitStep
        : IStubbingStep
    {
        public bool ShouldExec(StubbingContext context)
        {
            return !context.Progress.IsSubmitCompleted;
        }

        public void Exec(StubbingContext context)
        {
            context.Progress.ReportStepName = SR.StepName_Submit;
            context.Progress.GetTotal = () => context.Progress.TopicCount;
            context.Progress.GetSuccessed = () => context.Progress.Submitted;
            context.Progress.GetIgnored = () => context.Progress.Ignored;
            // for continue.
//            MergeDevCommentsTransform transform = new MergeDevCommentsTransform(context.AccessorFactory.CreateBlobAccessor(), context.CurrentVersion);
            DirectlyCommentsTransform transform = new DirectlyCommentsTransform(context.AccessorFactory.CreateBlobAccessor(), context.CurrentVersion);
            int initFileIndex = context.Progress.Submitted / context.Progress.FileCapacity;
            int initElementIndex = context.Progress.Submitted % context.Progress.FileCapacity;
            for (int fileIndex = initFileIndex; fileIndex < context.Progress.FileCount; fileIndex++)
            {
                var list = GetChangeList(context, initElementIndex, fileIndex);
                foreach (var buffer in list.BlockBuffer(StubbingFacade.MaxItemCountInSubmit))
                {
                    ReflectionAddEntityInput[] input = Transform(context, buffer, transform);
                    context.Progress.Ignored += buffer.Count - input.Length;
                    if (input.Length == 0)
                        continue;
                    ValidateSchema(input, context);
                    Util.CallSoapService(context.SoapUri, client =>
                    {
                        client.ReflectionAddEntities(
                            context.ProjectId,
                            context.CurrentVersion,
                            EntityTypes.Article,
                            input);
                    });
                    context.Progress.Submitted += input.Length;
                    context.Progress.Save();
                }
                initElementIndex = 0;
            }
            context.Progress.CompletedOtherTaskCount++;
            context.Progress.Save();
            context.Report(comment: "Hierarchy submitting completed.");
        }

        public bool ShouldSendReport { get { return true; } }

        private static List<HierarchyChanges> GetChangeList(StubbingContext context, int initElementIndex, int fileIndex)
        {
            XElement element;
            using (var stream = context.AccessorFactory.CreateBlobAccessor().OpenRead(
                context.CurrentVersion,
                BlobFileType.OtherIntermediateFile,
                string.Format(SR.ChangesFileNameFormatter, fileIndex + 1)))
            {
                element = XElement.Load(stream);
            }
            var sequence = element.Elements();
            if (initElementIndex != 0)
            {
                sequence = sequence.Skip(initElementIndex);
            }
            return (from elem in sequence select HierarchyChanges.FromXml(elem)).ToList();
        }

        private ReflectionAddEntityInput[] Transform(StubbingContext context, IEnumerable<HierarchyChanges> changesList, IDdueTransform transform)
        {
            return (from changes in changesList
                    where changes.ChangeType != HierarchyChangeType.None
                    let idMapItem = context.IdMap[changes.InternalName]
                    where idMapItem.File[0] == changes.File // if one item exists in multifile, we only export it in first file
                    select new ReflectionAddEntityInput
                    {
                        Blobs = new ReflectionBlob[]
                        {
                            transform.GetDdueDocument(context, context.CurrentVersion, changes.Type, changes.InternalName, changes.File)
                        },
                        EntityName = changes.FriendlyName,
                        Level = changes.Type.ToLevel(),
                        MappingId = idMapItem.MappingGuid.ToString(),
                        Operation = Transform(changes.ChangeType),
                        ParentEntityType = (changes.NewParent == null) ? EntityTypes.Project : EntityTypes.Article,
                        ParentMappingId = (changes.NewParent == null) ? (string)null : context.IdMap[changes.NewParent].MappingGuid.ToString(),
                        SubEntityTypeId = Transform(context, changes.Type),
                    }).ToArray();
        }

        private ReflectionEntityOperations Transform(HierarchyChangeType changeType)
        {
            switch (changeType)
            {
                case HierarchyChangeType.Add:
                    return ReflectionEntityOperations.Insert;
                case HierarchyChangeType.Remove:
                    return ReflectionEntityOperations.Delete;
                case HierarchyChangeType.Update:
                    return ReflectionEntityOperations.Update;
                case HierarchyChangeType.Move:
                    return ReflectionEntityOperations.Move;
                default:
                    throw new ArgumentOutOfRangeException("changeType");
            }
        }

        private static long Transform(StubbingContext context, HierarchyType type)
        {
            var templateName = HierarchyTypeHelper.GetDdueTemplateName(type);
            if (templateName == null)
                throw new ArgumentOutOfRangeException("type", string.Format("HierarchyType({0}) is invalid.", type));
            return (from t in context.ArticleTypes
                    where string.Equals(t.ArticleType.ArticleTypeName, templateName, StringComparison.OrdinalIgnoreCase)
                    select t.ArticleType.ArticleTypeId).First();
        }

        private void ValidateSchema(ReflectionAddEntityInput[] input, StubbingContext context)
        {
            Validate vcomponent = new Validate
            {
                XsdFilePath = ServiceConfig.buildcomponentsetupfiles + "/DdueSchema/developer.xsd",
            };
            foreach(var entry in input)
            {
                var doc = new XmlDocument();
                doc.Load(new MemoryStream(entry.Blobs.SingleOrDefault().BlobStorage));
                vcomponent.Setup(context.BuildContext);
                vcomponent.Apply(doc, context.BuildContext);
            }
        }
    }
}

namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.Build.ReflectionXmlSyntax;
    using Microsoft.Content.BuildEngine.Constants;
    using Microsoft.Content.BuildEngine.DataAccessor.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal sealed class PreprocessStep
        : IStubbingStep
    {
        public bool ShouldExec(StubbingContext context)
        {
            return !context.Progress.IsPreprocessCompleted;
        }

        public void Exec(StubbingContext context)
        {
            context.Progress.ReportStepName = SR.StepName_Preprocess;
            context.Progress.GetTotal = () => context.Progress.TopicCount;
            context.Progress.GetSuccessed = () => context.Progress.Preprocessed;
            context.Progress.GetIgnored = () => 0;

            var syntax = GetSyntaxVisitor(context);
            var dict = new Dictionary<string, List<HierarchyInformation>>();
            for (int fileIndex = 0; fileIndex < context.Progress.FileCount; fileIndex++)
            {
                var list = GetChangeList(context, fileIndex);
                list.ForEach(item =>
                {
                    switch (item.ChangeType)
                    {
                        case HierarchyChangeType.None:
                            context.IdMap.GetOrCreateLink(item.InternalName, item.NewParent);
                            break;
                        case HierarchyChangeType.Add:
                        case HierarchyChangeType.Update:
                            context.IdMap.GetOrCreate(item.InternalName, item.File);
                            item.FriendlyName = syntax.GetHierarchyText(item.InternalName, Path.GetFileNameWithoutExtension(item.File));
                            AddItem(item, dict, fileIndex);
                            break;
                        case HierarchyChangeType.Remove:
                        case HierarchyChangeType.Move:
                            break;
                        default:
                            throw new InvalidDataException(string.Format("The change type out of range: {0}", item.ChangeType));
                    }
                });
                SaveChangeList(context, fileIndex, list);
                context.Progress.Preprocessed += list.Count;
            }

            // handle duplication
            var renamedItems = RenameDuplication(FindDuplication(dict), context, syntax).ToList();
            CheckDuplication(dict);
            UpdateHierarchyItems(renamedItems, context);

            SaveIdMap(context);
            context.Progress.CompletedOtherTaskCount++;
            context.Report(comment: "All hierarchy text generated.");
        }

        public bool ShouldSendReport { get { return true; } }

        private static HierarchySyntaxVisitor GetSyntaxVisitor(StubbingContext context)
        {
            var blobAccessor = context.AccessorFactory.CreateBlobAccessor();
            string coreFile;
            using (var stream = blobAccessor.OpenRead(context.CurrentVersion, BlobFileType.IdMap, MRefConstants.Storage_CoreFile_FileName))
            using (var reader = new StreamReader(stream))
            {
                coreFile = Path.GetFileNameWithoutExtension(reader.ReadToEnd());
            }
            var files = new HashSet<string>(
                from f in blobAccessor.EnumerateFiles(context.CurrentVersion, BlobFileType.IntergratedReflectionXml)
                select Path.GetFileNameWithoutExtension(f));
            return new HierarchySyntaxVisitor(coreFile, GetDocumentLoader(context, files));
        }

        private static Func<string, XPathDocument> GetDocumentLoader(StubbingContext context, HashSet<string> files)
        {
            return file =>
            {
                using (var stream = context.AccessorFactory.CreateBlobAccessor().OpenRead(
                    context.CurrentVersion,
                    files.Contains(file) ? BlobFileType.IntergratedReflectionXml : BlobFileType.StandaloneReflectionXml,
                    file + ".xml"))
                {
                    return new XPathDocument(stream);
                }
            };
        }

        private static List<HierarchyChanges> GetChangeList(StubbingContext context, int fileIndex)
        {
            XElement element;
            using (var stream = context.AccessorFactory.CreateBlobAccessor().OpenRead(
                context.CurrentVersion,
                BlobFileType.OtherIntermediateFile,
                string.Format(SR.ChangesFileNameFormatter, fileIndex + 1)))
            {
                element = XElement.Load(stream);
            }
            return (from elem in element.Elements() select HierarchyChanges.FromXml(elem)).ToList();
        }

        private static void SaveChangeList(StubbingContext context, int fileIndex, List<HierarchyChanges> changes)
        {
            var root = new XStreamingElement(
                SR.ChangesXml_RootElement_Name,
                from item in changes select item.ToXml());
            using (var stream = context.AccessorFactory.CreateBlobAccessor().OpenWrite(
                context.CurrentVersion,
                BlobFileType.OtherIntermediateFile,
                string.Format(SR.ChangesFileNameFormatter, fileIndex + 1)))
            {
                root.Save(stream);
            }
        }

        private static void SaveIdMap(StubbingContext context)
        {
            using (var stream = context.AccessorFactory.CreateBlobAccessor().OpenWrite(
                context.CurrentVersion,
                BlobFileType.IdMap,
                MRefConstants.Storage_IdMap_FileName))
            using (var sw = new StreamWriter(stream))
            {
                context.IdMap.Save(sw);
            }
        }

        private static void AddItem(HierarchyChanges changeItem, Dictionary<string, List<HierarchyInformation>> dict, int fileIndex)
        {
            var parent = changeItem.NewParent ?? string.Empty;
            var item = new HierarchyInformation(changeItem.InternalName, parent, changeItem.FriendlyName, fileIndex);
            List<HierarchyInformation> list;
            if (!dict.TryGetValue(parent, out list))
            {
                dict.Add(parent, new List<HierarchyInformation> { item });
            }
            else
            {
                list.Add(item);
            }
        }

        private static IEnumerable<HierarchyInformation> FindDuplication(Dictionary<string, List<HierarchyInformation>> dict)
        {
            return from pair in dict
                   from result in
                       from info in pair.Value
                       group info by info.FriendlyName into g
                       where g.Select(x => x.InternalName).Distinct().Count() > 1
                       from item in g
                       select item
                   select result;
        }

        private static void CheckDuplication(Dictionary<string, List<HierarchyInformation>> dict)
        {
            var list = FindDuplication(dict).ToList();
            if (list.Count > 0)
            {
                var strs = new List<string> { "Find duplication in hierarchy text:" };
                foreach (var item in list)
                {
                    strs.Add(string.Format("  Parent:{0}, Internal Name:{1}, Hierarchy Text:{2}", item.Parent, item.InternalName, item.FriendlyName));
                }
                throw new InvalidDataException(string.Join(Environment.NewLine, strs.ToArray()));
            }
        }

        private static IEnumerable<HierarchyInformation> RenameDuplication(
            IEnumerable<HierarchyInformation> duplications,
            StubbingContext context,
            HierarchySyntaxVisitor syntax)
        {
            return from item in duplications
                   group item by item.FileIndex into g
                   let cl = GetChangeList(context, g.Key)
                   from result in
                       from info in g
                       from hi in cl
                       where info.InternalName == hi.InternalName
                       let friendlyName = syntax.GetHierarchyText(hi.InternalName, Path.GetFileNameWithoutExtension(hi.File), showCMod: true)
                       where friendlyName != info.FriendlyName
                       select info.UpdateFriendlyName(friendlyName)
                   select result;
        }

        private static void UpdateHierarchyItems(
            IEnumerable<HierarchyInformation> renamedItems,
            StubbingContext context)
        {
            foreach (var g in from item in renamedItems
                              group item by item.FileIndex)
            {
                var cl = GetChangeList(context, g.Key);
                foreach (var item in g)
                {
                    var changes = cl.Find(x => x.InternalName == item.InternalName);
                    Debug.Assert(changes != null, "Cannot find HierarchyChanges!");
                    changes.FriendlyName = item.FriendlyName;
                }
                SaveChangeList(context, g.Key, cl);
            }
        }

        private sealed class HierarchyInformation
        {
            public HierarchyInformation(string internalName, string parent, string friendlyName, int fileIndex)
            {
                InternalName = internalName;
                Parent = parent;
                FriendlyName = friendlyName;
                FileIndex = fileIndex;
            }
            public string InternalName { get; private set; }
            public string Parent { get; private set; }
            public string FriendlyName { get; private set; }
            public int FileIndex { get; private set; }
            public HierarchyInformation UpdateFriendlyName(string friendlyName)
            {
                FriendlyName = friendlyName;
                return this;
            }
        }
    }
}

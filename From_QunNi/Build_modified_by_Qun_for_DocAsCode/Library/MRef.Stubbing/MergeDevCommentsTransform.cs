namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.DataAccessor.Extensions;
    using Microsoft.Content.BuildEngine.DataAccessorInterface;
    using Microsoft.Content.WebService;
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Xsl;


    public class MergeDevCommentsTransform : IDdueTransform
    {

        #region Fields

        private IBlobAccessor blobAccessor;
        private string version;
        private DeveloperCommentsTransform commentsTransform;
        private string currentFileName;

        #endregion

        public MergeDevCommentsTransform(IBlobAccessor blobAccessor, string version)
        {
            this.blobAccessor = blobAccessor;
            this.version = version;
            this.commentsTransform = InitDeveloperCommentsTransform();
            this.currentFileName = null;
        }

        private DeveloperCommentsTransform InitDeveloperCommentsTransform()
        {
            var transform = new XslCompiledTransform();
            var settings = new XsltSettings(true, true);
            using (var fs = File.OpenRead(Path.Combine(BuildService.ServiceConfig.buildcomponentsetupfiles, "DeveloperCommentsTransform.xsl")))
            {
                transform.Load(XmlReader.Create(fs), settings, new XmlUrlResolver());
            }
            return new DeveloperCommentsTransform(transform);
        }

        private Tuple<long, XslCompiledTransform> GetDdueTemplate(StubbingContext context, HierarchyType type)
        {
            var templateName = HierarchyTypeHelper.GetDdueTemplateName(type);
            if (templateName == null)
                throw new ArgumentOutOfRangeException("type", string.Format("HierarchyType({0}) is invalid.", type));
            return context.TransformDic[templateName];
        }

        public ReflectionBlob GetDdueDocument(StubbingContext context, string version, HierarchyType type, string entityName, string fileName)
        {
            var transformContext = GetDdueTemplate(context, type);

            SetCurrentDevCommentNav(fileName);

            var result = new ReflectionBlob { BlobTypeId = transformContext.Item1 };

            var writerSettings = new XmlWriterSettings { Indent = true };
            var entityNavigator = context.DataCache.GetCodeEntity(version, entityName);

            var ms = new MemoryStream();
            var transform = transformContext.Item2;

            var xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:mref-extensions", commentsTransform);

            using (var xmlWriter = XmlWriter.Create(ms, writerSettings))
            {
                transform.Transform(entityNavigator, xslArg, xmlWriter);
            }

            result.BlobStorage = ms.ToArray();
            return result;
        }

        private void SetCurrentDevCommentNav(string fileName)
        {
            if (String.IsNullOrEmpty(currentFileName) || currentFileName != fileName)
            {
                currentFileName = fileName;
                if (blobAccessor.Exists(version, BlobFileType.DeveloperComments, fileName))
                {
                    using (var fs = blobAccessor.OpenRead(version, BlobFileType.DeveloperComments, fileName))
                    {
                        commentsTransform.CurrentCommentDocNav = new XPathDocument(fs).CreateNavigator();
                    }
                }
                else
                {
                    commentsTransform.CurrentCommentDocNav = null;
                }
            }
        }
    }
}

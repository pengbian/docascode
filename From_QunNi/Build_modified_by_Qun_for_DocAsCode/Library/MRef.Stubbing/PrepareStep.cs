namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.MRef.Caching;
    using Microsoft.Content.BuildEngine.StatusReport;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Xsl;

    internal sealed class PrepareStep
        : IStubbingStep
    {
        public bool ShouldExec(StubbingContext context)
        {
            return true;
        }

        public void Exec(StubbingContext context)
        {
            if (context.BaseVersion == null)
            {
                context.IdMap = new IdMap();
                context.DataCache = new ReflectionDataCache(s => context.IdMap);
                InitializeTransformDic(context);
            }
            else
            {
                throw new NotImplementedException("Upgrade is not implemented in this release.");
            }
            TraceEx.WriteLine("Stubbing topics", GenerateBuildStatus(context));
        }

        public bool ShouldSendReport { get { return false; } }

        private void InitializeTransformDic(StubbingContext context)
        {
            context.TransformDic = new Dictionary<string, Tuple<long, XslCompiledTransform>>();
            foreach (HierarchyType type in Enum.GetValues(typeof(HierarchyType)))
            {
                var templateName = HierarchyTypeHelper.GetDdueTemplateName(type);
                if (templateName != null)
                {
                    var templateBlob = (from t in context.ArticleTypes
                                        where string.Equals(t.ArticleType.ArticleTypeName, templateName, StringComparison.OrdinalIgnoreCase)
                                        select t.BlobStorage).FirstOrDefault();
                    if (templateBlob == null)
                        throw new ApplicationException(string.Format("Cannot find article template({0})", type.ToString()));

                    var transform = new XslCompiledTransform();
                    var settings = new XsltSettings(true, true);

                    using (var memoryStream = new MemoryStream(templateBlob.BlobStorage))
                    using (var templateReader = XmlReader.Create(memoryStream))
                    {
                        transform.Load(templateReader, settings, new XmlUrlResolver());
                    }

                    context.TransformDic.Add(templateName, Tuple.Create(templateBlob.BlobTypeId, transform));
                }
            }
        }

        private StatusReport.BuildStatus GenerateBuildStatus(StubbingContext context)
        {
            return new StatusReport.BuildStatus
            {
                ProjectId = context.ProjectId,
                LanguageRegionSetId = context.LanguageRegionSetId,
                ReflectionSequenceId = context.CurrentVersion,
                EventState = WebService.ReflectionEventStates.InProgress,
                Phase = StatusReportConstants.MREF_PHASE_STUBBINGTOPICS,
                TrackingId = context.TrackingId,
                SoapServiceURI = context.SoapUri,
                RestServiceURI = context.RestUri,
                CompletedCount = 0,
                TotalCount = 1,
                Comment = "Stubbing topics starts.",
                Report = string.Empty
            };
        }

    }
}

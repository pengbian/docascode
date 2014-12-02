namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.DataAccessorInterface;
    using Microsoft.Content.BuildEngine.MRef.Caching;
    using Microsoft.Content.WebService;
    using Microsoft.DxBuild.Components;
    using System;
    using System.Collections.Generic;
    using System.Xml.Xsl;

    public class StubbingContext
    {
        public IAccessorFactory AccessorFactory { get; set; }
        public long ProjectId { get; set; }
        public string BaseVersion { get; set; }
        public string CurrentVersion { get; set; }
        public string SoapUri { get; set; }
        public ICollection<ReflectionArticleType> ArticleTypes { get; set; }
        public long LanguageRegionSetId { get; set; }
        public string TrackingId { get; set; }
        public string RestUri { get; set; }
        internal IdMap IdMap { get; set; }
        internal StubbingProgress Progress { get; set; }
        internal ReflectionDataCache DataCache { get; set; }
        internal Dictionary<string, Tuple<long, XslCompiledTransform>> TransformDic { get; set; }
        public BuildContext BuildContext { get; set; }
    }
}

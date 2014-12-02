namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System.Xml.Xsl;

    public class DeveloperCommentsTransform
    {
        #region Fields

        private readonly XslCompiledTransform _devCommentsXslCompiledTransform;

        #endregion

        #region Properties

        public XPathNavigator CurrentCommentDocNav { get; set; }

        #endregion

        public DeveloperCommentsTransform(XslCompiledTransform transform)
        {
            _devCommentsXslCompiledTransform = transform;
        }
        
        public NodesIterator GetDeveloperComments(string entityName, string relativeXPath)
        {
            var list = new List<XPathNavigator>();

            if (CurrentCommentDocNav == null) return new NodesIterator(list);

            var nodes = CurrentCommentDocNav.Select(string.Format("/doc/members/member[@name='{0}']/{1}", entityName, relativeXPath));
            if (nodes.Count == 0) return new NodesIterator(list);

            // For exception, example and seealso tag, possibly multiple nodes; otherwise one node
            list.AddRange(from XPathNavigator node in nodes select TransformCore(node).CreateNavigator());

            return new NodesIterator(list);
        }

        private XElement TransformCore(XPathNavigator navigator)
        {
            if (navigator == null) throw new ArgumentNullException("navigator");

            var document = new XDocument();
            using (var xmlWriter = document.CreateWriter())
            {
                _devCommentsXslCompiledTransform.Transform(navigator, xmlWriter);
            }

            return document.Root;
        }
    }
}

namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using System.Xml.Linq;

    public class HierarchyChanges
    {
        public HierarchyChangeType ChangeType { get; set; }
        public HierarchyType Type { get; set; }
        public string InternalName { get; set; }
        public string FriendlyName { get; set; }
        public string Assembly { get; set; }
        public string File { get; set; }
        public string OldParent { get; set; }
        public string NewParent { get; set; }

        public static HierarchyChanges FromXml(XElement item)
        {
            return new HierarchyChanges
            {
                ChangeType = (HierarchyChangeType)(int)item.Attribute(SR.ChangesXml_Attribute_ChangeType),
                Type = (HierarchyType)(int)item.Attribute(SR.ChangesXml_Attribute_ElementType),
                InternalName = (string)item.Attribute(SR.ChangesXml_Attribute_InternalName),
                FriendlyName = (string)item.Attribute(SR.ChangesXml_Attribute_FriendlyName),
                Assembly = (string)item.Attribute(SR.ChangesXml_Attribute_Assembly),
                File = (string)item.Attribute(SR.ChangesXml_Attribute_File),
                OldParent = (string)item.Attribute(SR.ChangesXml_Attribute_OldParent),
                NewParent = (string)item.Attribute(SR.ChangesXml_Attribute_NewParent),
            };
        }

        public XElement ToXml()
        {
            return new XElement(SR.ChangesXml_Element_Name,
                new XAttribute(SR.ChangesXml_Attribute_ChangeType, (int)ChangeType),
                new XAttribute(SR.ChangesXml_Attribute_ElementType, (int)Type),
                new XAttribute(SR.ChangesXml_Attribute_InternalName, InternalName),
                FriendlyName != null ? new XAttribute(SR.ChangesXml_Attribute_FriendlyName, FriendlyName) : null,
                Assembly != null ? new XAttribute(SR.ChangesXml_Attribute_Assembly, Assembly) : null,
                new XAttribute(SR.ChangesXml_Attribute_File, File),
                NewParent != null ? new XAttribute(SR.ChangesXml_Attribute_NewParent, NewParent) : null,
                OldParent != null ? new XAttribute(SR.ChangesXml_Attribute_OldParent, OldParent) : null);
        }
    }
}

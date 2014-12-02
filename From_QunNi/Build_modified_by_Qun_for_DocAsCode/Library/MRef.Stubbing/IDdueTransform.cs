namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.WebService;
    using System;
    using System.Xml.Xsl;

    public interface IDdueTransform
    {
        ReflectionBlob GetDdueDocument(StubbingContext context, string version, HierarchyType type, string entityName, string fileName);
    }
}

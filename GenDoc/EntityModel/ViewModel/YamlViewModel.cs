using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EntityModel.ViewModel
{
    /// <summary>
    /// Defines the layout for an API page
    /// </summary>
    public enum LayoutItem
    {
        Title,
        Hierarchy,
        Exception,
        Syntax,
        InlineComments,
        ExternalComments,
        SeeAlso,
        MemberTable,
    }

    public class SourceDetail
    {
        public string RemoteUrl { get; set; }
        public string Path { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public string Content { get; set; }
        public bool IsExtern { get; set; }
    }

    public class SyntaxDetail
    {
        public Dictionary<SyntaxLanguage, string> Content { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "parameters")]
        public List<YamlItemParameterViewModel> Parameters { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "return")]
        public YamlItemParameterViewModel Return { get; set; }
    }

    public class LinkDetail
    {
        [YamlDotNet.Serialization.YamlMember(Alias = "id")]
        public string Name { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "href")]
        public string Href { get; set; }
        public int Height { get; set; }
        public bool IsExtern { get; set; }
    }

    public class ItemType
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class YamlItemViewModel : ICloneable
    {
        [YamlDotNet.Serialization.YamlMember(Alias = "id")]
        public string Name { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "yaml")]
        public string YamlPath { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "href")]
        public string Href { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "name")]
        public Dictionary<SyntaxLanguage, string> DisplayNames { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "qualifiedName")]
        public Dictionary<SyntaxLanguage, string> DisplayQualifiedNames { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "parent")]
        public string ParentName { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "type")]
        public MemberType Type { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "source")]
        public SourceDetail Source { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "documentation")]
        public SourceDetail Documentation { get; set; }

        public List<LayoutItem> Layout { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "summary")]
        public string Summary { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "remarks")]
        public string Remarks { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "syntax")]
        public SyntaxDetail Syntax { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "inheritence")]
        public List<LinkDetail> Inheritence { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "itemTypes")]
        public List<ItemType> ItemTypes { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "items")]
        public List<YamlItemViewModel> Items { get; set; }

        public override string ToString()
        {
            return Type + ": " + Name;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class YamlViewModel
    {
        public YamlItemViewModel TocYamlViewModel { get; set; }
        public Dictionary<string, IndexYamlItemViewModel> IndexYamlViewModel { get; set; }
        public List<YamlItemViewModel> MemberYamlViewModelList { get; set; }
    }

    public class IndexYamlItemViewModel
    {
        public string Name { get; set; }
        public string YamlPath { get; set; }
        public string Href { get; set; }
    }

    public class YamlItemParameterViewModel
    {
        [YamlDotNet.Serialization.YamlMember(Alias = "id")]
        public string Name { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "type")]
        public LinkDetail Type { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "description")]
        public string Description { get; set; }
    }
}

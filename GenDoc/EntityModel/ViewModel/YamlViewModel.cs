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
    }

    public class SyntaxDetail
    {
        public SyntaxLanguage Language { get; set; }
        public string Content { get; set; }

        public List<YamlItemParameterViewModel> Parameters { get; set; }
        public YamlItemParameterViewModel Return { get; set; }
    }

    public class LinkDetail
    {
        public string Name { get; set; }
        public string Href { get; set; }
        public int Height { get; set; }
    }

    public class ItemType
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class YamlItemViewModel
    {
        [YamlDotNet.Serialization.YamlMember(Alias = "name")]
        public string Name { get; set; }
        public string Href { get; set; }
        public string YamlPath { get; set; }
        public List<LayoutItem> Layout { get; set; }
        public MemberType Type { get; set; } 
        public SourceDetail Source { get; set; }
        public SourceDetail Documentation { get; set; }
        public string Summary { get; set; }

        public List<SyntaxDetail> Syntax { get; set; }
        public List<LinkDetail> Inheritence { get; set; }
        public List<ItemType> ItemTypes { get; set; }

        public List<YamlItemViewModel> Items { get; set; }
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
        public string Name { get; set; }
        public LinkDetail Type { get; set; }
        public string Description { get; set; }
    }
}

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

    public class MarkdownYamlViewModel : List<string>
    {
    }

    public class TocYamlItemViewModel
    {
        public string DisplayName { get; set; }
        public string Identity { get; set; }
        public string Url { get; set; }
    }

    public class TocYamlViewModel : List<TocYamlItemViewModel>
    {
    }

    public class YamlViewModel
    {
        public IndexYamlViewModel IndexYamlViewModel { get; set; }
        public MarkdownYamlViewModel MarkdownYamlViewModel { get; set; }
        public TocYamlViewModel TocYamlViewModel { get; set; }
    }

    /// <summary>
    /// Principle of design: each memeber can be a seperate YamlViewModel
    /// </summary>
    public class IndexYamlViewModel : IScopeViewModel
    {
        public string BaseUrl { get; set; }
        public string YamlUrl { get; set; }

        public string RepositoryUrl { get; set; }
        public List<LayoutItem> Layout { get; set; }

        public Dictionary<string, YamlItemViewModel> Items { get; set; }
    }

    /// <summary>
    /// {} wraps a Scope, inside a scope, the following properties are shared if it is not overridden
    /// </summary>
    public interface IScopeViewModel
    {
        /// <summary>
        /// The layout of the containing members
        /// </summary>
        List<LayoutItem> Layout { get; set; }

        /// <summary>
        /// The repository url for the source of the code base
        /// </summary>
        string RepositoryUrl { get; set; }

        /// <summary>
        /// The base url for current domain, If not set, inherit from the parent one
        /// </summary>
        string BaseUrl { get; set; }

        /// <summary>
        /// The url path for current yaml file
        /// </summary>
        string YamlUrl { get; set; }
    }

    public interface IMemberViewModel : IScopeViewModel
    {
        string Identity { get; set; }
        string Url { get; }
        string DisplayName { get; }
        MemberType MemberType { get; set; }
        string RelativeSourceFilePath { get; set; }
        string Comment { get; set; }
        int CommentLineNumber { get; set; }
        string Syntax { get; set; }

        /// <summary>
        /// Member's yaml file, could be current file or link to other files
        /// </summary>
        string ItemYamlUrl { get; set; }
        List<string> Hierarchy { get; set; }
        AssemblyDetail Assembly { get; set; }
        List<YamlItemParameterViewModel> Parameters { get; set; }
    }

    public class YamlItemViewModel : CommentViewModelBase, IMemberViewModel
    {
        public string Identity { get; set; }
        public string DisplayName
        {
            get
            {
                return Identity.Substring(2);
            }
        }
        public string Url
        {
            get
            {
                return DisplayName;
            }
        }

        public string BaseUrl { get; set; }
        public string YamlUrl { get; set; }
        public string RepositoryUrl { get; set; }

        public string RelativeSourceFilePath { get; set; }

        public List<LayoutItem> Layout { get; set; }

        /// <summary>
        /// Member's yaml file, could be current file or link to other files
        /// </summary>
        public string ItemYamlUrl { get; set; }
        public List<string> Items { get; set; }

        public MemberType MemberType { get; set; }

        public string Syntax { get; set; }
        public List<string> Hierarchy { get; set; }
        public AssemblyDetail Assembly { get; set; }
        public List<YamlItemParameterViewModel> Parameters { get; set; }
    }

    public class AssemblyDetail
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Path { get; set; }
    }

    public class YamlItemParameterViewModel : CommentViewModelBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ParameterType ParameterType { get; set; }
    }

    public class CommentViewModelBase
    {
        public string Comment { get; set; }
        public int CommentLineNumber { get; set; }
    }

    public enum ParameterType
    {
        Input,
        Return,
    }
}

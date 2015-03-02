using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EntityModel.ViewModel
{
    /// <summary>
    /// TODO: Rough idea, need refactor...
    /// </summary>
    public static class ViewModelExtension
    {
        public static string ToComment(this IDescription des)
        {
            var raw = des.Comments[0].Raw;
            XmlDocument doc = new XmlDocument();

            // Really Rough, need change
            doc.LoadXml(raw);
            return doc.InnerText;
        }

        public static int ToCommentLineNumber(this IDescription des)
        {
            return des.Comments[0].StartLine;
        }

        public static string ToId(this Identity id)
        {
            if (id == null)
            {
                return null;
            }

            string str = id.ToString();
            if (str.StartsWith("{") || str.Length < 3)
            {
                return str;
            }

            return id.ToString().Substring(2);
        }

        public static YamlViewModel ToYamlViewModel(this ProjectMetadata projectMetadata)
        {
            // 1. Merge project metadata list
            YamlViewModel yaml = new YamlViewModel();
            yaml.BaseUrl = "https://localhost/";
            yaml.Items = new Dictionary<string, YamlItemViewModel>();
            yaml.YamlUrl = "test.yaml";
            yaml.DefaultLayoutItems = new List<LayoutItem>
            {
                LayoutItem.Title,
                LayoutItem.Hierarchy,
                LayoutItem.Syntax,
                LayoutItem.InlineComments,
                LayoutItem.ExternalComments,
                LayoutItem.Exception,
                LayoutItem.SeeAlso,
                LayoutItem.MemberTable,
            };

            foreach (var ns in projectMetadata.Namespaces)
            {
                YamlItemViewModel item = new YamlItemViewModel();
                string[] assemblyInfos = ns.Value.AssemblyName.Split(new char[] { ' ', ',' });
                var version = assemblyInfos[1].Replace("Version=", string.Empty);
                if (string.IsNullOrWhiteSpace(version))
                {
                    version = "1.0.0.0";
                }
                item.Assembly = new AssemblyDetail()
                {
                    Name = assemblyInfos[0],
                    Version = version,
                };
                item.Identity = ns.Key.ToId();
                item.Members = new List<string>();
                item.MemberType = ns.Value.MemberType;
                foreach (var member in ns.Value.Members)
                {
                    YamlItemViewModel memberItem = new YamlItemViewModel();
                    memberItem.Identity = member.Identity.ToId();
                    memberItem.Assembly = item.Assembly;
                    var csharpSyntax = member.SyntaxDescriptionGroup[SyntaxLanguage.CSharp];
                    memberItem.Comment = csharpSyntax.ToComment();
                    memberItem.CommentLineNumber = csharpSyntax.ToCommentLineNumber();
                    memberItem.Syntax = csharpSyntax.Syntax;
                    memberItem.Hierarchy = member.InheritanceHierarchy.Select(s => s.ToString().Substring(2)).ToList();
                    memberItem.MemberType = member.MemberType;
                    switch (csharpSyntax.SyntaxType)
                    {
                        case SyntaxType.MethodSyntax:
                            {
                                var description = (MethodSyntaxDescription)csharpSyntax;
                                List<YamlItemParameterViewModel> itemParams = new List<YamlItemParameterViewModel>();
                                foreach (var desc in description.Parameters)
                                {
                                    YamlItemParameterViewModel yamlParameter = new YamlItemParameterViewModel();
                                    yamlParameter.Comment = description.ToComment();
                                    yamlParameter.CommentLineNumber = description.ToCommentLineNumber();
                                    yamlParameter.Type = desc.Type.ToId();
                                    yamlParameter.ParameterType = ParameterType.Input;
                                    yamlParameter.Name = desc.Name;
                                    itemParams.Add(yamlParameter);
                                }

                                YamlItemParameterViewModel yamlReturn = new YamlItemParameterViewModel();
                                var returnPara = description.Return;
                                yamlReturn.Comment = returnPara.ToComment();
                                yamlReturn.CommentLineNumber = returnPara.ToCommentLineNumber();
                                yamlReturn.Name = returnPara.Name;
                                yamlReturn.Type = returnPara.Type.ToId();
                                yamlReturn.ParameterType = ParameterType.Return;
                                itemParams.Add(yamlReturn);

                                memberItem.Parameters = itemParams;
                                break;
                            }
                        case SyntaxType.ConstructorSyntax:
                            {
                                var description = (ConstructorSyntaxDescription)csharpSyntax;
                                List<YamlItemParameterViewModel> itemParams = new List<YamlItemParameterViewModel>();
                                foreach (var desc in description.Parameters)
                                {
                                    YamlItemParameterViewModel yamlParameter = new YamlItemParameterViewModel();
                                    yamlParameter.Comment = description.ToComment();
                                    yamlParameter.CommentLineNumber = description.ToCommentLineNumber();
                                    yamlParameter.Type = desc.Type.ToId();
                                    yamlParameter.ParameterType = ParameterType.Input;
                                    yamlParameter.Name = desc.Name;
                                    itemParams.Add(yamlParameter);
                                }
                                memberItem.Parameters = itemParams;
                                break;
                            }
                        case SyntaxType.PropertySyntax:
                            {
                                var description = (PropertySyntaxDescription)csharpSyntax;
                                List<YamlItemParameterViewModel> itemParams = new List<YamlItemParameterViewModel>();
                                var desc = description.Property;
                                {
                                    YamlItemParameterViewModel yamlParameter = new YamlItemParameterViewModel();
                                    yamlParameter.Comment = description.ToComment();
                                    yamlParameter.CommentLineNumber = description.ToCommentLineNumber();
                                    yamlParameter.Type = desc.Type.ToId();
                                    yamlParameter.ParameterType = ParameterType.Input;
                                    yamlParameter.Name = desc.Name;
                                    itemParams.Add(yamlParameter);
                                }
                                memberItem.Parameters = itemParams;
                                break;
                            }
                    }
                    
                    item.Members.Add(memberItem.Identity);
                    yaml.Items.Add(memberItem.Identity, memberItem);
                }

                yaml.Items.Add(item.Identity, item);
            }

            return yaml;
        }

        public static TocViewModel ToTocViewModel(List<YamlViewModel> yamls)
        {
            throw new NotImplementedException();
        }
    }
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

    public class TocViewModel
    {
        public List<TocItemViewModel> Items { get; set; } 
    }

    public class TocItemViewModel
    {
        public string Identity { get; set; }

        /// <summary>
        /// The yaml file consists of current item
        /// </summary>
        public string YamlUrl { get; set; }
        public bool MemberLoaded { get; set; }
        public List<string> Members { get; set; }
    }

    public class YamlViewModel
    {
        /// <summary>
        /// The base url for current domain
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// The url path for current yaml file
        /// </summary>
        public string YamlUrl { get; set; }

        [YamlDotNet.Serialization.YamlMember(Alias = "layout")]
        public List<LayoutItem> DefaultLayoutItems { get; set; }

        public Dictionary<string, YamlItemViewModel> Items { get; set; }
    }

    public class YamlItemViewModel : CommentViewModelBase
    {
        public string Identity { get; set; }
        public MemberType MemberType { get; set; }
        public string Syntax { get; set; }
        public List<string> Hierarchy { get; set; }
        /// <summary>
        /// Relative path
        /// </summary>
        public string Path { get; set; }
        public AssemblyDetail Assembly { get; set; }
        public List<YamlItemParameterViewModel> Parameters { get; set; }
        public List<string> Members { get; set; }
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

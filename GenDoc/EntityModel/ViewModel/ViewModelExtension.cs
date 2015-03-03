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
        public static List<LayoutItem> DefaultLayoutItems = new List<LayoutItem>
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

        public static string ToComment(this IDescription des)
        {
            var raw = des.Comments[0].Raw;
            if (string.IsNullOrEmpty(raw))
            {
                return null;
            }

            XmlDocument doc = new XmlDocument();

            // Really Rough, need change
            try
            {
                doc.LoadXml(raw);
                return doc.InnerText;
            }
            catch
            {
                return raw;
            }
        }

        public static int ToCommentLineNumber(this IDescription des)
        {
            return des.Comments[0].StartLine;
        }

        public static string ToId(this Identity id)
        {
            return id.ToString();
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

        public static List<YamlItemParameterViewModel> ToParams(this SyntaxDescription csharpSyntax)
        {
            List<YamlItemParameterViewModel> itemParams = new List<YamlItemParameterViewModel>();
            switch (csharpSyntax.SyntaxType)
            {
                case SyntaxType.MethodSyntax:
                    {
                        var description = (MethodSyntaxDescription)csharpSyntax;
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
                        break;
                    }
                case SyntaxType.ConstructorSyntax:
                    {
                        var description = (ConstructorSyntaxDescription)csharpSyntax;
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
                        break;
                    }
                case SyntaxType.PropertySyntax:
                    {
                        var description = (PropertySyntaxDescription)csharpSyntax;
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
                        break;
                    }
                default:
                    itemParams = null;
                    break;
            }

            return itemParams;
        }

        public static YamlItemViewModel ToItem(this NamespaceMetadata member)
        {
            var memberItem = new YamlItemViewModel();
            string[] assemblyInfos = member.AssemblyName.Split(new char[] { ' ', ',' });
            var version = assemblyInfos[1].Replace("Version=", string.Empty);
            if (string.IsNullOrWhiteSpace(version))
            {
                version = "1.0.0.0";
            }
            memberItem.Assembly = new AssemblyDetail()
            {
                Name = assemblyInfos[0],
                Version = version,
            };
            memberItem.Items = new List<string>();

            memberItem.Identity = member.Identity.ToId();
            var csharpSyntax = member.SyntaxDescriptionGroup[SyntaxLanguage.CSharp];
            memberItem.Comment = csharpSyntax.ToComment();
            memberItem.CommentLineNumber = csharpSyntax.ToCommentLineNumber();
            memberItem.Syntax = csharpSyntax.Syntax;
            memberItem.MemberType = member.MemberType;

            memberItem.Parameters = csharpSyntax.ToParams();
            return memberItem;
        }

        public static YamlItemViewModel ToItem(this NamespaceMemberMetadata member)
        {
            var memberItem = new YamlItemViewModel();
            memberItem.Items = new List<string>();
            memberItem.Identity = member.Identity.ToId();
            var csharpSyntax = member.SyntaxDescriptionGroup[SyntaxLanguage.CSharp];
            memberItem.Comment = csharpSyntax.ToComment();
            memberItem.CommentLineNumber = csharpSyntax.ToCommentLineNumber();
            memberItem.Syntax = csharpSyntax.Syntax;
            memberItem.Hierarchy = member.InheritanceHierarchy == null ? null : member.InheritanceHierarchy.Select(s => s.ToString().Substring(2)).ToList();
            memberItem.MemberType = member.MemberType;

            memberItem.Parameters = csharpSyntax.ToParams();
            return memberItem;
        }

        public static YamlItemViewModel ToItem(this NamespaceMembersMemberMetadata member)
        {
            var memberItem = new YamlItemViewModel();
            memberItem.Identity = member.Identity.ToId();
            var csharpSyntax = member.SyntaxDescriptionGroup[SyntaxLanguage.CSharp];
            memberItem.Comment = csharpSyntax.ToComment();
            memberItem.CommentLineNumber = csharpSyntax.ToCommentLineNumber();
            memberItem.Syntax = csharpSyntax.Syntax;
            memberItem.MemberType = member.MemberType;

            memberItem.Parameters = csharpSyntax.ToParams();
            return memberItem;
        }

        public static YamlViewModel ToYamlViewModel(this ProjectMetadata projectMetadata)
        {
            YamlViewModel yamlViewModel = new YamlViewModel();
            MarkdownYamlViewModel markdown = new MarkdownYamlViewModel();
            // 1. Merge project metadata list
            IndexYamlViewModel yaml = new IndexYamlViewModel();
            yaml.BaseUrl = "https://localhost/";
            yaml.RepositoryUrl = "https://github.com/vicancy/vicancy.github.io.git";
            yaml.Items = new Dictionary<string, YamlItemViewModel>();
            yaml.YamlUrl = projectMetadata.ProjectName + ".yaml";
            yaml.Layout = DefaultLayoutItems;

            foreach (var ns in projectMetadata.Namespaces)
            {
                YamlItemViewModel item = ns.Value.ToItem();
                foreach (var member in ns.Value.Members)
                {
                    YamlItemViewModel memberItem = member.ToItem();
                    memberItem.Assembly = item.Assembly;

                    foreach (var membersMember in member.Members)
                    {
                        YamlItemViewModel membersMemberItem = membersMember.ToItem();
                        membersMemberItem.Assembly = item.Assembly;
                        memberItem.Items.Add(membersMemberItem.Identity);
                        yaml.Items.Add(membersMemberItem.Identity, membersMemberItem);
                    }

                    item.Items.Add(memberItem.Identity);
                    yaml.Items.Add(memberItem.Identity, memberItem);
                }

                yaml.Items.Add(item.Identity, item);
            }

            yaml.Items = yaml.Items.OrderBy(s => s.Value.MemberType).ToDictionary(s => s.Key, s => s.Value);

            TocYamlViewModel toc = new TocYamlViewModel();
            toc.AddRange(yaml.Items.Select(s =>
            new TocYamlItemViewModel
            {
                DisplayName = s.Value.DisplayName,
                Identity = s.Value.Identity,
                Url = s.Value.Url,
            }));
            yamlViewModel.TocYamlViewModel = toc;
            yamlViewModel.IndexYamlViewModel = yaml;
            yamlViewModel.MarkdownYamlViewModel = markdown;
            return yamlViewModel;
        }

        public static IndexYamlViewModel ToTocViewModel(List<IndexYamlViewModel> yamls)
        {
            throw new NotImplementedException();
        }
    }
}

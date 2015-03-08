using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace EntityModel.ViewModel
{
    public static class TripleSlashCommentParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="trim"></param>
        /// <returns></returns>
        public static string GetSummary(string xml, bool trim)
        {
            // Resolve <see cref> to @ syntax
            // Also support <seealso cref>
            string selector = "/member/summary";
            xml = ResolveSeeCref(xml, selector);
            xml = ResolveSeeAlsoCref(xml, selector);

            return GetSingleNode(xml, selector, trim, (e) => null);
        }

        public static string GetReturns(string xml, bool trim)
        {
            // Resolve <see cref> to @ syntax
            // Also support <seealso cref>
            string selector = "/member/returns";
            xml = ResolveSeeCref(xml, selector);
            xml = ResolveSeeAlsoCref(xml, selector);

            return GetSingleNode(xml, selector, trim, (e) => null);
        }

        public static string GetParam(string xml, string param, bool trim)
        {
            Debug.Assert(!string.IsNullOrEmpty(param));
            if (string.IsNullOrEmpty(param))
            {
                return null;
            }

            // Resolve <see cref> to @ syntax
            // Also support <seealso cref>
            string selector = "/member/param[@name='" + param + "']";
            xml = ResolveSeeCref(xml, selector);
            xml = ResolveSeeAlsoCref(xml, selector);

            return GetSingleNode(xml, selector, trim, (e) => null);
        }

        /// <summary>
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="nodeSelector"></param>
        /// <returns></returns>
        public static string ResolveSeeAlsoCref(string xml, string nodeSelector)
        {
            // Resolve <see cref> to @ syntax
            return ResolveCrefLink(xml, nodeSelector + "/seealso");
        }

        public static string ResolveSeeCref(string xml, string nodeSelector)
        {
            // Resolve <see cref> to @ syntax
            return ResolveCrefLink(xml, nodeSelector + "/see");
        }

        public static string ResolveCrefLink(string xml, string nodeSelector)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                var nav = doc.CreateNavigator();
                var iter = nav.Select(nodeSelector + "[@cref]");
                List<XPathNavigator> sees = new List<XPathNavigator>();
                foreach (XPathNavigator i in iter)
                {
                    var node = i.SelectSingleNode("@cref");
                    if (node != null)
                    {
                        var currentNode = i.Clone();
                        var value = node.Value;

                        // Current: Always append a space, remove when resolve

                        // TODO: need more discussion on @ syntax
                        // what if <see cref="book">s intentionally want no space in between
                        // Append a space to the link 
                        //i.MoveToNext();
                        //if (i.NodeType == XPathNodeType.Text)
                        //{
                        //    if (!string.IsNullOrWhiteSpace(i.Value.Substring(0, 1)))
                        //    {
                        //        i.ReplaceSelf(" " + i.Value);
                        //    }
                        //}

                        currentNode.InsertAfter("@" + value + " ");

                        sees.Add(currentNode);
                    }
                }

                // on successful deleteself, i would point to its parent
                foreach (XPathNavigator i in sees)
                {
                    i.DeleteSelf();
                }

                xml = doc.InnerXml;
            }
            catch
            {
            }

            return xml;
        }

        public static string GetSingleNode(string xml, string selector, bool trim, Func<Exception, string> errorHandler)
        {
            try
            {
                using (StringReader reader = new StringReader(xml))
                {
                    XPathDocument doc = new XPathDocument(reader);
                    var nav = doc.CreateNavigator();
                    var output = nav.SelectSingleNode(selector).Value;
                    if (trim) output = output.Trim();
                    return output;
                }
            }
            catch (Exception e)
            {
                if (errorHandler != null)
                {
                    return errorHandler(e);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public static class YamlViewModelExtension
    {
        public static bool IsPageLevel(this MemberType type)
        {
            return type == MemberType.Namespace || type == MemberType.Class || type == MemberType.Enum || type == MemberType.Delegate || type == MemberType.Interface || type == MemberType.Struct;
        }

        public static YamlItemViewModel Shrink(this YamlItemViewModel item)
        {
            YamlItemViewModel shrinkedItem = new YamlItemViewModel();
            shrinkedItem.Name = item.Name;
            shrinkedItem.Href = item.Href;
            shrinkedItem.Summary = item.Summary;
            shrinkedItem.Type = item.Type;
            return shrinkedItem;
        }

        public static YamlItemViewModel ShrinkSelfAndChildren(this YamlItemViewModel item)
        {
            if (item.Items == null)
            {
                return item;
            }

            YamlItemViewModel shrinkedItem = item.Shrink();
            shrinkedItem.Items = new List<YamlItemViewModel>();
            foreach (var i in item.Items)
            {
                shrinkedItem.Items.Add(i.Shrink());
            }

            return shrinkedItem;
        }

        public static YamlItemViewModel ShrinkChildren(this YamlItemViewModel item)
        {
            if (item.Items == null)
            {
                return item;
            }

            YamlItemViewModel shrinkedItem = (YamlItemViewModel)item.Clone();
            shrinkedItem.Items = new List<YamlItemViewModel>();
            foreach(var i in item.Items)
            {
                shrinkedItem.Items.Add(i.Shrink());
            }

            return shrinkedItem;
        }
    }

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

        public static YamlItemParameterViewModel GetReturn(this SyntaxDescription csharpSyntax)
        {
            switch (csharpSyntax.SyntaxType)
            {
                case SyntaxType.MethodSyntax:
                    {
                        var description = (MethodSyntaxDescription)csharpSyntax;
                        YamlItemParameterViewModel yamlReturn = new YamlItemParameterViewModel();
                        var returnPara = description.Return;
                        yamlReturn.Description = returnPara.ToComment();
                        yamlReturn.Name = returnPara.Name;
                        yamlReturn.Type =
                            new SourceDetail
                            {
                                Name = returnPara.Type.ToId(),
                            };
                        return yamlReturn;
                    }
            }

            return null;
        }

        /// <summary>
        /// TODO: Resolve Link by passing in index view model
        /// </summary>
        /// <param name="csharpSyntax"></param>
        /// <returns></returns>
        public static List<YamlItemParameterViewModel> ToParams(this SyntaxDescription csharpSyntax, Dictionary<string, IndexYamlItemViewModel> apis)
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
                            yamlParameter.Description = description.ToComment();
                            yamlParameter.Type =
                            new SourceDetail
                            {
                                Name = desc.Type.ToId(),
                            };

                            yamlParameter.Name = desc.Name;
                            itemParams.Add(yamlParameter);
                        }
                        break;
                    }
                case SyntaxType.ConstructorSyntax:
                    {
                        var description = (ConstructorSyntaxDescription)csharpSyntax;
                        foreach (var desc in description.Parameters)
                        {
                            YamlItemParameterViewModel yamlParameter = new YamlItemParameterViewModel();
                            yamlParameter.Description = description.ToComment();
                            yamlParameter.Type =
                            new SourceDetail
                            {
                                Name = desc.Type.ToId(),
                            };
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
                            yamlParameter.Description = description.ToComment();
                            yamlParameter.Type =
                            new SourceDetail
                            {
                                Name = desc.Type.ToId(),
                            };
                            yamlParameter.Name = desc.Name;
                            itemParams.Add(yamlParameter);
                        }
                        break;
                    }
                default:
                    itemParams = null;
                    break;
            }

            // Resolve link
            if (itemParams != null)
            {
                foreach (var param in itemParams)
                {
                    IndexYamlItemViewModel item;
                    if (apis.TryGetValue(param.Type.Name, out item))
                    {
                        param.Type.Href = apis[param.Type.Name].Href;
                    }
                    else
                    {
                        // TODO: make it accurate
                        param.Type.Href = string.Format("https://msdn.microsoft.com/en-us/library/{0}(v=vs.110).aspx", param.Type.Name);
                    }
                }
            }

            return itemParams;
        }

        public static YamlItemViewModel ToItem(this IMetadata member, Dictionary<string, IndexYamlItemViewModel> apis)
        {
            var memberItem = new YamlItemViewModel();
            string[] assemblyInfos = member.AssemblyName.Split(new char[] { ' ', ',' });
            var version = assemblyInfos[1].Replace("Version=", string.Empty);
            if (string.IsNullOrWhiteSpace(version))
            {
                version = "1.0.0.0";
            }

            memberItem.Name = member.Identity.ToId();
            memberItem.Type = member.MemberType;
            memberItem.Source = new SourceDetail
            {
                Path = member.FilePath,
            };

            memberItem.Syntax = new SyntaxDetail
            {
                Content = new Dictionary<SyntaxLanguage, string>()
            };

            foreach(var syntaxDescription in member.SyntaxDescriptionGroup)
            {
                memberItem.Syntax.Content.Add(syntaxDescription.Key, syntaxDescription.Value.ToComment());
                memberItem.Syntax.Parameters = syntaxDescription.Value.ToParams(apis);
                memberItem.Syntax.Return = syntaxDescription.Value.GetReturn();
            }

            return memberItem;
        }

        public static YamlViewModel ToYamlViewModel(this DocumentationMetadata docMetadata, string folderName)
        {
            YamlViewModel yamlViewModel = new YamlViewModel();
            yamlViewModel.MemberYamlViewModelList = new List<YamlItemViewModel>();
            yamlViewModel.IndexYamlViewModel = docMetadata.AllMembers.Select(s => new IndexYamlItemViewModel
            {
                Name = s.Key.ToId(),
                Href = folderName + "/" + s.Key.ToId(),
                YamlPath = folderName + "/" + s.Key.ToId() + ".yaml",
            }).ToDictionary(s => s.Name, s => s);
            var apis = yamlViewModel.IndexYamlViewModel;
            var toc = new YamlItemViewModel();
            yamlViewModel.TocYamlViewModel = toc;
            toc.Type = MemberType.Toc;
            toc.Items = new List<YamlItemViewModel>();
            toc.Layout = DefaultLayoutItems;

            // Toc only need namespace
            foreach (var ns in docMetadata.Namespaces)
            {
                YamlItemViewModel item = ns.Value.ToItem(apis);
                toc.Items.Add(item);
            }

            // For namespaces and namespace's members, only need one layer 
            foreach (var ns in docMetadata.Namespaces)
            {
                YamlItemViewModel item = ns.Value.ToItem(apis);
                item.Items = new List<YamlItemViewModel>();
                yamlViewModel.MemberYamlViewModelList.Add(item);

                foreach (var member in ns.Value.Members)
                {
                    YamlItemViewModel memberItem = member.ToItem(apis);
                    item.Items.Add(memberItem);
                    YamlItemViewModel memberItemDetail = member.ToItem(apis);
                    memberItemDetail.Items = new List<YamlItemViewModel>();
                    foreach(var membersMember in member.Members)
                    {
                        YamlItemViewModel membersMemberItem = membersMember.ToItem(apis);
                        memberItemDetail.Items.Add(membersMemberItem);
                    }

                    yamlViewModel.MemberYamlViewModelList.Add(memberItemDetail);
                }
            }

            // set href TODO: a better way???
            foreach (var item in yamlViewModel.MemberYamlViewModelList)
            {
                item.Href = yamlViewModel.IndexYamlViewModel[item.Name].Href;
                item.YamlPath = yamlViewModel.IndexYamlViewModel[item.Name].YamlPath;
            }

            return yamlViewModel;
        }
    }
}

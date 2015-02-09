using DocAsCode.EntityModel;
using DocAsCode.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LibGit2Sharp;
using System.Net;

namespace DocAsCode.MergeDoc
{
    public class ViewModel
    {
        public string baseURL;
        public string mataPath;
        private string sourceGitUrl;

        public AssemblyDocMetadata assemblyMta;
        public NamespaceDocMetadata namespaceMta;
        public ClassDocMetadata classMta;
        public InterfaceDocMetadata interfaceMta;
        public StructDocMetadata structMta;
        public Dictionary<string, string> idPathRelativeMapping;
        public Dictionary<string, string> idDisplayNameRelativeMapping;
        private MarkDownConvertor _mdConvertor;
        private MarkdownCollectionCache _markdownCollectionCache;

        /// <summary>
        /// Resolve name from mta by the Id and syntax
        /// e.g. methodA``1 to methodA<T>
        /// </summary>
        /// <param name="mta"></param>
        /// <returns></returns>
        public string ResolveName(MemberDocMetadata mta)
        {
            string id = mta.Id.ToString();
            string name = id.Substring(id.IndexOf(":") + 1).Trim();

            string parentName;
            string parentId = mta.Parent.ToString();

            if (idDisplayNameRelativeMapping.TryGetValue(parentId, out parentName))
            {
               name = name.Replace(parentId.Substring(parentId.IndexOf(":") + 1).Trim(), parentName);
            }

            if (name.IndexOf(".#ctor") != -1)
            {
                name = name.Replace(".#ctor", "");
                if (!name.EndsWith(")"))
                {
                    name += "()";
                }
            }

            if (name.IndexOf("``1") != -1 || name.IndexOf("`1") != -1)
            {
                Regex typeRegex = new Regex(@"[\s\S]*\<(?<type>\w+)\>[\s\S]*\bwhere\b\s*\1\s*:\s*(?<baseType>[\S]*)");
                string baseType = typeRegex.Match(mta.Syntax.Content).Groups["baseType"].Value;
                string type = typeRegex.Match(mta.Syntax.Content).Groups["type"].Value;

                string templateType = baseType.Equals("") ? type : baseType;
                return name.Replace("``1", string.Format("<{0}>", templateType)).Replace("`1", string.Format("<{0}>", templateType));
            }

            return name;
        }

        /// <summary>
        /// Given an ID, return the link in html: <a href="path">displayName</a>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ResolveLink(string id)
        {
            Regex typeRegex = new Regex((@"^(?<class>[\s\S]*?)(\{(?<type>[\s\S]*?)\})?$"));

            string type = typeRegex.Match(id).Groups["type"].Value;
            string classId = typeRegex.Match(id).Groups["class"].Value;


            string right = "";
            if (!type.Equals(""))
            {
                if (!type.Equals("``0"))
                {
                    right = ResolveLink("T:" + type);
                }
                classId = classId + "`1";
            }
            string path;
            string name = classId.Substring(classId.IndexOf(":") + 1).Trim().Replace("``1", "<T>").Replace("`1", "<T>");
            if (idPathRelativeMapping.ContainsKey(classId))
            {
                path = idPathRelativeMapping[classId];
            }
            else
            {
                path = "https://msdn.microsoft.com/en-us/library/" + classId.Substring(classId.IndexOf(":") + 1).Trim();
            }
            if (right.Equals(""))
            {
                return string.Format("<a href=\"{0}\">{1}</a>", path, name);
            }
            else
            {
                return string.Format("<a href=\"{0}\">{1}</a><a>&nbsp;&lt;</a>{2}<a>&gt;</a>", path, name, right);
            }
        }

        /// <summary>
        /// To resolve triple slash component with <see cref=""/>, turn the crefed class to link
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public string ResolveSeeCref(string description)
        {
            var refs = TripleSlashParser.SeeCrefRegex.Matches(description);

            for (int i = 0; i < refs.Count; i++)
            {
                var @ref = refs[i];
                string link = ResolveLink(@ref.Groups["ref"].Value);
                description = description.Replace(@ref.Groups["seeCrefMatch"].Value, "@" + link);
            }
            return description;
        }

        private string GetSourceUrlWithoutExtension(string path, out Branch branch, out Repository repo)
        {
            try
            {
                string gitPath = Repository.Discover(path);
                repo = new Repository(Path.GetDirectoryName(gitPath));
                branch = repo.Head;
                Remote remote = repo.Network.Remotes["origin"];
                string url = remote.Url;
                if (url.EndsWith(".git"))
                {
                    url = url.Substring(0, url.LastIndexOf(".git"));
                }
                return url;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failing in finding source git url without extension from path {0}", path, e);
                branch = null;
                repo = null;
                return "";
            }
        }

        public string GetClassSourceUrl(CompositeDocMetadata classMta)
        {
            try
            {
                Branch branch;
                Repository repo;
                string url = GetSourceUrlWithoutExtension(classMta.FilePath, out branch, out repo);
                sourceGitUrl = Path.Combine(url, "blob", branch.Name, classMta.FilePath.Replace(repo.Info.WorkingDirectory, "")).Replace("\\", "/");
               /* if (!RemoteUrlExists(sourceGitUrl))
                {
                    sourceGitUrl = "";
                    Console.Error.WriteLine("Cannot find source git url of class {0}", classMta.Id);
                }*/
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failing in finding source git url of class {0}", classMta.Id, e);
                sourceGitUrl = "";
            }
            return sourceGitUrl;
        }

        public string GetMemberSourceUrl(MemberDocMetadata methodMta)
        {
            if (!sourceGitUrl.Equals(""))
            {
                return sourceGitUrl + "#L" + (methodMta.Syntax.StartLine + 1);
            }
            else
            {
                return sourceGitUrl;
            }
        }

        public string GetMarkdownSourceUrl(DocMetadata classMta)
        {
            string markdownGitURL = "";

            string markdownFilePath;
            try
            {
                if (_markdownCollectionCache.IdMarkdownFileMap.TryGetValue(classMta.Id, out markdownFilePath))
                {
                    Branch branch;
                    Repository repo;
                    string url = GetSourceUrlWithoutExtension(markdownFilePath, out branch, out repo);
                    string relativePath = markdownFilePath.Replace(repo.Info.WorkingDirectory, "");
                    markdownGitURL = Path.Combine(url, "edit", branch.Name, relativePath).Replace("\\", "/");
                    if (!RemoteUrlExists(markdownGitURL))
                    {
                        markdownGitURL = Path.Combine(url, "new", branch.Name, Path.GetDirectoryName(relativePath), "?filename=" + Path.GetFileName(relativePath)).Replace("\\", "/");
                        markdownGitURL += String.Format(string.Format("&value=---%0Dclass: {0}%0D---%0D", classMta.Id));
                    }
                }
                else
                {
                    Branch branch;
                    Repository repo;
                    string url = GetSourceUrlWithoutExtension(mataPath, out branch, out repo);
                    markdownGitURL = Path.Combine(url, "new", branch.Name, "?filename=" + FileExtensions.ToValidFilePath(classMta.Id) + ".md").Replace("\\", "/");
                    markdownGitURL += String.Format(string.Format("&value=---%0Dclass: {0}%0D---%0D", classMta.Id));
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failing in finding source git url of class {0}", classMta.Id, e);
            }

            return markdownGitURL;
        }

        private bool RemoteUrlExists(string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        public string DisplaySummary(string xmlDocumentation)
        {
            string summary = TripleSlashParser.Parse(xmlDocumentation)["summary"].Trim();

            return ResolveSeeCref(summary);
        }

        private string ExtractMarkdownContent(DocMetadata mta)
        {
            string content;
            if (_markdownCollectionCache.TryGetValue(mta.Id, out content))
            {
                content = _mdConvertor.ConvertToHTML(content);
            }
            return content;
        }

        private SortedDictionary<string, string> ExtractParameters(DocMetadata mta, SortedDictionary<string, string> parameters)
        {
            parameters = TripleSlashParser.ParseParam(mta.XmlDocumentation, parameters);

            for (int i = 0; i < parameters.Count; i++)
            {
                string param = parameters.ElementAt(i).Key;
                string description =parameters.ElementAt(i).Value;
                parameters[param] = ResolveSeeCref(description.Trim());
            }
            return parameters;
        }

        private Dictionary<string, string> ExtractReturn(MethodDocMetadata m)
        {
            var returns = m.MethodSyntax.Returns;
            string description = (TripleSlashParser.Parse(m.XmlDocumentation))["returns"].Trim();
            string returnType = returns.Keys.FirstOrDefault();
            returns[returnType] = _mdConvertor.ConvertToHTML(description);
            return returns;
        }

        private string ExtractSyntaxContent(MemberDocMetadata mta)
        {
            return _mdConvertor.ConvertToHTML(string.Format(@"
```
{0}
```
", mta.Syntax.Content));
        }

        public void ResolveClassTypeSymbolContent(CompositeDocMetadata mta)
        {
            if (mta == null) return;
            foreach (var member in mta.MemberDict)
            {
                switch (member.Key)
                {
                    case MemberType.Method:
                        {
                            foreach (var m in member.Value.Values)
                            {
                                var method = m as MethodDocMetadata;
                                method.MethodSyntax.Parameters = ExtractParameters(method, method.MethodSyntax.Parameters);
                                method.MethodSyntax.Returns = ExtractReturn(method);
                                method.MarkdownContent = ExtractMarkdownContent(method);
                                method.Syntax.Content = ExtractSyntaxContent(method);
                            }
                            break;
                        };
                    case MemberType.Constructor:
                        {
                            foreach (var c in member.Value.Values)
                            {
                                var constructor = c as ConstructorDocMetadata;
                                constructor.ConstructorSyntax.Parameters = ExtractParameters(constructor, constructor.ConstructorSyntax.Parameters);
                                constructor.MarkdownContent = ExtractMarkdownContent(constructor);
                                constructor.Syntax.Content = ExtractSyntaxContent(constructor);

                            }
                            break;
                        };
                    case MemberType.Property:
                        {
                            foreach (var p in member.Value.Values)
                            {
                                var property = p as PropertyDocMetadata;
                                property.MarkdownContent = ExtractMarkdownContent(property);
                                property.Syntax.Content = ExtractSyntaxContent(property);
                            }
                            break;
                        }
                }
            }
        }

        public void ResolveContent()
        {
            ExtractMarkdownContent(namespaceMta);
            ExtractMarkdownContent(classMta);

            if(classMta != null)
            {
                ResolveClassTypeSymbolContent(classMta);
            }
            if(interfaceMta != null)
            {
                ResolveClassTypeSymbolContent(interfaceMta);
            }
            if(structMta != null)
            {
                ResolveClassTypeSymbolContent(structMta);
            }
        }

        private void AddToMap(MemberDocMetadata mta, bool isHtml, bool isChild)
        {
            string path;
            if (!idPathRelativeMapping.TryGetValue(mta.Id, out path))
            {
                string filePath;

                if (isChild && isHtml)
                {
                    filePath = Path.Combine(mta.Parent.ToString().ToValidFilePath(), mta.Id.ToString().ToValidFilePath() + ".html");
                }
                else
                {
                    if (isHtml)
                    {
                        filePath = mta.Id.ToString().ToValidFilePath() + ".html";
                    }
                    else
                    {
                        filePath = idDisplayNameRelativeMapping[mta.Parent.ToString()];
                    }
                }
                idPathRelativeMapping.Add(mta.Id, filePath);
            }

            string name;
            if (!idDisplayNameRelativeMapping.TryGetValue(mta.Id, out name))
            {
                string mtaName = ResolveName(mta);
                idDisplayNameRelativeMapping.Add(mta.Id, mtaName);
            }
        }

        private void init()
        {
            idPathRelativeMapping = new Dictionary<string, string>();
            idDisplayNameRelativeMapping = new Dictionary<string, string>();

            if (assemblyMta.Namespaces != null)
            {
                foreach (var ns in assemblyMta.Namespaces)
                {
                    AddToMap(ns, true, false);

                    if (ns.Members != null)
                    {
                        foreach (var c in ns.Members)
                        {
                            AddToMap(c, true, true);

                            var type = c as CompositeDocMetadata;
                            if (type != null && type.Members != null)
                            {
                                foreach (var m in type.Members)
                                {
                                    AddToMap(m, false, true);
                                }
                            }
                        }
                    }
                }
            }

            this._mdConvertor = new MarkDownConvertor(idPathRelativeMapping);
        }

        public ViewModel(AssemblyDocMetadata assemblyMta, string mataFilePath,MarkdownCollectionCache markdownCollectionCache)
        {
            this.assemblyMta = assemblyMta;
            this.mataPath = mataFilePath;
            this._markdownCollectionCache = markdownCollectionCache;
            init();
        }


    }

    /// <summary>
    /// Resolve the triple slashes
    /// </summary>
    public class TripleSlashParser
    {
        static private string[] tripleSlashTypes = {    "summary",
                                                        "param",
                                                        "returns",
                                                        "example",
                                                        "code",
                                                        "see",
                                                        "seealso",
                                                        "list",
                                                        "value",
                                                        "author",
                                                        "file",
                                                        "copyright"  };
        static public Regex SeeCrefRegex = new Regex(@"(?<seeCrefMatch><see cref=""(?<ref>[\s\S]*?)""/>?)", RegexOptions.Compiled);

        static public Dictionary<string, string> Parse(string tripleSlashStr)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            // Replace <see cref=""/>

            foreach (string type in tripleSlashTypes)
            {
                string typeRegexPatten = string.Format(@"<{0}>(?<typeContent>[\s\S]*?)</{0}>", type);
                Regex typeRegex = new Regex(typeRegexPatten, RegexOptions.Multiline);
                result.Add(type, typeRegex.Match(tripleSlashStr).Groups["typeContent"].Value);
            }
            return result;
        }

        static public SortedDictionary<string, string> ParseParam(string tripleSlashStr, SortedDictionary<string, string> parameters)
        {
            SortedDictionary<string, string> result = new SortedDictionary<string, string>();

            for (int i = 0; i < parameters.Count; i++)
            {
                string param = parameters.ElementAt(i).Key;
                int index = param.LastIndexOf(":");

                if (index != -1 && index + 1 < param.Length)
                {
                    string paramName = param.Substring(index + 1).Trim();
                    string typeRegexPatten = string.Format("<param name=\"{0}\">(?<typeContent>[\\s\\S]*?)</param>", paramName);
                    Regex typeRegex = new Regex(typeRegexPatten, RegexOptions.Multiline);

                    result.Add(param, typeRegex.Match(tripleSlashStr).Groups["typeContent"].Value);
                }
            }

            return result;
        }

    }
}


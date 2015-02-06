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
        public MethodDocMetadata methodMta;
        public Dictionary<string, string> idPathRelativeMapping;
        public Dictionary<string, string> idDisplayNameRelativeMapping;
        private MarkDownConvertor _mdConvertor;
        private MarkdownCollectionCache _markdownCollectionCache;

        public string ResolveName(MemberDocMetadata mta)
        {
            string id = mta.Id.ToString();

            string parentId = mta.Parent.ToString();

            string name = id.Substring(id.IndexOf(":") + 1).Trim().Replace(parentId.Substring(parentId.IndexOf(":") + 1).Trim(), idDisplayNameRelativeMapping[parentId]);

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

        public string DisplaySummary(string xmlDocumentation)
        {
            string summary = TripleSlashParser.Parse(xmlDocumentation)["summary"].Trim();

            var refs = TripleSlashParser.SeeCrefRegex.Matches(summary);

            for(int i = 0; i < refs.Count; i++)
            {
                var @ref = refs[i];
                string link = ResolveLink(@ref.Groups["ref"].Value);
                summary = summary.Replace(@ref.Groups["seeCrefMatch"].Value, "@" + link);
            }
            return summary;
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

        public string GetClassSourceUrl(ClassDocMetadata classMta)
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

        public string GetMethodSourceUrl(MethodDocMetadata methodMta)
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

        public string GetMarkdownSourceUrl(ClassDocMetadata classMta)
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

        public void resolveContent()
        {
            string content;
            if (_markdownCollectionCache.TryGetValue(namespaceMta.Id, out content))
            {
                namespaceMta.MarkdownContent = _mdConvertor.ConvertToHTML(content);
            }

            if (_markdownCollectionCache.TryGetValue(classMta.Id, out content))
            {
                classMta.MarkdownContent = _mdConvertor.ConvertToHTML(content);
            }

            if (classMta.Methods != null)
            {
                foreach (var m in classMta.Methods)
                {
                    m.MethodSyntax.Parameters = TripleSlashParser.ParseParam(m.XmlDocumentation, m.MethodSyntax.Parameters);

                    for (int i = 0; i < m.MethodSyntax.Parameters.Count; i++)
                    {
                        string param = m.MethodSyntax.Parameters.ElementAt(i).Key;
                        string description = m.MethodSyntax.Parameters.ElementAt(i).Value;
                        m.MethodSyntax.Parameters[param] = _mdConvertor.ConvertToHTML(description.Trim());
                    }

                    string returnType = m.MethodSyntax.Returns.Keys.FirstOrDefault();
                    m.MethodSyntax.Returns[returnType] = _mdConvertor.ConvertToHTML((TripleSlashParser.Parse(m.XmlDocumentation))["returns"].Trim());

                    if (_markdownCollectionCache.TryGetValue(m.Id, out content))
                    {
                        m.MarkdownContent = _mdConvertor.ConvertToHTML(content);

                    }

                    m.Syntax.Content = _mdConvertor.ConvertToHTML(string.Format(@"
```
{0}
```
", m.Syntax.Content));
                }
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
                    string nsFile = ns.Id.ToString().ToValidFilePath() + ".html";
                    idPathRelativeMapping.Add(ns.Id, nsFile);

                    string nsId = ns.Id.ToString();
                    idDisplayNameRelativeMapping.Add(ns.Id, nsId.Substring(nsId.IndexOf(":") + 1).Trim());

                    if (ns.Classes != null)
                    {
                        foreach (var c in ns.Classes)
                        {
                            string classPath = Path.Combine(ns.Id.ToString().ToValidFilePath(), c.Id.ToString().ToValidFilePath() + ".html");
                            idPathRelativeMapping.Add(c.Id, classPath);

                            string className = ResolveName(c);
                            idDisplayNameRelativeMapping.Add(c.Id, className);

                            if (c.Methods != null)
                            {
                                foreach (var m in c.Methods)
                                {
                                    idPathRelativeMapping.Add(m.Id, classPath);

                                    string methodName = ResolveName(m);
                                    idDisplayNameRelativeMapping.Add(m.Id, methodName);
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


using DocAsCode.EntityModel;
using DocAsCode.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DocAsCode.MergeDoc
{
    public class ViewModel
    {
        public string baseURL;

        public AssemblyDocMetadata assemblyMta;
        public NamespaceDocMetadata namespaceMta;
        public ClassDocMetadata classMta;
        public MethodDocMetadata methodMta;
        public Dictionary<string, string> idPathRelativeMapping;
        public Dictionary<string, string> idDisplayNameRelativeMapping;


        public string resolveName(MemberDocMetadata mta)
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

        public string resolveLink(string id)
        {
            Regex typeRegex = new Regex((@"^(?<class>[\s\S]*?)(\{(?<type>[\s\S]*?)\})?$"));

            string type = typeRegex.Match(id).Groups["type"].Value;
            string classId = typeRegex.Match(id).Groups["class"].Value;


            string right = "";
            if (!type.Equals(""))
            {
                if (!type.Equals("``0"))
                {
                    right = resolveLink("T:" + type);
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
                return string.Format("<a href=\"{0}\">{1}</a><a>&lt;</a>{2}<a>&gt;</a>", path, name, right);
            }
        }

        public void resolveContent(MarkdownCollectionCache markdownCollectionCache)
        {
            MarkDownConvertor mdConvertor = new MarkDownConvertor();
            mdConvertor.init(idPathRelativeMapping);

            string content;
            if (markdownCollectionCache.TryGetValue(namespaceMta.Id, out content))
            {
                namespaceMta.MarkdownContent = mdConvertor.ConvertToHTML(content);
            }
            //This may not be a good solution, just display the summary of triple slashes
            namespaceMta.XmlDocumentation = TripleSlashParser.Parse(namespaceMta.XmlDocumentation)["summary"].Trim();
            namespaceMta.XmlDocumentation = mdConvertor.ConvertToHTML(namespaceMta.XmlDocumentation);

            if (markdownCollectionCache.TryGetValue(classMta.Id, out content))
            {
                classMta.MarkdownContent = mdConvertor.ConvertToHTML(content);
            }
            classMta.XmlDocumentation = TripleSlashParser.Parse(classMta.XmlDocumentation)["summary"].Trim();
            classMta.XmlDocumentation = mdConvertor.ConvertToHTML(classMta.XmlDocumentation);
            resolveName(classMta);

            if (classMta.Methods != null)
            {
                foreach (var m in classMta.Methods)
                {
                    resolveName(m);
                    m.MethodSyntax.Parameters = TripleSlashParser.ParseParam(m.XmlDocumentation, m.MethodSyntax.Parameters);

                    for (int i = 0; i < m.MethodSyntax.Parameters.Count; i++)
                    {
                        string param = m.MethodSyntax.Parameters.ElementAt(i).Key;
                        string description = m.MethodSyntax.Parameters.ElementAt(i).Value;
                        m.MethodSyntax.Parameters[param] = mdConvertor.ConvertToHTML(description.Trim());
                    }

                    var parseDic = TripleSlashParser.Parse(m.XmlDocumentation);
                    string returnType = m.MethodSyntax.Returns.Keys.FirstOrDefault();
                    m.MethodSyntax.Returns[returnType] = mdConvertor.ConvertToHTML(parseDic["returns"].Trim());

                    m.XmlDocumentation = parseDic["summary"].Trim();
                    m.XmlDocumentation = mdConvertor.ConvertToHTML(m.XmlDocumentation);

                    if (markdownCollectionCache.TryGetValue(m.Id, out content))
                    {
                        m.MarkdownContent = mdConvertor.ConvertToHTML(content);
                    }

                    m.Syntax.Content = mdConvertor.ConvertToHTML(string.Format(@"
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

                            string className = resolveName(c);
                            idDisplayNameRelativeMapping.Add(c.Id, className);

                            if (c.Methods != null)
                            {
                                foreach (var m in c.Methods)
                                {
                                    idPathRelativeMapping.Add(m.Id, classPath);

                                    string methodName = resolveName(m);
                                    idDisplayNameRelativeMapping.Add(m.Id, methodName);
                                }
                            }
                        }
                    }
                }
            }
        }

        public ViewModel(AssemblyDocMetadata assemblyMta)
        {
            this.assemblyMta = assemblyMta;
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
        static private Regex SeeCrefRegex = new Regex(@"<see cref=""(?<ref>[\s\S]*?)""/>", RegexOptions.Compiled);

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


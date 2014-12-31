using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocAsCode.EntityModel;
using DocAsCode.Utility;
using System.IO;
using RazorEngine;
using System.Text.RegularExpressions;
using System.Diagnostics;
using MarkdownSharp;

namespace DocAsCode.MergeDoc
{
    class Program
    {
        private static DelimitedStringArrayConverter _delimitedArrayConverter = new DelimitedStringArrayConverter();
        static void Main(string[] args)
        {
            string mtaFile = @"TestData2\DocProject.docmta";
            string delimitedMdFiles = @"TestData\T_GenDocMetadata.AssemblyDocMetadata.md";
            string outputDirectory = "output";
            string templateDirectory = "Templates";
            string cssDirecotry = "css";
            string scriptDirecotry = "script";
            //Create a ViewModel for Razor to render the template
            ViewModel viewModel = new ViewModel();
            //This Dictionary stores the output file path for every namespace,class and method
            Dictionary<string, string> idPathRelativeMapping = new Dictionary<string, string>();
            MarkDownConvertor mdConvertor = new MarkDownConvertor();
            mdConvertor.init(idPathRelativeMapping);

            var options = new Option[]
                {
                    new Option(null, s => mtaFile = s, helpName: "metadataFile", defaultValue: mtaFile, helpText: @"The doc metadata file, only file with .docmta is recognized"),
                    new Option("outputDirectory", s => outputDirectory = s, defaultValue: outputDirectory, helpText: "The output html files will be generated into this folder"),
                    new Option("templateDirectory", s => templateDirectory = s, defaultValue: templateDirectory, helpText: "The template folder for generated html, the folder should contain class.html and namespace.html, the former one is the html template for Class pages, and the latter one is the html template for Namespace pages"),
                    new Option("delimitedMdFiles", s => delimitedMdFiles = s, defaultValue: delimitedMdFiles, helpName: "delimitedMdFiles", helpText: "Markdown files delimited by comma, only files with .md extension will be recognized"),
                };

            if (!ConsoleParameterParser.ParseParameters(options, args))
            {
                return;
            }

            // Input.1. docmta per ProjectReference
            // TODO: what if the file is extremely large?
            AssemblyDocMetadata assemblyMta = LoadAssemblyDocMetadataFromFile(mtaFile);

            // Input.2. a set of md files user inputs
            // Scan available md files' yaml headers to get available custom markdown content
            // TODO: what if the collection is extremely large?
            string[] mdFiles = (string[])_delimitedArrayConverter.ConvertFrom(delimitedMdFiles);
            MarkdownCollectionCache markdownCollectionCache = new MarkdownCollectionCache(mdFiles);

            // Step.1. create the id-path mapping
            foreach (var ns in assemblyMta.Namespaces)
            {
                string assemblyFile = ns.Id.ToString().ToValidFilePath() + ".html";
                idPathRelativeMapping.Add(ns.Id, assemblyFile);
                foreach (var c in ns.Classes)
                {
                    string classPath = Path.Combine(ns.Id.ToString().ToValidFilePath(), c.Id.ToString().ToValidFilePath() + ".html");
                    foreach (var m in c.Methods)
                    {
                        idPathRelativeMapping.Add(m.Id, classPath);
                    }
                    idPathRelativeMapping.Add(c.Id, classPath);
                }
            }

            // Step.2. write contents to those files
            Directory.CreateDirectory(outputDirectory);
            string classTemplate = File.ReadAllText(Path.Combine(templateDirectory, "class.html"));
            string nsTemplate = File.ReadAllText(Path.Combine(templateDirectory, "namespace.html"));
            //Add baseUrl to the template,this is for @ link
            viewModel.baseURL = Path.Combine(System.Environment.CurrentDirectory, outputDirectory) + "/";
            viewModel.assemblyMta = assemblyMta;

            foreach (var ns in assemblyMta.Namespaces)
            {
                viewModel.namespaceMta = ns;
                string content;
                if (markdownCollectionCache.TryGetValue(ns.Id, out content))
                {
                    ns.MarkdownContent = mdConvertor.ConvertToHTML(content);
                }
                string assemblyFolder = Path.Combine(outputDirectory, ns.Id.ToString().ToValidFilePath());
                string assemblyFile = assemblyFolder + ".html";
                //This may not be a good solution, just display the summary of triple slashes
                ns.XmlDocumentation = "###summary###" + TripleSlashPraser.Parse(ns.XmlDocumentation)["summary"];
                ns.XmlDocumentation = mdConvertor.ConvertToHTML(ns.XmlDocumentation);
                string result;

                Directory.CreateDirectory(assemblyFolder);
                foreach(var c in ns.Classes)
                {
                    viewModel.classMta = c;
                    //This may not be a good solution, just display the summary of triple slashes
                    c.XmlDocumentation = "###summary###" + TripleSlashPraser.Parse(c.XmlDocumentation)["summary"];
                    c.XmlDocumentation = mdConvertor.ConvertToHTML(c.XmlDocumentation);
                    if (markdownCollectionCache.TryGetValue(c.Id, out content))
                    {
                        c.MarkdownContent = mdConvertor.ConvertToHTML(content);
                    }
                    foreach (var m in c.Methods)
                    {
                        viewModel.methodMta = m;
                        //This may not be a good solution, just display the summary of triple slashes
                        m.XmlDocumentation = "###summary###" + TripleSlashPraser.Parse(m.XmlDocumentation)["summary"];
                        m.XmlDocumentation = mdConvertor.ConvertToHTML(m.XmlDocumentation);
                        if (markdownCollectionCache.TryGetValue(m.Id, out content))
                        {
                            m.MarkdownContent = mdConvertor.ConvertToHTML(content);
                        }
                    }

                    string classPath = Path.Combine(assemblyFolder, c.Id.ToString().ToValidFilePath() + ".html");
                    result = Razor.Parse(classTemplate, viewModel);
                    File.WriteAllText(classPath, result);
                    Console.Error.WriteLine("Successfully saved {0}", classPath);
                }
                result = Razor.Parse(nsTemplate, viewModel);
                File.WriteAllText(assemblyFile, result);
                Console.Error.WriteLine("Successfully saved {0}", assemblyFile);
            }

            //Copy the css and js
            CopyDir(Path.Combine(templateDirectory,cssDirecotry), Path.Combine(outputDirectory, cssDirecotry), true, true);
            CopyDir(Path.Combine(templateDirectory,scriptDirecotry), Path.Combine(outputDirectory, scriptDirecotry), true, true);
        }

        static string GetRealName(string id)
        {
            return id.Substring(id.LastIndexOf(":") + 1);
        }

        static void CopyDir(string sourceDir, string targetDir, bool overwrite, bool copySubdir)
        {
            Directory.CreateDirectory(targetDir);
            //Copy files   
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, Path.GetFileName(sourceFileName));
                if (File.Exists(targetFileName))
                {
                    if (overwrite == true)
                    {
                        File.Copy(sourceFileName, targetFileName, overwrite);
                    }
                }
                else
                {
                    File.Copy(sourceFileName, targetFileName, overwrite);
                }
            }
            //copy sub directories
            if (copySubdir)
            {
                foreach (string sourceSubdir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubdir.Substring(sourceSubdir.LastIndexOf("\\") + 1));
                    if (!Directory.Exists(targetSubDir))
                        Directory.CreateDirectory(targetSubDir);
                    CopyDir(sourceSubdir, targetSubDir, overwrite, true);
                }
            }
        }

        static string GetHtmlOutputFile(Identity id)
        {
            return id.ToString().ToValidFilePath() + ".html";
        }

        public static AssemblyDocMetadata LoadAssemblyDocMetadataFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return DocMetadataExtensions.LoadMetadata<AssemblyDocMetadata>(reader);
            }
        }
    }

    public class MarkdownCollectionCache : Dictionary<string, string>
    {
        public MarkdownCollectionCache(IEnumerable<string> mdFiles)
            : base(mdFiles.Where(s => s.EndsWith(".md", StringComparison.OrdinalIgnoreCase)).SelectMany(s => MarkdownFile.Load(s).Sections).Where(s => s != null).ToDictionary(s => s.Id, s => s.MarkdownContent))
        {
        }
    }

    public class MarkdownFile
    {
        private static CommentIdToYamlHeaderConverter _converter = new CommentIdToYamlHeaderConverter();

        public string FilePath { get; set; }

        public IReadOnlyList<MarkdownSection> Sections { get; set; }

        /// <summary>
        /// TODO: Load from md file
        /// ---
        /// method: A()
        /// ---
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static MarkdownFile Load(string filePath)
        {
            MarkdownFile file;
            TryParseCustomizedMarkdown(filePath, out file);

            return file;
        }

        public class MarkdownSection
        {
            public string Id { get; set; }

            public string MarkdownContent { get; set; }

            public int ContentStartIndex { get; set; }

            public int ContentEndIndex { get; set; }
        }
        
        private static bool TryParseCustomizedMarkdown(string markdownFilePath, out MarkdownFile markdown)
        {
            string markdownFile = File.ReadAllText(markdownFilePath);
            int length = markdownFile.Length;
            Dictionary<string, MarkdownSection> sections = new Dictionary<string, MarkdownSection>();
            var yamlRegex = CommentIdToYamlHeaderConverter.YamlHeaderRegex;
            MatchCollection matches = yamlRegex.Matches(markdownFile);
            if (matches.Count == 0)
            {
                markdown = null;
                return false;
            }

            int startIndex = 0;
            string lastCommentId = null;
            MarkdownSection lastSection = null;
            markdown = new MarkdownFile { FilePath = markdownFilePath };

            MarkdownSection section;

            // TODO: current assumption is : match is in order, need confirmation
            foreach (Match match in matches)
            {
                lastCommentId = (string)_converter.ConvertFrom(match.Value);
                startIndex = match.Index + match.Length;

                // If current Id is already set, ignore the duplicate one
                if (lastSection != null && !sections.TryGetValue(lastCommentId, out section))
                {
                    lastSection.ContentEndIndex = match.Index - 1;
                    if (lastSection.ContentEndIndex >= lastSection.ContentStartIndex)
                    {
                        lastSection.MarkdownContent = markdownFile.Substring(lastSection.ContentStartIndex, lastSection.ContentEndIndex - lastSection.ContentStartIndex + 1);
                        sections.Add(lastCommentId, lastSection);
                    }
                }

                lastSection = new MarkdownSection { Id = lastCommentId, ContentStartIndex = startIndex, ContentEndIndex = length }; // endIndex should be set from next match if there is next match
            }

            if (lastSection != null && !sections.TryGetValue(lastCommentId, out section))
            {
                sections.Add(lastCommentId, lastSection);
            }

            markdown.Sections = sections.Values.ToList();
            return true;
        }
    }

    /// <summary>
    /// Resolve the triple slashes
    /// </summary>
    public class TripleSlashPraser
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
        static public Dictionary<string, string> Parse(string tripleSlashStr)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach(string type in tripleSlashTypes)
            {
                string typeRegexPatten = string.Format(@"<{0}>(?<typeContent>[\s\S]*?)</{0}>",type);
                Regex typeRegex = new Regex(typeRegexPatten, RegexOptions.Compiled | RegexOptions.Multiline);
                result.Add(type,typeRegex.Match(tripleSlashStr).Groups["typeContent"].Value);
            }
            return result;
        }
    }

    public class MarkDownConvertor
    {
        private Markdown markdown = new Markdown();
        Dictionary<string, string> idPathRelativeMapping;
        public void init(Dictionary<string, string> iprm)
        {
            markdown.AutoHyperlink = true;
            markdown.AutoNewLines = true;
            markdown.EmptyElementSuffix = ">";
            markdown.EncodeProblemUrlCharacters = true;
            markdown.LinkEmails = false;
            markdown.StrictBoldItalic = true;
            idPathRelativeMapping = iprm;
        }
        public string ConvertToHTML(string md)
        {
            string result = ResolveAT(md);
            result = markdown.Transform(result);
            return result;
        }
        public string ResolveAT(string md)
        {
            md = Regex.Replace(md, @"@(?<ATcontent>\S*)\s", new MatchEvaluator(ATResolver), RegexOptions.Compiled);
            return md;
        }
        private string ATResolver(Match match)
        {
            string filePath;
            string id = match.Groups["ATcontent"].Value;
            if (idPathRelativeMapping.TryGetValue(id,out filePath))
            {
                return string.Format("[{0}]({1})", match.Value, filePath); ;
            }
            return match.Value;
        }
    }

    public class ViewModel
    {
        public string baseURL;
        public AssemblyDocMetadata assemblyMta;
        public NamespaceDocMetadata namespaceMta;
        public ClassDocMetadata classMta;
        public MethodDocMetadata methodMta;

        public ViewModel()
        {
            
        }
    }

}

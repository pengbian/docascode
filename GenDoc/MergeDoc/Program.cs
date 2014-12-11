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

namespace DocAsCode.MergeDoc
{
    class Option
    {
        public string Name { get; set; }
        public string HelpName { get; set; }
        public string DefaultValue { get; set; }
        public Action<string> Setter { get; set; }
        public bool Required { get; set; }
        public string HelpText { get; set; }

        public Option(string name, Action<string> setter, string helpName = null, bool required = false, string defaultValue = null, string helpText = null)
        {
            Name = name;
            Setter = setter;
            HelpName = helpName;
            DefaultValue = defaultValue;
            Required = required;
            HelpText = helpText;
        }
    }

    class ConsoleParameterParser
    {
        static string[] helpCommand = new[] { "/?", "?", "-?", "/help", "/h", "--help", "-h", "--?" };
        public static bool ParseParameters(IEnumerable<Option> options, string[] args)
        {
            if (args.Length == 2 && helpCommand.Contains(args[1], StringComparer.OrdinalIgnoreCase))
            {
                PrintUsage(options);
                return false;
            }

            // Set default value first
            foreach (var o in options)
            {
                o.Setter(o.DefaultValue);
            }

            Regex paramRegex = new Regex(@"^/(\w+):(.+)$");
            var optionDict = options.ToDictionary(o => o.Name ?? string.Empty, o => o);
            try
            {
                foreach (var a in args)
                {
                    var match = paramRegex.Match(a);
                    string key;
                    string value;
                    if (match.Success && optionDict.ContainsKey(match.Groups[1].Value))
                    {
                        key = match.Groups[1].Value;
                        value = match.Groups[2].Value;
                    }
                    else if (optionDict.ContainsKey(string.Empty))
                    {
                        key = string.Empty;
                        value = a;
                    }
                    else throw new ArgumentException(string.Format("Unrecognized parameter: {0}.", a));
                    optionDict[key].Setter(value);
                    optionDict.Remove(key);
                }

                foreach (var o in optionDict.Values)
                {
                    if (o.Required) throw new ArgumentException(string.Format("{0} is not specified.", o.HelpName));
                }

                return true;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                PrintUsage(options);
                return false;
            }
        }

        static void PrintUsage(IEnumerable<Option> options)
        {
            if (options == null)
            {
                return;
            }

            // Write usage
            Console.Write(Process.GetCurrentProcess().ProcessName);
            foreach (var o in options)
            {
                Console.Write(" ");
                if (!o.Required) Console.Write("[");
                if (o.Name != null) Console.Write("/{0}:", o.Name);
                Console.Write(o.HelpName);
                if (!o.Required) Console.Write("]");
            }

            Console.WriteLine();
            Console.WriteLine("Available parameters:");
            foreach (var o in options)
            {
                if (o.Name != null) Console.Write("/{0}:", o.Name);
                Console.WriteLine(o.HelpName);
                if (o.HelpText != null) Console.WriteLine("  " + o.HelpText.Replace(Environment.NewLine, Environment.NewLine + "  "));
            }
        }
    }

    class Program
    {
        private static DelimitedStringArrayConverter _delimitedArrayConverter = new DelimitedStringArrayConverter();
        static void Main(string[] args)
        {
            string mtaFile = @"TestData\GenDocMetadata.docmta";
            string delimitedMdFiles = @"TestData\T_GenDocMetadata.AssemblyDocMetadata.md";
            string outputDirectory = "output";
            string templateDirectory = "Templates";
            string cssDirecotry = "Css";
            string scriptDirecotry = "Script";

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

            Directory.CreateDirectory(outputDirectory);

            string classTemplate = File.ReadAllText(Path.Combine(templateDirectory, "class.html"));
            string nsTemplate = File.ReadAllText(Path.Combine(templateDirectory, "namespace.html"));

            foreach (var ns in assemblyMta.Namespaces)
            {
                string content;
                if (markdownCollectionCache.TryGetValue(ns.Id, out content))
                {
                    ns.MarkdownContent = content;
                }
                string assemblyFolder = Path.Combine(outputDirectory, ns.Id.ToString().ToValidFilePath());
                string assemblyFile = assemblyFolder + ".html";
                string result = Razor.Parse(nsTemplate, ns);
                File.WriteAllText(assemblyFile, result);
                Console.Error.WriteLine("Successfully saved {0}", assemblyFile);

                Directory.CreateDirectory(assemblyFolder);
                foreach(var c in ns.Classes)
                {
                    if (markdownCollectionCache.TryGetValue(c.Id, out content))
                    {
                        c.MarkdownContent = content;
                    }
                    foreach (var m in c.Methods)
                    {
                        if (markdownCollectionCache.TryGetValue(m.Id, out content))
                        {
                            m.MarkdownContent = content;
                        }
                    }

                    string classPath = Path.Combine(assemblyFolder, c.Id.ToString().ToValidFilePath() + ".html");
                    result = Razor.Parse(classTemplate, c);
                    File.WriteAllText(classPath, result);
                    Console.Error.WriteLine("Successfully saved {0}", classPath);
                }
            }

            //Copy the css and js
            CopyDir(cssDirecotry, outputDirectory + @"\css", true, true);
            CopyDir(scriptDirecotry, outputDirectory + @"\script", true, true);
        }

        static string GetRealName(string id)
        {
            return id.Substring(id.LastIndexOf(":") + 1);
        }

        static void CopyDir(string sourceDir, string targetDir, bool overWrite, bool copySubDir)
        {
            Directory.CreateDirectory(targetDir);
            //Copy files   
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf("\\") + 1));
                if (File.Exists(targetFileName))
                {
                    if (overWrite == true)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Copy(sourceFileName, targetFileName, overWrite);
                    }
                }
                else
                {
                    File.Copy(sourceFileName, targetFileName, overWrite);
                }
            }
            //copy sub directories
            if (copySubDir)
            {
                foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubDir.Substring(sourceSubDir.LastIndexOf("\\") + 1));
                    if (!Directory.Exists(targetSubDir))
                        Directory.CreateDirectory(targetSubDir);
                    CopyDir(sourceSubDir, targetSubDir, overWrite, true);
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

}

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

namespace DocAsCode.MergeDoc
{
    class Program
    {
        static string[] helpCommand = new[] { "/?", "?", "-?", "/help", "/h", "--help", "-h", "--?" };
        static void PrintUsage()
        {
            Console.Error.WriteLine(@"Usage: 
    MergeDoc    (<.docmta File Path>=TestData\GenDocMetadata.docmta)
                (<MarkdownDirectory>=TestData) 
                (<OutputDirectory>=output)
                (<TemplateDirectory>=Templates)");
        }

        static void Main(string[] args)
        {
            if (args.Length == 1 && helpCommand.Contains(args[0]))
            {
                PrintUsage();
                return;
            }

            string mtaFile = @"TestData\GenDocMetadata.docmta";
            string mdDirectory = "TestData";
            string outputDirectory = "output";
            string templateDirecotry = "Templates";
            if (args.Length > 0)
            {
                mtaFile = args.Length > 0 ? args[0] : mtaFile;
                mdDirectory = args.Length > 1 ? args[1] : mdDirectory;
                outputDirectory = args.Length > 2 ? args[2] : outputDirectory;
                templateDirecotry = args.Length > 3 ? args[3] : templateDirecotry;
            }

            // Input.1. docmta per ProjectReference
            // TODO: what if the file is extremely large?
            AssemblyDocMetadata assemblyMta = LoadAssemblyDocMetadataFromFile(mtaFile);
            
            // Input.2. a set of md files user inputs
            // Scan available md files' yaml headers to get available custom markdown content
            // TODO: what if the collection is extremely large?
            MarkdownCollectionCache markdownCollectionCache = new MarkdownCollectionCache(mdDirectory);

            Directory.CreateDirectory(outputDirectory);

            string classTemplate = File.ReadAllText(Path.Combine(templateDirecotry, "class.html"));
            string nsTemplate = File.ReadAllText(Path.Combine(templateDirecotry, "namespace.html"));

            foreach (var ns in assemblyMta.Namespaces)
            {
                string content;
                if (markdownCollectionCache.TryGetValue(ns.Id, out content))
                {
                    ns.MarkdownContent = content;
                }
                string assemblyFolder = Path.Combine(outputDirectory, ns.Id.ToString().ToValidFilePath());
                string assemblyFile = assemblyFolder + ".html";
                string result = Razor.Parse(nsTemplate, assemblyMta);
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
        public MarkdownCollectionCache(string directory)
            : base(Directory.EnumerateFiles(directory, "*.md").SelectMany(s => MarkdownFile.Load(s).Sections).Where(s => s != null).ToDictionary(s => s.Id, s => s.MarkdownContent))
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

            // TODO: current assumption is : match is in order, need confirmation
            foreach (Match match in matches)
            {
                lastCommentId = (string)_converter.ConvertFrom(match.Value);
                startIndex = match.Index + match.Length;

                MarkdownSection section;
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

            markdown.Sections = sections.Values.ToList();
            return true;
        }
    }

}

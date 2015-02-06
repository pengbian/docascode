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
    public class Program
    {
        private static DelimitedStringArrayConverter _delimitedArrayConverter = new DelimitedStringArrayConverter();
        static int Main(string[] args)
        {
            string mtaFile = @"C:\Users\t-hax\myCode\roslynCopy\src\DocProject1\bin\Debug\doctemp\mta\Microsoft.CodeAnalysis.docmta";
            string delimitedMdFiles = @"C:\Users\t-hax\myCode\roslynCopy\src\DocProject1\Docs\CodeAnalysis\T_Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.md,C:\Users\t-hax\myCode\roslynCopy\src\DocProject1\Docs\CodeAnalysis\T_Microsoft.CodeAnalysis.Emit.EmitOptions.md";
            string outputDirectory = "output";
            string templateDirectory = "Templates";
            string cssDirecotry = "css";
            string scriptDirecotry = "script";
            string publishBaseUrl = "";
            try
            {
                var options = new Option[]
                    {
                    new Option(null, s => mtaFile = s, helpName: "metadataFile", defaultValue: mtaFile, helpText: @"The doc metadata file, only file with .docmta is recognized"),
                    new Option("outputDirectory", s => outputDirectory = s, defaultValue: outputDirectory, helpText: "The output html files will be generated into this folder"),
                    new Option("templateDirectory", s => templateDirectory = s, defaultValue: templateDirectory, helpText: "The template folder for generated html, the folder should contain class.html and namespace.html, the former one is the html template for Class pages, and the latter one is the html template for Namespace pages"),
                    new Option("delimitedMdFiles", s => delimitedMdFiles = s, defaultValue: delimitedMdFiles, helpName: "delimitedMdFiles", helpText: "Markdown files delimited by comma, only files with .md extension will be recognized"),
                    new Option("publishBaseUrl", s => publishBaseUrl = s, defaultValue: publishBaseUrl, helpText: "Specify the baseurl of the published htmls."),
                    };

                if (!ConsoleParameterParser.ParseParameters(options, args))
                {
                    return 1;
                }

                // Input.1. docmta per ProjectReference
                // TODO: what if the file is extremely large?
                AssemblyDocMetadata assemblyMta = LoadAssemblyDocMetadataFromFile(mtaFile);

                // Input.2. a set of md files user inputs
                // Scan available md files' yaml headers to get available custom markdown content
                // TODO: what if the collection is extremely large?
                string[] mdFiles = (string[])_delimitedArrayConverter.ConvertFrom(delimitedMdFiles);
                MarkdownCollectionCache markdownCollectionCache = new MarkdownCollectionCache(mdFiles);

                // Step.2. write contents to those files
                Directory.CreateDirectory(outputDirectory);
                string classTemplate = File.ReadAllText(Path.Combine(templateDirectory, "class-ios.html"));
                string nsTemplate = File.ReadAllText(Path.Combine(templateDirectory, "namespace-ios.html"));
                string asmTemplate = File.ReadAllText(Path.Combine(templateDirectory, "index.html"));

                ViewModel viewModel = new ViewModel(assemblyMta, mtaFile, markdownCollectionCache);

                //Add baseUrl to the template,this is for @ link
                if (publishBaseUrl == "")
                {
                    viewModel.baseURL = Path.Combine(System.Environment.CurrentDirectory, outputDirectory) + "/";
                }
                else
                {
                    viewModel.baseURL = publishBaseUrl;
                }

                string assemblyFile = Path.Combine(outputDirectory, "index.html");

                if (assemblyMta.Namespaces != null)
                {
                    foreach (var ns in assemblyMta.Namespaces)
                    {
                        viewModel.namespaceMta = ns;
                        string namespaceFolder = Path.Combine(outputDirectory, ns.Id.ToString().ToValidFilePath());
                        string namespaceFile = namespaceFolder + ".html";
                        string result;

                        Directory.CreateDirectory(namespaceFolder);
                        if (ns.Classes != null)
                        {
                            foreach (var c in ns.Classes)
                            { 
                                viewModel.classMta = c;
                                viewModel.ResolveContent();
                                string classPath = Path.Combine(namespaceFolder, c.Id.ToString().ToValidFilePath() + ".html");
                                result = Razor.Parse(classTemplate, viewModel);
                                File.WriteAllText(classPath, result);
                                Console.Error.WriteLine("Successfully saved {0}", classPath);
                            }
                        }
                        result = Razor.Parse(nsTemplate, viewModel);
                        File.WriteAllText(namespaceFile, result);
                        Console.Error.WriteLine("Successfully saved {0}", namespaceFile);
                    }
                }

                var resultAsm = Razor.Parse(asmTemplate, viewModel);
                File.WriteAllText(assemblyFile, resultAsm);
                Console.Error.WriteLine("Successfully saved {0}", assemblyFile);

                //Copy the css and js
                CopyDir(Path.Combine(templateDirectory, cssDirecotry), Path.Combine(outputDirectory, cssDirecotry), true, true);
                CopyDir(Path.Combine(templateDirectory, scriptDirecotry), Path.Combine(outputDirectory, scriptDirecotry), true, true);
                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failing in merging document from {0}: {1}", mtaFile, e);
                return 1;
            }
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


        public static AssemblyDocMetadata LoadAssemblyDocMetadataFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return DocMetadataExtensions.LoadMetadata<AssemblyDocMetadata>(reader);
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using Microsoft.CodeAnalysis.MSBuild;
using Newtonsoft.Json;
using System.Collections;
using DocAsCode.Utility;
using System.ComponentModel;
using EntityModel;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace DocAsCode.BuildMeta
{
    enum BuildType
    {
        GenerateMetadata,
        BuildDocumentation,
        PublishDocumentation
    }

    public class Program
    {
        static int Main(string[] args)
        {
            const string DefaultOutputListFileName = "output.list";
            const string DefaultTocFileName = "toc.yaml";
            const string DefaultIndexFileName = "index.yaml";
            const string DefaultMarkdownFileName = "md.yaml";
            const string DefaultApiFolderName = "api";
            const string DefaultOutputFolderName = "output";
            string projectListFile = null;
            string outputFolder = null;
            string outputListFile = null;
            string markdownListFile = null;
            string markdownIndexFileName = null;
            string tocFileName = null;
            string indexFileName = null;
            string apiFolderName = null;

            try
            {
                var options = new Option[]
                    {
                    new Option(null, p => projectListFile = p, helpName: "projectListFile", required: true, helpText: "Required. Specify [1. the file path with extension .list | 2. the file list seperated by comma(,)] that contains all the project/solution paths whose metadata is to be generated."),
                    new Option("m", s => markdownListFile = s, defaultValue: null, helpName: "markdownListFile", helpText: "Specify the file path that contains all the additional markdown files. (default: null)."),
                    new Option("o", s => outputFolder = s, defaultValue: DefaultOutputFolderName, helpName: "outputFolder", helpText: "Specify the output folder that contains all the generated metadata files. (default: " + DefaultOutputFolderName + ")."),
                    new Option("toc", s => tocFileName = s, defaultValue: DefaultTocFileName, helpName: "tocFileName", helpText: "Specify the file name containing all the namespace yaml file paths. (default: " + DefaultTocFileName + ")."),
                    new Option("index", s => indexFileName = s, defaultValue: DefaultIndexFileName, helpName: "indexFileName", helpText: "Specify the file name containing all the available yaml files. (default: " + DefaultIndexFileName + ")."),
                    new Option("md", s => markdownIndexFileName = s, defaultValue: DefaultMarkdownFileName, helpName: "markdownIndexFileName", helpText: "Specify the file name containing all the markdown index. (default: " + DefaultMarkdownFileName + ")."),
                    new Option("folder", s => apiFolderName = s, defaultValue: DefaultApiFolderName, helpName: "apiFolderName", helpText: "Specify the subfolder name containing all the member's yaml file. (default: " + DefaultApiFolderName + ")."),
                    };

                if (!ConsoleParameterParser.ParseParameters(options, args))
                {
                    return 1;
                }
                outputListFile = Path.GetRandomFileName();
                BuildMetaHelper.GenerateMetadataFromProjectListAsync(projectListFile, outputListFile).Wait();
                BuildMetaHelper.MergeMetadataFromMetadataListAsync(outputListFile, outputFolder, indexFileName, BuildMetaHelper.MetadataType.Yaml).Wait();
                BuildMetaHelper.GenerateIndexForMarkdownListAsync(outputFolder, indexFileName, markdownListFile, markdownIndexFileName).Wait();
                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failing in generating metadata from {0}: {1}", projectListFile, e);
                return 1;
            }
        }
    }
}

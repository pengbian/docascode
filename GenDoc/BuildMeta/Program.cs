using System;
using System.IO;
using DocAsCode.Utility;
using EntityModel;

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
            const string DefaultMdFolderName = "md";
            const string DefaultApiFolderName = "api";
            const string DefaultOutputFolderName = "output";
            string inputProjectList = null;
            string outputFolder = null;
            string outputListFile = null;
            string inputMarkdownList = null;
            string outputMarkdownIndexFile = null;
            string outputTocFile = null;
            string outputIndexFile = null;
            string outputApiFolder = null;
            string outputMarkdownFolder = null;

            try
            {
                var options = new Option[]
                    {
                    new Option(null, p => inputProjectList = p, helpName: "inputProjectList", required: false, helpText: "Required. Specify the list of project files to generate documentation. Supported input types are: [1. the file path with extension .list | 2. the file paths seperated by comma(,)]."),
                    new Option("m", s => inputMarkdownList = s, defaultValue: null, helpName: "inputMarkdownList", helpText: "Specify the list of markdown files that contains all the additional documentations to the members in projects files. Supported input types are: [1. the file path with extension .list | 2. the file paths seperated by comma(,)]."),
                    new Option("o", s => outputFolder = s, defaultValue: DefaultOutputFolderName, helpName: "outputFolder", helpText: "Specify the output folder that contains all the generated metadata files. (default: " + DefaultOutputFolderName + ")."),
                    new Option("toc", s => outputTocFile = s, defaultValue: DefaultTocFileName, helpName: "outputTocFile", helpText: "Specify the file name containing all the namespace yaml file paths. (default: " + DefaultTocFileName + ")."),
                    new Option("index", s => outputIndexFile = s, defaultValue: DefaultIndexFileName, helpName: "outputIndexFile", helpText: "Specify the file name containing all the available yaml files. (default: " + DefaultIndexFileName + ")."),
                    new Option("md", s => outputMarkdownIndexFile = s, defaultValue: DefaultMarkdownFileName, helpName: "outputMarkdownIndexFile", helpText: "Specify the file name containing all the markdown index. (default: " + DefaultMarkdownFileName + ")."),
                    new Option("folder", s => outputApiFolder = s, defaultValue: DefaultApiFolderName, helpName: "outputApiFolder", helpText: "Specify the output subfolder name containing all the member's yaml file. (default: " + DefaultApiFolderName + ")."),
                    new Option("mdFolder", s => outputMarkdownFolder = s, defaultValue: DefaultMdFolderName, helpName: "outputMarkdownFolder", helpText: "Specify the output subfolder name containing all the markdown files copied. (default: " + DefaultMdFolderName+ ")."),
                    };

                if (!ConsoleParameterParser.ParseParameters(options, args))
                {
                    return 1;
                }

                if (inputProjectList == null || inputProjectList.Length == 0)
                {
                    ParseResult.WriteToConsole(ResultLevel.Warn, "No source project files are referenced. No documentation will be generated.");
                    return 2;
                }

                outputListFile = Path.GetRandomFileName() + ".list";
                BuildMetaHelper.GenerateMetadataFromProjectListAsync(inputProjectList, outputListFile).Wait();
                BuildMetaHelper.MergeMetadataFromMetadataListAsync(outputListFile, outputFolder, outputIndexFile, BuildMetaHelper.MetadataType.Yaml).Wait();
                BuildMetaHelper.GenerateIndexForMarkdownListAsync(outputFolder, outputIndexFile, inputMarkdownList, outputMarkdownIndexFile, outputMarkdownFolder).Wait();
                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failing in generating metadata from {0}: {1}", inputProjectList, e);
                return 1;
            }
        }
    }
}

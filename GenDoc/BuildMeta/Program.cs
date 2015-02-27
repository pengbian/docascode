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

    [Flags]
    public enum OutputType
    {
        Metadata = 0x1,
        Markdown = 0x2,
        Both = 0x3,
    }

    public class Program
    {
        static int Main(string[] args)
        {
            string slnOrProjectPath = null;
            string outputDirectory = null;
            string delimitedProjectFileNames = null;
            OutputType outputType = OutputType.Both;

            try
            {
                var options = new Option[]
                    {
                    new Option(null, s => slnOrProjectPath = s, helpName: "solutionPath/projectPath", required: true, helpText: @"The path of the solution or the project whose metadata is to be generated"),
                    new Option("o", s => outputDirectory = s, defaultValue: null, helpName: "outputDirectory", helpText: "The output metadata files will be generated into this folder. If not set, the default output directory would be under the current folder with the sln name"),
                    new Option("p", s => delimitedProjectFileNames = s, defaultValue: null, helpName: "delimitedProjectFiles", helpText: "Specifiy the project names whose metadata file will be generated, delimits files with comma"),
                    new Option("t", s => outputType = (OutputType)Enum.Parse(typeof(OutputType), s, true), defaultValue: outputType.ToString(), helpName: "outputType", helpText: "could be Both, Metadata or Markdown; specifiy if the docmta or the markdown file will be generated, by default is Both as both the docmta and the markdown file will be generated"),
                    };

                if (!ConsoleParameterParser.ParseParameters(options, args))
                {
                    return 1;
                }

                DocAsCodeUtility.GenerateMetadataAsync(slnOrProjectPath, outputDirectory, delimitedProjectFileNames, outputType).Wait();
                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failing in generating metadata from {0}: {1}", slnOrProjectPath, e);
                return 1;
            }
        }

        /// <summary>
        /// ProjectName <-> Namespace
        /// </summary>
        /// <param name="namespaceMapping"></param>
        /// <param name="baseDirectory"></param>
        //private static void ExportMetadataFile(IdentityMapping<NamespaceMetadata> namespaceMapping, string baseDirectory)
        //{
        //    if (Directory.Exists(baseDirectory))
        //    {
        //        Console.Error.WriteLine("Warning:" + string.Format("Directory {0} already exists!", baseDirectory));
        //    }

        //    Directory.CreateDirectory(baseDirectory);

        //    // TODO: For metadata file, one file for each run(merge in the run) or per project per run(merge after the run)?
        //    //var metadataFilePath = Path.Combine(baseDirectory, (assemblyDocMetadata.Id + ".docmta").ToValidFilePath());

        //    //Console.WriteLine("Generating metadata file {0}", metadataFilePath);
        //    //using (StreamWriter streamWriter = new StreamWriter(metadataFilePath))
        //    //{
        //    //    assemblyDocMetadata.WriteMetadata(streamWriter);
        //    //}
        //}
    }
}

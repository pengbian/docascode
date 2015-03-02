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
using System.Diagnostics;

namespace DocAsCode.BuildMeta
{
    public static class BuildDocHelper
    {
        private const string DefaultOutputDirectory = "output";
        /// <summary>
        /// Input:
        /// 1. Multiple Json Files with ProjectName as FileName? seems no need
        /// 2. List of conceptual file names or file containing the filename list?
        /// 3. templatedirectory?
        /// Output:
        /// A Directory containing folder structure and output markdown files? or html?
        /// </summary>
        /// <param name="slnOrProjectPath"></param>
        /// <param name="outputDirectory"></param>
        /// <param name="delimitedProjectFileNames"></param>
        /// <param name="outputType"></param>
        /// <returns></returns>
        public static async Task BuildDocAsync(string slnOrProjectPath, string outputDirectory, string delimitedProjectFileNames, OutputType outputType)
        {
            if (string.IsNullOrEmpty(slnOrProjectPath))
            {
                throw new ArgumentNullException("slnOrProjectPath");
            }

            List<Project> projects = new List<Project>();
            var fileExtension = Path.GetExtension(slnOrProjectPath);
            var workspace = MSBuildWorkspace.Create();
            if (fileExtension == ".sln")
            {
                var solution = await workspace.OpenSolutionAsync(slnOrProjectPath);
                projects = solution.Projects.ToList();
            }
            else if (fileExtension == ".csproj")
            {
                var project = await workspace.OpenProjectAsync(slnOrProjectPath);
                projects.Add(project);
            }
            else
            {
                throw new NotSupportedException(string.Format("Project type {0} is currently not supported", fileExtension));
            }

            if (projects.Count == 0)
            {
                Console.Error.WriteLine("No project is found under {0}, exiting...", slnOrProjectPath);
            }

            if (string.IsNullOrEmpty(outputDirectory))
            {
                // use the sln/project name as the default output directory
                outputDirectory = Path.GetFileNameWithoutExtension(slnOrProjectPath);
            }

            if (Directory.Exists(outputDirectory))
            {
                Console.Error.WriteLine("Warning: {0} directory already exists.", outputDirectory);
            }

            Directory.CreateDirectory(outputDirectory);

            if (!string.IsNullOrEmpty(delimitedProjectFileNames))
            {
                string[] specifiedProjectFileNames = delimitedProjectFileNames.ToArray(StringSplitOptions.RemoveEmptyEntries, ',');
                if (specifiedProjectFileNames != null && specifiedProjectFileNames.Length > 0)
                {
                    var intersection = projects.Select(s => s.Name).Intersect(specifiedProjectFileNames, StringComparer.OrdinalIgnoreCase);
                    var invalidProjectFileNames = projects.Select(s => s.Name).Except(intersection).Distinct();
                    if (invalidProjectFileNames.Any())
                    {
                        Console.Error.WriteLine("Project(s) {0} is(are) not available in {1}, will be ignored", invalidProjectFileNames.ToDelimitedString(), slnOrProjectPath);
                    }

                    var excludedProjectNames = projects.Select(s => s.Name).Except(intersection);
                    if (excludedProjectNames.Any())
                    {
                        Console.Error.WriteLine("Project(s) {0} is(are) not in the specified file list, will be ignored.", excludedProjectNames.ToDelimitedString());
                    }

                    projects = projects.Where(s => intersection.Contains(s.Name)).ToList();
                }
            }
        }

        private static async Task BuildDocCoreAsync(string[] metadataFileNames, string[] conceptualFileNames, string outputDirectory)
        {
            if (metadataFileNames == null || metadataFileNames.Length == 0)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(outputDirectory))
            {
                outputDirectory = DefaultOutputDirectory;
            }

            if (Directory.Exists(outputDirectory))
            {
                Console.Error.WriteLine("Warning: {0} directory already exists.", outputDirectory);
            }
            else
            {
                Directory.CreateDirectory(outputDirectory);
            }

            List<ProjectMetadata> projectMetadataList = new List<ProjectMetadata>();
            // Step1. Load metadata files
            foreach(var metadataFile in metadataFileNames)
            {
                ProjectMetadata projectMetadata;
                string message;
                if (!TryParseMetadataFile(metadataFile, out projectMetadata, out message))
                {
                    Console.Error.WriteLine("Warning: Invalid metadata file {0} will be ignored, {1}", metadataFile, message);
                }
                else
                {
                    projectMetadataList.Add(projectMetadata);
                }
            }
            
            if (projectMetadataList.Count == 0)
            {
                Console.Error.WriteLine("Warning: None invalid file is found in {0}. Exiting...", metadataFileNames.ToDelimitedString());
                return;
            }

            // Step2. Merge metadata files into a MappingTable with Identity-IMetadata Mapping for later Resolver
            // SHOULD Check invalid situations
            // 1. When ProjectName duplicate in different metadata files
            // 2. When Same Id appears in different metadata files
            var namespaceMapping = MergeProjectMetadata(projectMetadataList);

            CommentVisitor<string> commentVisitor = new CommentVisitor<string>();
            foreach(var ns in namespaceMapping)
            {
                await ns.Value.AcceptAsync(commentVisitor, "");
            }
        }
        
        public static bool TryParseMetadataFile(string metadataFileName, out ProjectMetadata projectMetadata, out string message)
        {
            projectMetadata = null;
            message = string.Empty;
            try
            {
                using (StreamReader reader = new StreamReader(metadataFileName))
                {
                    projectMetadata = JsonUtility.Deserialize<ProjectMetadata>(reader);
                    return true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

        }

        /// <summary>
        /// Step2. Merge metadata files into a MappingTable with Identity-IMetadata Mapping for later Resolver
        /// SHOULD Check invalid situations
        /// 1. When ProjectName duplicate in different metadata files
        /// 2. When Same Id appears in different metadata files
        /// </summary>
        /// <param name="projectMetadataList"></param>
        /// <returns></returns>
        private static IdentityMapping<NamespaceMetadata> MergeProjectMetadata(IEnumerable<ProjectMetadata> projectMetadataList)
        {
            throw new NotImplementedException();
        }

        interface IPipeline
        {

        }

        class MarkdownPipeline
        {

        }

        class HtmlPipeline
        {

        }

        interface IPipelineComponent
        {

        }

        interface IValidation : IPipelineComponent
        {

        }

        class ResolveLink : IPipelineComponent
        {

        }

        class ResolveTripleSlashComments : IPipelineComponent
        {

        }

        class ResolveMarkdownComments : IPipelineComponent
        {

        }

        class GenerateHtmlOutput : IPipelineComponent
        {

        }

        class GenerateMarkdownOutput : IPipelineComponent
        {

        }

        /// <summary>
        /// Generate TOC according to folder structure? or according to a toc file?
        /// </summary>
        class GenerateToc : IPipelineComponent
        {

        }

        class ProjectNameValidation : IValidation
        {

        }

        class DuplicationIdentityValidation : IValidation
        {

        }

        class NamespacePageViewModel
        {

        }

        class ClassPageViewModel
        {

        }

        class IndexPageViewModel
        {

        }
    }
}

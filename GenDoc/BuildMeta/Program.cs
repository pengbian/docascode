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
    enum OutputType
    {
        Metadata = 0x1,
        Markdown = 0x2,
        Both = 0x3,
    }
    public static class DocAsCodeUtility
    {
        public static async Task<Tuple<bool, string>> TryGenerateMetadataAsync(string slnOrProjectPath, string outputDirectory, string delimitedProjectFileNames, OutputType outputType)
        {
            if (string.IsNullOrEmpty(slnOrProjectPath))
            {
                throw new ArgumentNullException("slnOrProjectPath");
            }

            List<Project> projects = new List<Project>();
            var fileExtension = Path.GetExtension(slnOrProjectPath);
            if (fileExtension == ".sln")
            {
                var solution = MSBuildWorkspace.Create().OpenSolutionAsync(slnOrProjectPath).Result;
                projects = solution.Projects.ToList();
            }
            else if (fileExtension == ".csproj")
            {
                var project = MSBuildWorkspace.Create().OpenProjectAsync(slnOrProjectPath).Result;
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

                }
            }


            var intersection = projects.Select(s=>s.Name).Intersect(specifiedProjectFileNames, StringComparer.OrdinalIgnoreCase);
            var invalidProjectFileNames = projects.Select(s => s.Name).Except(intersection).Distinct();
            Console.Error.WriteLine("Project(s) {0} is(are) not available in {1}, will be ignored", invalidProjectFileNames.ToDelimitedString(), slnOrProjectPath);

            var excludedProjectNames = availableProjectNames.Except(intersection);
            Console.Error.WriteLine("Project(s) {0} is(are) not in the specified file list, will be ignored.", excludedProjectNames.ToDelimitedString());

            foreach (var project in projects)
            {
                if (intersection.Contains(project.Name))
                {
                    continue;
                }

                var namespaceMapping = await GenerateMetadataAsync(projects.Where(s => intersection.Contains(s.Name)));

                if (outputType.HasFlag(OutputType.Metadata))
                {
                    //ExportMetadataFile(assemblyDocMetadata, Path.Combine(outputDirectory, "mta"));
                }

                if (outputType.HasFlag(OutputType.Markdown))
                {
                    throw new NotImplementedException();
                }
            }

            Console.WriteLine("Metadata files successfully generated under {0}", outputDirectory);
        }
        private class IdentityMapping<T> : ConcurrentDictionary<Identity, T>
        {
        }

        private static async Task<IdentityMapping<NamespaceMetadata>> GenerateMetadataAsync(IEnumerable<Project> projects)
        {
            if (projects == null || !projects.Any())
            {
                return null;
            }

            IdentityMapping<NamespaceMetadata> namespaceMapping = new IdentityMapping<NamespaceMetadata>();

            // Project.Name is unique per solution
            foreach (var project in projects)
            {
                var compilation = await project.GetCompilationAsync();
                var namespaceMembers = compilation.Assembly.GlobalNamespace.GetNamespaceMembers();
                var namespaceQueue = new Queue<INamespaceSymbol>();

                foreach (var ns in namespaceMembers)
                {
                    namespaceQueue.Enqueue(ns);
                }

                while (namespaceQueue.Count > 0)
                {
                    var ns = namespaceQueue.Dequeue();
                    foreach (var namespaceMember in ns.GetNamespaceMembers())
                    {
                        namespaceQueue.Enqueue(namespaceMember);
                    }

                    var nsMembers = ns.GetTypeMembers();

                    // Ignore current namespace if it contains none members
                    if (!nsMembers.Any())
                    {
                        continue;
                    }

                    NamespaceMetadata nsMetadata = await MetadataExtractorManager.ExtractAsync(ns) as NamespaceMetadata;

                    // Namespace(N)--(N)Project is N-N mapping
                    nsMetadata = namespaceMapping.GetOrAdd(nsMetadata.Identity, nsMetadata);

                    if (nsMetadata != null)
                    {
                        foreach (var nsMember in nsMembers)
                        {
                            NamespaceMemberMetadata nsMemberMetadata = await MetadataExtractorManager.ExtractAsync(nsMember) as NamespaceMemberMetadata;
                            if (nsMemberMetadata != null)
                            {
                                // Will not check if members with duplicate Identify exists
                                // Should not ignore the namespace's member even if it contains nothing because we can override the comments in markdown
                                nsMetadata.Members.Add(nsMemberMetadata);

                                // Fulfill the nsMemberMetadata with detailed info extracted from symbol as well as its members
                                var nsMembersMembers = nsMember.GetTypeMembers();
                                foreach (var nsMembersMember in nsMembersMembers)
                                {
                                    NamespaceMembersMemberMetadata nsMembersMembersMetadata = await MetadataExtractorManager.ExtractAsync(nsMembersMember) as NamespaceMembersMemberMetadata;
                                    if (nsMembersMembersMetadata != null)
                                    {
                                        nsMemberMetadata.Members.Add(nsMembersMembersMetadata);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return namespaceMapping;
        }
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

                var generateMetadataResult = DocAsCodeUtility.TryGenerateMetadataAsync(slnOrProjectPath, outputDirectory, delimitedProjectFileNames, outputType).Result;
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

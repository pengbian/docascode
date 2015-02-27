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
    public static class DocAsCodeUtility
    {
        public static async Task GenerateMetadataAsync(string slnOrProjectPath, string outputDirectory, string delimitedProjectFileNames, OutputType outputType)
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

            foreach(var project in projects)
            {
                var namespaceMapping = await GenerateMetadataAsync(project);
                Debug.Assert(namespaceMapping != null);

                // Each project has its own metadata file
                if (outputType.HasFlag(OutputType.Metadata))
                {
                    ExportMetadataFile(namespaceMapping, Path.Combine(outputDirectory, "mta"));
                    Console.WriteLine("Metadata file for {0} is successfully generated under {1}", project.Name, outputDirectory);
                }

                if (outputType.HasFlag(OutputType.Markdown))
                {
                    throw new NotImplementedException();
                }

                Console.WriteLine("Metadata files successfully generated under {0}", Path.GetFullPath(outputDirectory));
            }
        }

        private static async Task<ProjectMetadata> GenerateMetadataAsync(Project project)
        {
            if (project == null)
            {
                return null;
            }

            IdentityMapping<NamespaceMetadata> namespaceMapping = new IdentityMapping<NamespaceMetadata>();

            ProjectMetadata projectMetadata = new ProjectMetadata
            {
                ProjectName = project.Name,
                Namespaces = namespaceMapping,
            };

            // Project.Name is unique per solution
            MetadataExtractContext context = new MetadataExtractContext
            {
                ProjectName = project.Name
            };

            var compilation = await project.GetCompilationAsync();
            var namespaceMembers = compilation.Assembly.GlobalNamespace.GetNamespaceMembers();
            var namespaceQueue = new Queue<INamespaceSymbol>();

            foreach (var ns in namespaceMembers)
            {
                await TreeIterator.PreorderAsync<INamespaceSymbol>(ns, s => s.GetNamespaceMembers(), s => GetNamespaceMembersAsync(s, context, namespaceMapping));
            }

            return projectMetadata;
        }

        private static async Task GetNamespaceMembersAsync(INamespaceSymbol ns, IMetadataExtractContext context, IdentityMapping<NamespaceMetadata> namespaceMapping)
        {
            // Get namespace members
            var nsMembers = ns.GetMembers();

            // Ignore current namespace if it contains none members
            if (!nsMembers.Any())
            {
                return;
            }

            NamespaceMetadata nsMetadata = await MetadataExtractorManager.ExtractAsync(ns, context) as NamespaceMetadata;

            // Namespace(N)--(N)Project is N-N mapping
            nsMetadata = namespaceMapping.GetOrAdd(nsMetadata.Identity, nsMetadata);

            if (nsMetadata != null)
            {
                foreach (var nsTypeMember in nsMembers)
                {
                    await TreeIterator.PreorderAsync<INamespaceOrTypeSymbol>(nsTypeMember, s => s.GetTypeMembers(), s => GetNamespaceMembersMemberAsync(s, context, nsMetadata));
                }
            }
        }

        private static async Task GetNamespaceMembersMemberAsync(INamespaceOrTypeSymbol nsMember, IMetadataExtractContext context, NamespaceMetadata nsMetadata)
        {
            NamespaceMemberMetadata nsMemberMetadata = await MetadataExtractorManager.ExtractAsync(nsMember, context) as NamespaceMemberMetadata;
            if (nsMemberMetadata != null)
            {
                // Will not check if members with duplicate Identify exists
                // Should not ignore the namespace's member even if it contains nothing because we can override the comments in markdown
                nsMetadata.Members.Add(nsMemberMetadata);

                // Fulfill the nsMemberMetadata with detailed info extracted from symbol as well as its members
                var nsMembersMembers = nsMember.GetMembers();
                foreach (var nsMembersMember in nsMembersMembers)
                {
                    NamespaceMembersMemberMetadata nsMembersMembersMetadata = await MetadataExtractorManager.ExtractAsync(nsMembersMember, context) as NamespaceMembersMemberMetadata;
                    if (nsMembersMembersMetadata != null)
                    {
                        nsMemberMetadata.Members.Add(nsMembersMembersMetadata);
                    }
                }
            }
        }

        class TreeIterator
        {
            public static async Task PreorderAsync<T>(T root, Func<T, IEnumerable<T>> childrenGetter, Func<T, Task> action)
            {
                if (root == null || action == null)
                {
                    return;
                }

                await action(root);

                if (childrenGetter == null)
                {
                    return;
                }

                var children = childrenGetter(root);
                if (children != null)
                {
                    foreach(var child in children)
                    {
                        await PreorderAsync(child, childrenGetter, action);
                    }
                }
            }
        }

        private static void ExportMetadataFile(ProjectMetadata projectMetadata, string baseDirectory)
        {
            if (Directory.Exists(baseDirectory))
            {
                Console.Error.WriteLine("Warning:" + string.Format("Directory {0} already exists!", baseDirectory));
            }
            else
            {
                Directory.CreateDirectory(baseDirectory);
            }

            // TODO: For metadata file, one file per project per run(merge after the run)
            var metadataFilePath = Path.Combine(baseDirectory, (projectMetadata.ProjectName + ".docmta").ToValidFilePath());

            //Console.WriteLine("Generating metadata file {0}", metadataFilePath);
            using (StreamWriter streamWriter = new StreamWriter(metadataFilePath))
            {
                JsonUtility.Serialize(projectMetadata, streamWriter);
            }
        }
    }
}

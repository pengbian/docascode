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
using EntityModel.ViewModel;
using YamlDotNet.Serialization;

namespace DocAsCode.BuildMeta
{
    public static class BuildMetaHelper
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
                    string baseDirectory = Path.Combine(outputDirectory, "mta");
                    if (Directory.Exists(baseDirectory))
                    {
                        Console.Error.WriteLine("Warning:" + string.Format("Directory {0} already exists!", baseDirectory));
                    }
                    else
                    {
                        Directory.CreateDirectory(baseDirectory);
                    }

                    // TODO: For metadata file, one file per project per run(merge after the run)
                    var metadataFilePath = Path.Combine(baseDirectory, (project.Name + ".docmta").ToValidFilePath());

                    string message;
                    if (!TryExportMetadataFile(namespaceMapping, metadataFilePath, ExportType.Json, out message) || !TryExportMetadataFile(namespaceMapping, metadataFilePath, ExportType.Yaml, out message))
                    {
                        Console.Error.WriteLine("Error: error trying export metadata file {0}, {1}", metadataFilePath, message);
                    }
                    else
                    {
                        Console.WriteLine("Metadata file for {0} is successfully generated under {1}", project.Name, outputDirectory);
                    }
                }

                if (outputType.HasFlag(OutputType.Markdown))
                {
                    throw new NotImplementedException();
                }

                Console.WriteLine("Metadata files successfully generated under {0}", Path.GetFullPath(outputDirectory));
            }
        }

        private class SymbolMetadataTable : ConcurrentDictionary<ISymbol, IMetadata>
        {
        }

        private static async Task<ProjectMetadata> GenerateMetadataAsync(Project project)
        {
            if (project == null)
            {
                return null;
            }

            IdentityMapping<NamespaceMetadata> namespaceMapping = new IdentityMapping<NamespaceMetadata>();

            // Stores the generated IMetadata with the relationship to ISymbol
            SymbolMetadataTable symbolMetadataTable = new SymbolMetadataTable();

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
                await TreeIterator.PreorderAsync<ISymbol>(
                    ns
                    , null
                    , s =>
                    {
                        var namespaceOrTypeSymbol = s as INamespaceOrTypeSymbol;
                        if (namespaceOrTypeSymbol != null)
                        {
                            return namespaceOrTypeSymbol.GetMembers();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    , (current, parent) => GenerateMetadataForEachSymbol(current, parent, context, namespaceMapping, symbolMetadataTable)
                    );
            }

            return projectMetadata;
        }

        private static async Task GenerateMetadataForEachSymbol(ISymbol current, ISymbol parent, IMetadataExtractContext context, IdentityMapping<NamespaceMetadata> namespaceMapping, SymbolMetadataTable symbolMetadataTable)
        {
            var namespaceSymbol = current as INamespaceSymbol;

            if (namespaceSymbol != null)
            {
                NamespaceMetadata nsMetadata = await MetadataExtractorManager.ExtractAsync(current, context) as NamespaceMetadata;
                context.OwnerNamespace = nsMetadata;

                // Namespace(N)--(N)Project is N-N mapping
                nsMetadata = namespaceMapping.GetOrAdd(nsMetadata.Identity, nsMetadata);

                Debug.Assert(!symbolMetadataTable.ContainsKey(current), "Should not contain current symbol in the first iteration");
                symbolMetadataTable.TryAdd(current, nsMetadata);
            }
            else
            {
                var currentAsTypeSymbol = current as ITypeSymbol;
                var parentAsNamespaceSymbol = parent as INamespaceSymbol;

                // Flatten TypeSymbol hierarchy
                if (currentAsTypeSymbol != null || parentAsNamespaceSymbol != null)
                {
                    IMetadata parentMetadata;
                    NamespaceMemberMetadata nsMemberMetadata = await MetadataExtractorManager.ExtractAsync(current, context) as NamespaceMemberMetadata;
                    if (nsMemberMetadata != null)
                    {
                        // Will not check if members with duplicate Identify exists
                        // Should not ignore the namespace's member even if it contains nothing because we can override the comments in markdown
                        ((NamespaceMetadata)context.OwnerNamespace).Members.Add(nsMemberMetadata);

                        Debug.Assert(!symbolMetadataTable.ContainsKey(current), "Should not contain current symbol in the first iteration");
                        symbolMetadataTable.TryAdd(current, nsMemberMetadata);
                    }
                }
                else
                {
                    IMetadata parentMetadata;
                    symbolMetadataTable.TryGetValue(parent, out parentMetadata);
                    var parentNamespaceMetadata = parentMetadata as NamespaceMemberMetadata;
                    Debug.Assert(parentNamespaceMetadata != null, "Non-INamespaceSymbol should correspond to NamespaceMemberMetadata");

                    NamespaceMembersMemberMetadata nsMembersMembersMetadata = await MetadataExtractorManager.ExtractAsync(current, context) as NamespaceMembersMemberMetadata;
                    if (nsMembersMembersMetadata != null)
                    {
                        parentNamespaceMetadata.Members.Add(nsMembersMembersMetadata);
                    }
                }
            }
        }

        class TreeIterator
        {
            public static async Task PreorderAsync<T>(T current, T parent, Func<T, IEnumerable<T>> childrenGetter, Func<T, T, Task> action)
            {
                if (current == null || action == null)
                {
                    return;
                }

                await action(current, parent);

                if (childrenGetter == null)
                {
                    return;
                }

                var children = childrenGetter(current);
                if (children != null)
                {
                    foreach(var child in children)
                    {
                        await PreorderAsync(child, current, childrenGetter, action);
                    }
                }
            }
        }

        public static bool TryExportMetadataFile(ProjectMetadata projectMetadata, string filePath, ExportType exportType, out string message)
        {
            message = string.Empty;
            try
            {
                if (exportType == ExportType.Json)
                {
                    filePath = Path.ChangeExtension(filePath, ".json");
                }
                else if (exportType == ExportType.Yaml)
                {
                    filePath = Path.ChangeExtension(filePath, ".yaml");
                }

                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    if (exportType == ExportType.Json)
                    {
                        JsonUtility.Serialize(projectMetadata, streamWriter);
                    }
                    else if (exportType == ExportType.Yaml)
                    {
                        Serializer serializer = new Serializer();
                        var viewModel = projectMetadata.ToYamlViewModel();
                        serializer.Serialize(streamWriter, viewModel.IndexYamlViewModel);
                        using (StreamWriter sw = new StreamWriter(Path.ChangeExtension(filePath, "toc.yaml")))
                        {
                            serializer.Serialize(sw, viewModel.TocYamlViewModel);
                        }
                        using (StreamWriter sw = new StreamWriter(Path.ChangeExtension(filePath, "md.yaml")))
                        {
                            serializer.Serialize(sw, viewModel.MarkdownYamlViewModel);
                        }
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
        }
    }

    public enum ExportType
    {
        Json,
        Yaml
    }
}

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

namespace GenDocMetadata
{
    public class Program //<T> where T : class, new() // comment
    {
        static void Main(string[] args)
        {
            if (args.Length > 2 || args.Length < 1)
            {
                PrintUsage();
                return;
            }

            var slnPath = args[0];
            var outputDirectory = Path.GetFileNameWithoutExtension(slnPath);
            if (args.Length == 2)
            {
                outputDirectory = args[1];
            }

            if (Directory.Exists(outputDirectory))
            {
                Console.Error.WriteLine("Warning: {0} directory already exists.", outputDirectory);
            }

            Directory.CreateDirectory(outputDirectory);
            try
            {
                var solution = MSBuildWorkspace.Create().OpenSolutionAsync(slnPath).Result;
                var projectIds = solution.GetProjectDependencyGraph().GetTopologicallySortedProjects();
                foreach (var projectId in projectIds)
                {
                    var project = solution.GetProject(projectId);
                    var assemblyDocMetadata = GenerateAssemblyDocMetadata(project);

                    // For metadata file, per assembly per file
                    var metadataFilePath = Path.Combine(outputDirectory, (assemblyDocMetadata.Id + ".docmta").ToValidFilePath());
                    Console.WriteLine("Generating metadata file {0}", metadataFilePath);
                    using (StreamWriter streamWriter = new StreamWriter(metadataFilePath))
                    {
                        assemblyDocMetadata.WriteMetadata(streamWriter);
                    }

                    assemblyDocMetadata.ExportMarkdownSkeletonToc(Path.Combine(outputDirectory, "mdtoc"));
                }

                Console.WriteLine("Metadata files successfully generated under {0}", outputDirectory);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failing in generating metadata from {0}: {1}", slnPath, e);
            }
        }

        static void PrintUsage()
        {
            Console.Error.WriteLine("Usage: GenDocMetadata <SolutionPath> (<OutputDirectory>)");
        }

        static AssemblyDocMetadata GenerateAssemblyDocMetadata(Project project)
        {
            var compilation = project.GetCompilationAsync().Result;

            // essentially you get the containing assembly of SpecialType.System_Object and get the assembly¡¯s version
            var mscolibAssembly = compilation.GetSpecialType(SpecialType.System_Object).ContainingAssembly.Identity;
            var version = mscolibAssembly.Version;

            // Get AssemblySymbols for above compilation and the assembly (mscorlib) referenced by it.
            IAssemblySymbol compilationAssembly = compilation.Assembly;

            AssemblyDocMetadata assemblyDocMetadata = new AssemblyDocMetadata(compilationAssembly.Name)
            {
                MscorlibVersion = version,
            };

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

                var types = ns.GetTypeMembers();

                if (!types.Any())
                {
                    continue;
                }

                // var identifier = syntaxRoot.DescendantNodes().OfType<NamespaceDeclarationSyntax>();
                var namespaceDocMetadata = DocMetadataConverterFactory.Convert(ns) as NamespaceDocMetadata;
                if (namespaceDocMetadata == null)
                {
                    continue;
                }

                assemblyDocMetadata.TryAddNamespace(namespaceDocMetadata);

                // Namespace:
                foreach (var type in types)
                {
                    var metadata = DocMetadataConverterFactory.Convert(type) as ClassDocMetadata;
                    if (metadata == null)
                    {
                        continue;
                    }

                    namespaceDocMetadata.TryAddClass(metadata);

                    foreach (var member in type.GetMembers())
                    {
                        var memberMta = DocMetadataConverterFactory.Convert(member) as MethodDocMetadata;
                        if (memberMta == null)
                        {
                            continue;
                        }

                        metadata.TryAddMethod(memberMta);
                    }
                }
            }

            return assemblyDocMetadata;
        }
    }
}

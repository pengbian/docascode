using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenDocMetadata
{
    /// <summary>
    /// Doc metadata is per project
    /// </summary>
    public class AssemblyDocMetadata : Metadata
    {
        private ConcurrentDictionary<Identity, NamespaceDocMetadata> _namespaces = new ConcurrentDictionary<Identity, NamespaceDocMetadata>();
        public IEnumerable<NamespaceDocMetadata> Namespaces { get { return _namespaces.Values; } }

        public AssemblyDocMetadata(string name) : base(name)
        {
        }

        public bool TryAddNamespace(NamespaceDocMetadata mta)
        {
            return _namespaces.TryAdd(mta.Id, mta);
        }

        /// <summary>
        /// |--AssemblyName
        ///         |--NamespaceId1
        ///                 |--NamespaceId1.md
        ///                 |--ClassId1.md
        ///                 |--ClassId2.md
        ///         |--NamepsaceId2
        ///                 |--NamespaceId2.md
        ///                 |--ClassId3.md
        /// </summary>
        /// <param name="directory"></param>
        public void ExportMarkdownSkeletonToc(string baseDirectory)
        {
            string markdownSkeletonTocDirectory = Path.Combine(baseDirectory, this.Id);
            if (Directory.Exists(markdownSkeletonTocDirectory))
            {
                Console.Error.WriteLine("Warning:" + string.Format("Directory {0} already exists!", markdownSkeletonTocDirectory));
            }

            if (!Namespaces.Any())
            {
                Console.Error.WriteLine("Warning:" + string.Format("No namespace is found inside current assembly {0}", this.Id));
            }

            Directory.CreateDirectory(markdownSkeletonTocDirectory);

            foreach(var @namespace in Namespaces)
            {
                @namespace.ExportMarkdownSkeletonToc(markdownSkeletonTocDirectory);
            }
        }
    }

    public class MemeberDocMetadata : Metadata
    {
        public Identity Namespace { get; set; }

        public Identity Assembly { get; set; }

        public SyntaxDocFragment Syntax { get; set; }

        public MemeberDocMetadata(string name) : base(name)
        {
        }

        public MemeberDocMetadata(Metadata mta) : base(mta.Id)
        {
            this.MscorlibVersion = mta.MscorlibVersion;
            this.XmlDocumentation = mta.XmlDocumentation;
        }

        public override void WriteMarkdownSkeleton(TextWriter writer)
        {
            base.WriteMarkdownSkeleton(writer);

            if (Syntax != null)
            {
                // TODO: convert from Syntax.Language
                writer.WriteLine("```csharp");
                writer.WriteLine(Syntax.Content);
                writer.WriteLine("```");
            }
        }
    }

    /// <summary>
    /// TODO: what if no namespace?
    /// </summary>
    public class NamespaceDocMetadata : MemeberDocMetadata
    {
        private ConcurrentDictionary<Identity, ClassDocMetadata> _classes =
            new ConcurrentDictionary<Identity, ClassDocMetadata>();
        public IEnumerable<ClassDocMetadata> Classes { get { return _classes.Values; } }
        public IReadOnlyList<InterfaceDocMetadata> Interfaces { get; set; }
        public IReadOnlyList<StructDocMetadata> Structs { get; set; }
        public IReadOnlyList<DelegateDocMetadata> Delegates { get; set; }
        public IReadOnlyList<EnumDocMetadata> Enums { get; set; }

        public NamespaceDocMetadata(string name) : base(name)
        {
        }

        public NamespaceDocMetadata(Metadata mta) : base(mta)
        {
        }

        public bool TryAddClass(ClassDocMetadata classMetadata)
        {
            return _classes.TryAdd(classMetadata.Id, classMetadata);
        }

        /// <summary>
        ///         |--NamespaceId1
        ///                 |--NamespaceId1.md
        ///                 |--ClassId1.md
        ///                 |--ClassId2.md
        /// </summary>
        /// <param name="directory"></param>
        public void ExportMarkdownSkeletonToc(string baseDirectory)
        {
            string directory = Path.Combine(baseDirectory, this.Id.ToString().ToValidFilePath());
            if (Directory.Exists(directory))
            {
            }

            if (!Classes.Any())
            {
                Console.Error.WriteLine("Warning:" + string.Format("No class is found inside current assembly {0}", this.Id));
            }

            Directory.CreateDirectory(directory);
            var namespaceFile = Path.Combine(directory, this.Id.ToString().ToValidFilePath()) + ".md";
            using (StreamWriter writer = new StreamWriter(namespaceFile))
            {
                this.WriteMarkdownSkeleton(writer);
            }

            foreach (var @class in Classes)
            {
                var classFile = Path.Combine(directory, @class.Id.ToString().ToValidFilePath()) + ".md";
                using (StreamWriter writer = new StreamWriter(classFile))
                {
                    @class.WriteMarkdownSkeleton(writer);
                }
            }
        }
    }

    public class ClassDocMetadata : MemeberDocMetadata
    {
        private ConcurrentDictionary<Identity, MethodDocMetadata> _methods =
            new ConcurrentDictionary<Identity, MethodDocMetadata>();
        public IEnumerable<MethodDocMetadata> Methods { get { return _methods.Values; } }
        public Stack<Identity> InheritanceHierarchy { get; set; }

        public IReadOnlyList<EventDocMetadataDefinition> Events { get; set; }
        public IReadOnlyList<ConstructorDocMetadata> Constructors { get; set; }
        public IReadOnlyList<PropertyDocMetadata> Properties { get; set; }

        public ClassDocMetadata(Metadata mta) : base(mta)
        {
        }

        public ClassDocMetadata(string name) : base(name)
        {
        }

        public MethodDocMetadata CreateMethod(string name)
        {
            return _methods.GetOrAdd(name, s => new MethodDocMetadata(s)
            {
                MscorlibVersion = this.MscorlibVersion,
                Assembly = this.Assembly,
                Namespace = this.Namespace,
            });
        }

        public bool TryAddMethod(MethodDocMetadata mta)
        {
            return _methods.TryAdd(mta.Id, mta);
        }

        public override void WriteMarkdownSkeleton(TextWriter writer)
        {
            base.WriteMarkdownSkeleton(writer);

            // TODO: normalize other members into one collection
            foreach (var method in Methods)
            {
                method.WriteMarkdownSkeleton(writer);
            }
        }
    }

    public class MethodDocMetadata : MemeberDocMetadata
    {
        public MethodDocMetadata(string name) : base(name)
        {
        }

        public MethodDocMetadata(Metadata mta) : base(mta)
        {
        }
    }

    public class PropertyDocMetadata : MemeberDocMetadata
    {
        public PropertyDocMetadata(string name) : base(name)
        {
        }
    }

    public class EventDocMetadataDefinition : Metadata
    {
        public EventDocMetadataDefinition(string name) : base(name)
        {
        }
    }

    public class ConstructorDocMetadata : Metadata
    {
        public ConstructorDocMetadata(string name) : base(name)
        {
        }
    }

    public class InterfaceDocMetadata : MemeberDocMetadata
    {

        public InterfaceDocMetadata(string name) : base(name)
        {
        }
    }

    public class StructDocMetadata : Metadata
    {

        public StructDocMetadata(string name) : base(name)
        {
        }
    }

    public class DelegateDocMetadata : Metadata
    {

        public DelegateDocMetadata(string name) : base(name)
        {
        }
    }

    public class EnumDocMetadata : Metadata
    {

        public EnumDocMetadata(string name) : base(name)
        {
        }
    }

    public class Metadata
    {
        public Identity Id { get; set; }

        public MemberType MemberType { get; set; }

        public string XmlDocumentation { get; set; }

        public Version MscorlibVersion { get; set; }

        public Metadata(string name)
        {
            this.Id = new Identity(name);
        }

        /// <summary>
        /// Export Markdown file
        /// ```
        /// @M:SDSearchLib.Managers.ConsoleManager.ShowWindow(System.Boolean)
        /// ```
        /// </summary>
        /// <param name="stream"></param>
        public virtual void WriteMarkdownSkeleton(TextWriter writer)
        {
            writer.WriteLine("```");
            writer.WriteLine("@" + this.Id);
            writer.WriteLine("```");
        }

        /// <summary>
        /// Export metadata file
        /// </summary>
        /// <param name="stream"></param>
        public virtual void WriteMetadata(TextWriter writer)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            JsonSerializer serializer = new JsonSerializer();
            serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            serializer.Formatting = Formatting.Indented;
            serializer.Converters.Add(new IdentityJsonConverter());
            serializer.Serialize(writer, this);
        }
    }

    /// <summary>
    /// http://msdn.microsoft.com/zh-cn/library/system.exception.helplink(v=vs.110).aspx
    /// </summary>
    public class PropertySyntax : SyntaxDocFragment
    {
        public Identity PropertyType { get; set; }

        public IReadOnlyList<Identity> Implements { get; set; }
    }

    /// <summary>
    /// http://msdn.microsoft.com/zh-cn/library/tz6bzkbf(v=vs.110).aspx
    /// </summary>
    public class ConstructorSyntax : SyntaxDocFragment
    {
        public IReadOnlyDictionary<Identity, string> Parameters { get; set; }
    }

    /// <summary>
    /// http://msdn.microsoft.com/zh-cn/library/tz6bzkbf(v=vs.110).aspx
    /// </summary>
    public class MethodSyntax : SyntaxDocFragment
    {
        public IReadOnlyDictionary<Identity, string> Parameters { get; set; }
        public IReadOnlyList<Identity> Implements { get; set; }
    }

    public class SyntaxDocFragment
    {
        public string Content { get; set; }

        /// <summary>
        /// TODO: how to merge comments in different language
        /// </summary>
        public string Language { get; set; }

        public string XmlDocumentation { get; set; }
    }

    public class Identity
    {
        string _fullyQualifiedName;

        public Identity(string fullyQualifiedName)
        {
            _fullyQualifiedName = fullyQualifiedName;
        }

        public override string ToString()
        {
            return _fullyQualifiedName;
        }

        public static implicit operator string (Identity memberIdentity)
        {
            return memberIdentity.ToString();
        }

        public static implicit operator Identity(string name)
        {
            return new Identity(name);
        }
    }

    public enum MemberType
    {
        Assembly,
        Namespace,
        Class,
        Interface,
        Struct,
        Delegate,
        Enum
    }

    public class IdentityJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Identity))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}

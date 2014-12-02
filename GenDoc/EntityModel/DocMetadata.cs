using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace DocAsCode.EntityModel
{
    /// <summary>
    /// Doc metadata is per project
    /// </summary>
    public class AssemblyDocMetadata : DocMetadata
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
    }

    public class MemeberDocMetadata : DocMetadata
    {
        public Identity Namespace { get; set; }

        public Identity Assembly { get; set; }

        public SyntaxDocFragment Syntax { get; set; }

        public MemeberDocMetadata(string name) : base(name)
        {
        }

        public MemeberDocMetadata(DocMetadata mta) : base(mta.Id)
        {
            this.MscorlibVersion = mta.MscorlibVersion;
            this.XmlDocumentation = mta.XmlDocumentation;
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

        public NamespaceDocMetadata(DocMetadata mta) : base(mta)
        {
        }

        public bool TryAddClass(ClassDocMetadata classMetadata)
        {
            return _classes.TryAdd(classMetadata.Id, classMetadata);
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

        public ClassDocMetadata(DocMetadata mta) : base(mta)
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

        public MethodDocMetadata(DocMetadata mta) : base(mta)
        {
        }
    }

    public class PropertyDocMetadata : MemeberDocMetadata
    {
        public PropertyDocMetadata(string name) : base(name)
        {
        }
    }

    public class EventDocMetadataDefinition : DocMetadata
    {
        public EventDocMetadataDefinition(string name) : base(name)
        {
        }
    }

    public class ConstructorDocMetadata : DocMetadata
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

    public class StructDocMetadata : DocMetadata
    {

        public StructDocMetadata(string name) : base(name)
        {
        }
    }

    public class DelegateDocMetadata : DocMetadata
    {

        public DelegateDocMetadata(string name) : base(name)
        {
        }
    }

    public class EnumDocMetadata : DocMetadata
    {

        public EnumDocMetadata(string name) : base(name)
        {
        }
    }

    public class DocMetadata
    {
        public Identity Id { get; set; }

        public MemberType MemberType { get; set; }

        public string XmlDocumentation { get; set; }

        public Version MscorlibVersion { get; set; }

        public DocMetadata(string name)
        {
            this.Id = new Identity(name);
        }

        /// <summary>
        /// Export Markdown file
        /// ---
        /// M:SDSearchLib.Managers.ConsoleManager.ShowWindow(System.Boolean)
        /// ---
        /// </summary>
        /// <param name="stream"></param>
        public virtual void WriteMarkdownSkeleton(TextWriter writer)
        {
            writer.WriteLine("---");
            writer.WriteLine(ConvertCommentIdToYamlHeader(this.Id));
            writer.WriteLine("---");
        }

        private string ConvertCommentIdToYamlHeader(string commendId)
        {
            if (string.IsNullOrEmpty(commendId))
            {
                throw new ArgumentNullException("commendId");
            }

            return this.GetYamlHeaderPrefix(commendId) + ": " + commendId.Substring(2);
        }

        private string GetYamlHeaderPrefix(string commendId)
        {
            string prefix = commendId.Substring(0, 2);
            switch (prefix)
            {
                case "N:":
                    return "namespace";
                case "T:":
                    return "class";
                case "M:":
                    return "method";
                case "P:":
                    return "property";
                default:
                    return commendId.Substring(0, 1);
            }
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
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocAsCode.EntityModel
{
    /// <summary>
    /// Doc metadata is per project
    /// </summary>
    public class AssemblyDocMetadata : DocMetadata
    {
        private ConcurrentDictionary<Identity, NamespaceDocMetadata> _namespaces = new ConcurrentDictionary<Identity, NamespaceDocMetadata>();
        public IEnumerable<NamespaceDocMetadata> Namespaces
        {
            get { return _namespaces.Values; }
            set
            {
                _namespaces = new ConcurrentDictionary<Identity, NamespaceDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }

        public AssemblyDocMetadata()
        {
        }

        public AssemblyDocMetadata(string name) : base(name)
        {
        }

        public bool TryAddNamespace(NamespaceDocMetadata mta)
        {
            return _namespaces.TryAdd(mta.Id, mta);
        }

        /// <summary>
        /// TODO: implement this to return all the supported DocMetadata
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DocMetadata> GetChildrenDocMetadata()
        {
            throw new NotImplementedException();
            foreach(var ns in Namespaces)
            {
                yield return ns;
                // Namespaces' Children
            }
        }
    }

    public class MemeberDocMetadata : DocMetadata
    {
        public Identity Namespace { get; set; }

        public Identity Assembly { get; set; }

        public SyntaxDocFragment Syntax { get; set; }

        public MemeberDocMetadata() { }
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
        public IEnumerable<ClassDocMetadata> Classes
        {
            get { return _classes.Values; }
            set
            {
                _classes = new ConcurrentDictionary<Identity, ClassDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }

        public IReadOnlyList<InterfaceDocMetadata> Interfaces { get; set; }
        public IReadOnlyList<StructDocMetadata> Structs { get; set; }
        public IReadOnlyList<DelegateDocMetadata> Delegates { get; set; }
        public IReadOnlyList<EnumDocMetadata> Enums { get; set; }

        public NamespaceDocMetadata() { }

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
        public IEnumerable<MethodDocMetadata> Methods
        {
            get { return _methods.Values; }
            set
            {
                _methods = new ConcurrentDictionary<Identity, MethodDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }
        public Stack<Identity> InheritanceHierarchy { get; set; }

        public IReadOnlyList<EventDocMetadataDefinition> Events { get; set; }
        public IReadOnlyList<ConstructorDocMetadata> Constructors { get; set; }
        public IReadOnlyList<PropertyDocMetadata> Properties { get; set; }
        public ClassDocMetadata() { }

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
        public MethodDocMetadata() { }

        public MethodDocMetadata(string name) : base(name)
        {
        }

        public MethodDocMetadata(DocMetadata mta) : base(mta)
        {
        }
    }

    public class PropertyDocMetadata : MemeberDocMetadata
    {
        public PropertyDocMetadata() { }
        public PropertyDocMetadata(string name) : base(name)
        {
        }
    }

    public class EventDocMetadataDefinition : DocMetadata
    {
        public EventDocMetadataDefinition() { }
        public EventDocMetadataDefinition(string name) : base(name)
        {
        }
    }

    public class ConstructorDocMetadata : DocMetadata
    {
        public ConstructorDocMetadata() { }
        public ConstructorDocMetadata(string name) : base(name)
        {
        }
    }

    public class InterfaceDocMetadata : MemeberDocMetadata
    {

        public InterfaceDocMetadata() { }
        public InterfaceDocMetadata(string name) : base(name)
        {
        }
    }

    public class StructDocMetadata : DocMetadata
    {

        public StructDocMetadata() { }
        public StructDocMetadata(string name) : base(name)
        {
        }
    }

    public class DelegateDocMetadata : DocMetadata
    {

        public DelegateDocMetadata() { }
        public DelegateDocMetadata(string name) : base(name)
        {
        }
    }

    public class EnumDocMetadata : DocMetadata
    {

        public EnumDocMetadata() { }
        public EnumDocMetadata(string name) : base(name)
        {
        }
    }

    public class DocMetadata
    {
        public Identity Id { get; set; }

        public MemberType MemberType { get; set; }

        public string XmlDocumentation { get; set; }

        public string MarkdownContent { get; set; }

        public Version MscorlibVersion { get; set; }
        public DocMetadata()
        {
        }

        public DocMetadata(string name)
        {
            this.Id = new Identity(name);
        }

        /// <summary>
        /// TODO: use razor engine to generate html
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="additionalContent"></param>
        public virtual void WriteHtml(TextWriter writer, string additionalContent)
        {
            writer.WriteLine(string.Format("<h>{0}</h>", Id));
            writer.WriteLine(string.Format("<p>{0}<p>"), XmlDocumentation);
            writer.WriteLine(string.Format("<p>{0}<p>"), additionalContent);
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
            writer.WriteLine(ConvertCommentIdToYamlHeader(this.Id));
        }

        private string ConvertCommentIdToYamlHeader(string commendId)
        {
            if (string.IsNullOrEmpty(commendId))
            {
                throw new ArgumentNullException("commendId");
            }

            return (string)new CommentIdToYamlHeaderConverter().ConvertTo(commendId, typeof(string));
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

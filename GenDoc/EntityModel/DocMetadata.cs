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
        private  ConcurrentDictionary<Identity, ClassDocMetadata> _classes = new ConcurrentDictionary<Identity,ClassDocMetadata>();
        private  ConcurrentDictionary<Identity, InterfaceDocMetadata> _interfaces = new ConcurrentDictionary<Identity, InterfaceDocMetadata>();
        private ConcurrentDictionary<Identity, StructDocMetadata> _structs = new ConcurrentDictionary<Identity, StructDocMetadata>();
        private ConcurrentDictionary<Identity, DelegateDocMetadata> _delegates = new ConcurrentDictionary<Identity, DelegateDocMetadata>();
        private ConcurrentDictionary<Identity, EnumDocMetadata> _enums = new ConcurrentDictionary<Identity, EnumDocMetadata>();

        private Dictionary<MemberType, ConcurrentDictionary<Identity, DocMetadata>> _members =
            new Dictionary<MemberType, ConcurrentDictionary<Identity, DocMetadata>>()
            {
                { MemberType.Class, new ConcurrentDictionary<Identity,DocMetadata>() },
                { MemberType.Interface, new ConcurrentDictionary<Identity, DocMetadata>()},
                { MemberType.Struct, new ConcurrentDictionary<Identity, DocMetadata>()},
                { MemberType.Delegate, new ConcurrentDictionary<Identity, DocMetadata>()},
                { MemberType.Enum, new ConcurrentDictionary<Identity, DocMetadata>()}
            };

        public IEnumerable<DocMetadata> Members
        {
            get
            {
                List<DocMetadata> list = new List<DocMetadata>();
                foreach(var member in _members.Values)
                {
                    foreach (var value in member.Values)
                    {
                        list.Add(value);
                    }
                }
               
                return list;
            }
        }

        public IEnumerable<ClassDocMetadata> Classes
        {
            get {
                //return _classes.Values;
                List<ClassDocMetadata> list = new List<ClassDocMetadata>();
                foreach (var value in _members[MemberType.Class].Values)
                {
                    list.Add((ClassDocMetadata)value);
                }
                return list;
            }
            set
            {
                _classes = new ConcurrentDictionary<Identity, ClassDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }

        public IEnumerable<InterfaceDocMetadata> Interfaces
        {
            get { return _interfaces.Values; }
            set
            {
                _interfaces = new ConcurrentDictionary<Identity, InterfaceDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }

        public IEnumerable<StructDocMetadata> Structs
        {
            get { return _structs.Values; }
            set
            {
                _structs = new ConcurrentDictionary<Identity, StructDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }

        public IEnumerable<DelegateDocMetadata> Delegates
        {
            get { return _delegates.Values; }
            set
            {
                _delegates = new ConcurrentDictionary<Identity, DelegateDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }

        public IEnumerable<EnumDocMetadata> Enums
        {
            get { return _enums.Values; }
            set
            {
                _enums = new ConcurrentDictionary<Identity, EnumDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }

        public NamespaceDocMetadata() { }

        public NamespaceDocMetadata(string name) : base(name)
        {
        }

        public NamespaceDocMetadata(DocMetadata mta) : base(mta)
        {
        }

        public bool TryAddClass(ClassDocMetadata classMetadata)
        {
            //return _classes.TryAdd(classMetadata.Id, classMetadata);
            return _members[MemberType.Class].TryAdd(classMetadata.Id, classMetadata);
        }

        public bool TryAddInterface(InterfaceDocMetadata interfaceMetadata)
        {
            return _members[MemberType.Interface].TryAdd(interfaceMetadata.Id, interfaceMetadata);
        }
        public bool TryAddDelegate(DelegateDocMetadata delegateMetadata)
        {
            return _members[MemberType.Delegate].TryAdd(delegateMetadata.Id, delegateMetadata);
        }
        public bool TryAddStruct(StructDocMetadata structMetadata)
        {
            return _members[MemberType.Struct].TryAdd(structMetadata.Id, structMetadata);
        }
        public bool TryAddEnum(EnumDocMetadata enumMetadata)
        {
            _members[MemberType.Enum].TryAdd(enumMetadata.Id, enumMetadata);
            return _members[MemberType.Enum].TryAdd(enumMetadata.Id, enumMetadata);
        }
    }

    public class ClassDocMetadata : MemeberDocMetadata
    {
        private ConcurrentDictionary<Identity, MethodDocMetadata> _methods = new ConcurrentDictionary<Identity, MethodDocMetadata>();
        private ConcurrentDictionary<Identity, EventDocMetadataDefinition> _events = new ConcurrentDictionary<Identity, EventDocMetadataDefinition>();
        private ConcurrentDictionary<Identity, ConstructorDocMetadata> _constructs = new ConcurrentDictionary<Identity, ConstructorDocMetadata>();
        private ConcurrentDictionary<Identity, PropertyDocMetadata> _properties = new ConcurrentDictionary<Identity, PropertyDocMetadata>();
        private ConcurrentDictionary<Identity, FieldDocMetadata> _fields = new ConcurrentDictionary<Identity, FieldDocMetadata>();

        public IEnumerable<DocMetadata> ClassMembers
        {
            get
            {
                List<DocMetadata> list = new List<DocMetadata>();
                foreach (var item in _methods.Values)
                {
                    list.Add(item);
                }
                foreach (var item in _events.Values)
                {
                    list.Add(item);
                }
                foreach (var item in _constructs.Values)
                {
                    list.Add(item);
                }
                foreach (var item in _properties.Values)
                {
                    list.Add(item);
                }
                foreach (var item in _fields.Values)
                {
                    list.Add(item);
                }
                return list;
            }
        }

        public IEnumerable<MethodDocMetadata> Methods
        {
            get { return _methods.Values; }
            set
            {
                _methods = new ConcurrentDictionary<Identity, MethodDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }
        public IEnumerable<EventDocMetadataDefinition> Events
        {
            get { return _events.Values; }
            set
            {
                _events = new ConcurrentDictionary<Identity, EventDocMetadataDefinition>(value.ToDictionary(s => s.Id, s => s));
            }
        }
        public IEnumerable<ConstructorDocMetadata> Constructs
        {
            get { return _constructs.Values; }
            set
            {
                _constructs = new ConcurrentDictionary<Identity, ConstructorDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }
        public IEnumerable<PropertyDocMetadata> Properties
        {
            get { return _properties.Values; }
            set
            {
                _properties = new ConcurrentDictionary<Identity, PropertyDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }

        public IEnumerable<FieldDocMetadata> Fields
        {
            get { return _fields.Values; }
            set
            {
                _fields = new ConcurrentDictionary<Identity, FieldDocMetadata>(value.ToDictionary(s => s.Id, s => s));
            }
        }

        public Stack<Identity> InheritanceHierarchy { get; set; }

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
        public bool TryAddEvent(EventDocMetadataDefinition mta)
        {
            return _events.TryAdd(mta.Id, mta);
        }
        public bool TryAddConstruct(ConstructorDocMetadata mta)
        {
            return _constructs.TryAdd(mta.Id, mta);
        }
        public bool TryAddProperty(PropertyDocMetadata mta)
        {
            return _properties.TryAdd(mta.Id, mta);
        }
        public bool TryAddField(FieldDocMetadata mta)
        {
            return _fields.TryAdd(mta.Id, mta);
        }

        public override void WriteMarkdownSkeleton(TextWriter writer)
        {
            base.WriteMarkdownSkeleton(writer);

            // TODO: normalize other members into one collection
            foreach (var member in ClassMembers)
            {
                member.WriteMarkdownSkeleton(writer);
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
        public PropertyDocMetadata(DocMetadata mta) : base(mta)
        {
        }
    }

    public class FieldDocMetadata : MemeberDocMetadata
    {
        public FieldDocMetadata() { }
        public FieldDocMetadata(string name) : base(name)
        {
        }
        public FieldDocMetadata(DocMetadata mta) : base(mta)
        {
        }
    }

    public class EventDocMetadataDefinition : MemeberDocMetadata
    {
        public EventDocMetadataDefinition() { }
        public EventDocMetadataDefinition(string name) : base(name)
        {
        }
        public EventDocMetadataDefinition(DocMetadata mta) : base(mta)
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
        public InterfaceDocMetadata(DocMetadata mta) : base(mta)
        {
        }
    }

    public class StructDocMetadata : MemeberDocMetadata
    {

        public StructDocMetadata() { }
        public StructDocMetadata(string name) : base(name)
        {
        }
        public StructDocMetadata(DocMetadata mta) : base(mta)
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

    public class FieldSyntax : SyntaxDocFragment
    {
        public Identity FieldType { get; set; }
    }
    public class EventSyntax : SyntaxDocFragment
    {
        public Identity EventType { get; set; }
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

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

    public class MemberDocMetadata : DocMetadata
    {
        public Identity Namespace { get; set; }

        public Identity Assembly { get; set; }

        public SyntaxDocFragment Syntax { get; set; }

        public MemberDocMetadata() { }
        public MemberDocMetadata(string name) : base(name)
        {
        }

        public MemberDocMetadata(DocMetadata mta) : base(mta.Id)
        {
            this.MscorlibVersion = mta.MscorlibVersion;
            this.XmlDocumentation = mta.XmlDocumentation;
        }
    }

    public class CompositeDocMetadata : MemberDocMetadata
    {
        private Dictionary<MemberType, ConcurrentDictionary<Identity, DocMetadata>> _members =
            new Dictionary<MemberType, ConcurrentDictionary<Identity, DocMetadata>>();

        protected List<MemberType> AllowedMemberTypes { get; set; }

      //  [JsonIgnore]
        public IEnumerable<DocMetadata> Members
        {
            get
            {
                List<DocMetadata> list = new List<DocMetadata>();
                for (int i = 0; i < AllowedMemberTypes.Count; i++)
                {
                    var type = AllowedMemberTypes[i];
                    if (_members.ContainsKey(type))
                    {
                        foreach (var value in _members[type].Values)
                        {
                            list.Add(value);
                        }
                    }
                }
                return list;
            }
        }

        public Dictionary<MemberType, ConcurrentDictionary<Identity, DocMetadata>> MemberDict
        {
            get { return _members; }
            set
            {
                _members = value;
            }
        }

        public bool TryAdd(MemberDocMetadata metadata, MemberType type)
        {
            if (!AllowedMemberTypes.Contains(type))
            {
                throw new Exception(string.Format("The metadata of type {0} cannot be generated.", type.ToString()));
            }

          //  var dict = _members.GetOrAdd(type, new ConcurrentDictionary<Identity, DocMetadata>());

            return _members[type].TryAdd(metadata.Id, metadata);
        }
         
        public IEnumerable<DocMetadata> GetMemberType(MemberType type)
        {
            if (!AllowedMemberTypes.Contains(type))
            {
                throw new Exception(string.Format("The metadata of type {0} is not allowed.", type.ToString()));
            }
            if (_members.ContainsKey(type))
            {
                return _members[type].Values;
            }
            else
            {
                return new List<DocMetadata>();
            }
        }

        protected void Init()
        {
            if (AllowedMemberTypes != null)
            {
                foreach(var i in AllowedMemberTypes)
                {
                    _members.Add(i, new ConcurrentDictionary<Identity, DocMetadata>());
                }
            }
        }
        public CompositeDocMetadata() {
        }

        public CompositeDocMetadata(string name) : base(name)
        {
        }

        public CompositeDocMetadata(DocMetadata mta) : base(mta)
        {
        }

        public override void WriteMarkdownSkeleton(TextWriter writer)
        {
            base.WriteMarkdownSkeleton(writer);

            foreach (var member in Members)
            {
                member.WriteMarkdownSkeleton(writer);
            }
        }
    }

    /// <summary>
    /// TODO: what if no namespace?
    /// </summary>
    public class NamespaceDocMetadata : CompositeDocMetadata
    {
        private ConcurrentDictionary<MemberType, ConcurrentDictionary<Identity, DocMetadata>> _members =
            new ConcurrentDictionary<MemberType, ConcurrentDictionary<Identity, DocMetadata>>();

        public NamespaceDocMetadata()
        {
            AllowedMemberTypes = new List<MemberType>()
            {
                MemberType.Class,
                MemberType.Interface,
                MemberType.Struct,
                MemberType.Delegate,
                MemberType.Enum
            };

           base.Init();
        }

        public NamespaceDocMetadata(string name) : base(name)
        {
            AllowedMemberTypes = new List<MemberType>()
            {
                MemberType.Class,
                MemberType.Interface,
                MemberType.Struct,
                MemberType.Delegate,
                MemberType.Enum
            };
            base.Init();
        }

        public NamespaceDocMetadata(DocMetadata mta) : base(mta)
        {
            AllowedMemberTypes = new List<MemberType>()
            {
                MemberType.Class,
                MemberType.Interface,
                MemberType.Struct,
                MemberType.Delegate,
                MemberType.Enum
            };
            base.Init();
        }
    }

    public class ClassDocMetadata : CompositeDocMetadata
    {
        public Stack<Identity> InheritanceHierarchy { get; set; }

        public ClassDocMetadata()
        {
            AllowedMemberTypes = new List<MemberType>
            {
                MemberType.Field,
                MemberType.Property,
                MemberType.Event,
                MemberType.Constructor,
                MemberType.Method,
            };
            base.Init();
        }

        public ClassDocMetadata(string name) : base(name)
        {
            AllowedMemberTypes = new List<MemberType>
            {
                MemberType.Field,
                MemberType.Property,
                MemberType.Event,
                MemberType.Constructor,
                MemberType.Method,
            };
            base.Init();
        }

        public ClassDocMetadata(DocMetadata mta) : base(mta)
        {
            AllowedMemberTypes = new List<MemberType>
            {
                MemberType.Field,
                MemberType.Property,
                MemberType.Event,
                MemberType.Constructor,
                MemberType.Method,
            };
            base.Init();
        }

        /*public MethodDocMetadata CreateMethod(string name)
        {
            return _members[SubMemberType.Method].GetOrAdd(name, s => new DocMetadata(s)
            {
                MscorlibVersion = this.MscorlibVersion,
                Assembly = this.Assembly,
                Namespace = this.Namespace,
            });
        }*/
    }

    public class MethodDocMetadata : MemberDocMetadata
    {
        public MethodDocMetadata() { }

        public MethodDocMetadata(string name) : base(name)
        {
        }

        public MethodDocMetadata(DocMetadata mta) : base(mta)
        {
        }
    }
    public class ConstructorDocMetadata : MemberDocMetadata
    {
        public ConstructorDocMetadata() { }

        public ConstructorDocMetadata(string name) : base(name)
        {
        }

        public ConstructorDocMetadata(DocMetadata mta) : base(mta)
        {
        }
    }

    public class PropertyDocMetadata : MemberDocMetadata
    {
        public PropertyDocMetadata() { }
        public PropertyDocMetadata(string name) : base(name)
        {
        }
        public PropertyDocMetadata(DocMetadata mta) : base(mta)
        {
        }
    }

    public class FieldDocMetadata : MemberDocMetadata
    {
        public FieldDocMetadata() { }
        public FieldDocMetadata(string name) : base(name)
        {
        }
        public FieldDocMetadata(DocMetadata mta) : base(mta)
        {
        }
    }

    public class EventDocMetadataDefinition : MemberDocMetadata
    {
        public EventDocMetadataDefinition() { }
        public EventDocMetadataDefinition(string name) : base(name)
        {
        }
        public EventDocMetadataDefinition(DocMetadata mta) : base(mta)
        {
        }
    }

    public class InterfaceDocMetadata : CompositeDocMetadata
    {
        public InterfaceDocMetadata()
        {
            AllowedMemberTypes = new List<MemberType>
            {
                MemberType.Property,
                MemberType.Event,
                MemberType.Method,
            };
            base.Init();
        }
        public InterfaceDocMetadata(string name) : base(name)
        {
            AllowedMemberTypes = new List<MemberType>
            {
                MemberType.Property,
                MemberType.Event,
                MemberType.Method,
            };
            base.Init();
        }
        public InterfaceDocMetadata(DocMetadata mta) : base(mta)
        {
            AllowedMemberTypes = new List<MemberType>
            {
                MemberType.Property,
                MemberType.Event,
                MemberType.Method,
            };
            base.Init();
        }
    }

    public class StructDocMetadata : CompositeDocMetadata
    {
        public StructDocMetadata()
        {
            AllowedMemberTypes = new List<MemberType>
            {
                MemberType.Field,
                MemberType.Property,
                MemberType.Event,
                MemberType.Constructor,
                MemberType.Method,
            };
            base.Init();
        }
        public StructDocMetadata(string name) : base(name)
        {
            AllowedMemberTypes = new List<MemberType>
            {
                MemberType.Field,
                MemberType.Property,
                MemberType.Event,
                MemberType.Constructor,
                MemberType.Method,
            };
            base.Init();
        }
        public StructDocMetadata(DocMetadata mta) : base(mta)
        {
            AllowedMemberTypes = new List<MemberType>
            {
                MemberType.Field,
                MemberType.Property,
                MemberType.Event,
                MemberType.Constructor,
                MemberType.Method,
            };
            base.Init();
        }
    }

    public class DelegateDocMetadata : MemberDocMetadata
    {

        public DelegateDocMetadata() { }
        public DelegateDocMetadata(string name) : base(name)
        {
        }
        public DelegateDocMetadata(DocMetadata mta) : base(mta)
        {
        }
    }

    public class EnumDocMetadata : MemberDocMetadata
    {

        public EnumDocMetadata() { }
        public EnumDocMetadata(string name) : base(name)
        {
        }
        public EnumDocMetadata(DocMetadata mta) : base(mta)
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
        Enum,
        Field,
        Property,
        Event,
        Constructor,
        Method,
    }
}

namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.Build.ReflectionXmlSyntax;
    using Microsoft.Content.BuildEngine.Constants;
    using Microsoft.Content.BuildEngine.DataAccessor.Extensions;
    using Microsoft.Content.BuildEngine.DataAccessorInterface;
    using Microsoft.Content.BuildEngine.MRef.Caching;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal sealed class HierarchyComparerForInitializing
        : HierarchyComparer
    {
        private readonly string m_currentVersion;

        public HierarchyComparerForInitializing(IBlobAccessor accessor, string currentVersion)
            : base(accessor)
        {
            m_currentVersion = currentVersion;
        }

        public override IEnumerable<HierarchyChanges> ScanHierarchy(IdMap map)
        {
            return GetNamespaceAndGroupsChanges().Concat(GetChanges());
        }

        private IEnumerable<HierarchyChanges> GetNamespaceAndGroupsChanges()
        {
            XDocument doc = LoadXDocument(MRefConstants.ReflectionFileName_Namespaces);
            return from parent in doc.Root.Element("apis").Elements("api")
                   let parentType = GetHierarchyTypeFromApiElement(parent)
                   where parentType == HierarchyType.RootNamespace ||
                         (parentType == HierarchyType.NamespaceGroup &&
                          parent.Element("elements").Elements("element").Skip(1).Any()) // ignore group only contains one ns
                   from item in parent.Element("elements").Elements("element")
                   let internalName = (string)item.Attribute("api")
                   join itemDef in doc.Root.Element("apis").Elements("api")
                   on internalName equals (string)itemDef.Attribute("id")
                   let type = GetHierarchyTypeFromApiElement(itemDef)
                   orderby parentType
                   select new HierarchyChanges
                   {
                       ChangeType = HierarchyChangeType.Add,
                       InternalName = internalName,
                       NewParent = (parentType == HierarchyType.RootNamespace) ? (string)null : (string)parent.Attribute("id"),
                       Type = type,
                       File = MRefConstants.ReflectionFileName_Namespaces,
                   };
        }

        private XDocument LoadXDocument(string filename)
        {
            using (var stream = m_accessor.OpenRead(m_currentVersion, BlobFileType.IntergratedReflectionXml, filename))
            {
                return XDocument.Load(stream);
            }
        }

        private IEnumerable<HierarchyChanges> GetChanges()
        {
            return from file in m_accessor.EnumerateFiles(m_currentVersion, BlobFileType.IntergratedReflectionXml)
                   where !string.Equals(file, MRefConstants.ReflectionFileName_Namespaces, StringComparison.CurrentCultureIgnoreCase)
                   from changes in GetChanges(file)
                   from change in changes
                   select change;
        }

        private IEnumerable<IEnumerable<HierarchyChanges>> GetChanges(string file)
        {
            XDocument doc = LoadXDocument(file);
            var assemblyName = (string)doc.Root.Element("assemblies").Element("assembly").Attribute("name");
            // ns -> type
            yield return from item in doc.Root.Element("apis").Elements("api")
                         let type = GetHierarchyTypeFromApiElement(item)
                         where HierarchyTypeHelper.IsType(type)
                         select new HierarchyChanges
                         {
                             ChangeType = HierarchyChangeType.Add,
                             Type = type,
                             InternalName = (string)item.Attribute("id"),
                             Assembly = assemblyName,
                             File = file,
                             NewParent = (string)item.Element("containers").Element("namespace").Attribute("api"),
                         };
            // type -> ctor group or ctor
            yield return from parent in doc.Root.Element("apis").Elements("api")
                         let parentType = GetHierarchyTypeFromApiElement(parent)
                         where HierarchyTypeHelper.IsType(parentType) &&
                             parentType != HierarchyType.Delegate && // delegate do not has member
                             parentType != HierarchyType.Enumeration && // enumeration add field directly
                             parent.Element("elements") != null
                         from item in parent.Element("elements").Elements("element")
                         let internalName = (string)item.Attribute("api")
                         join itemDef in doc.Root.Element("apis").Elements("api")
                         on internalName equals (string)itemDef.Attribute("id")
                         let type = GetHierarchyTypeFromApiElement(itemDef)
                         where (type == HierarchyType.Constructors) ||
                            (type == HierarchyType.Constructor && itemDef.Element("memberdata").Attribute("overload") == null)
                         select new HierarchyChanges
                         {
                             ChangeType = HierarchyChangeType.Add,
                             Type = type,
                             InternalName = internalName,
                             Assembly = assemblyName,
                             File = file,
                             NewParent = (string)parent.Attribute("id"),
                         };
            // type -> member group (exclude overloads, ctors)
            yield return from parent in doc.Root.Element("apis").Elements("api")
                         let parentType = GetHierarchyTypeFromApiElement(parent)
                         where HierarchyTypeHelper.IsType(parentType) &&
                             parentType != HierarchyType.Delegate && // delegate do not has member
                             parentType != HierarchyType.Enumeration && // enumeration add field directly
                             parent.Element("elements") != null
                         join itemDef in
                             (from api in doc.Root.Element("apis").Elements("api")
                              where api.Element("topicdata").Attribute("typeTopicId") != null
                              select api)
                         on (string)parent.Attribute("id") equals (string)itemDef.Element("topicdata").Attribute("typeTopicId")
                         let type = GetHierarchyTypeFromApiElement(itemDef)
                         where HierarchyTypeHelper.IsRootMemberGroup(type)
                         select new HierarchyChanges
                         {
                             ChangeType = HierarchyChangeType.Add,
                             Type = type,
                             InternalName = (string)itemDef.Attribute("id"),
                             Assembly = assemblyName,
                             File = file,
                             NewParent = (string)parent.Attribute("id"),
                         };
            // member group -> member or overloads
            yield return from parent in doc.Root.Element("apis").Elements("api")
                         let parentType = GetHierarchyTypeFromApiElement(parent)
                         where HierarchyTypeHelper.IsMemberGroup(parentType) &&
                               parent.Element("elements") != null
                         from item in parent.Element("elements").Elements("element")
                         let internalName = (string)item.Attribute("api")
                         join itemDef in doc.Root.Element("apis").Elements("api")
                         on internalName equals (string)itemDef.Attribute("id")
                         let type = GetHierarchyTypeFromApiElement(itemDef)
                         where (parentType == HierarchyType.Events ||
                                parentType == HierarchyType.Fields ||
                                parentType == HierarchyType.Methods ||
                                parentType == HierarchyType.Operators ||
                                parentType == HierarchyType.Properties ||
                                parentType == HierarchyType.AttachedEvents ||
                                parentType == HierarchyType.AttachedProperties)
                               ?
                               (string)parent.Element("topicdata").Attributes("typeTopicId").FirstOrDefault() == (string)itemDef.Element("containers").Elements("type").Attributes("api").FirstOrDefault()
                               :
                               (string)parent.Element("containers").Elements("type").Attributes("api").FirstOrDefault() == (string)itemDef.Element("containers").Elements("type").Attributes("api").FirstOrDefault()
                         select new HierarchyChanges
                         {
                             ChangeType = HierarchyChangeType.Add,
                             Type = type,
                             InternalName = internalName,
                             Assembly = assemblyName,
                             File = file,
                             NewParent = (string)parent.Attribute("id"),
                         };
            // notopic: enum field
            yield return from api in doc.Root.Element("apis").Elements("api")
                         where api.Element("topicdata").Attribute("notopic") != null
                         select new HierarchyChanges
                         {
                             ChangeType = HierarchyChangeType.None,
                             Type = HierarchyType.Field,
                             InternalName = (string)api.Attribute("id"),
                             Assembly = assemblyName,
                             File = file,
                             NewParent = (string)api.Element("containers").Element("type").Attribute("api")
                         };
        }

        private static HierarchyType GetHierarchyTypeFromApiElement(XElement itemDef)
        {
            var result = HierarchyTypeHelper.GetHierarchyType(
                (string)itemDef.Element("apidata").Attribute("group"),
                (string)itemDef.Element("apidata").Attribute("subgroup"),
                (string)itemDef.Element("topicdata").Attribute("group"),
                (string)itemDef.Element("topicdata").Attribute("subgroup"),
                (string)itemDef.Element("topicdata").Attribute("memberSubgroup"));
            if (result == HierarchyType.Method &&
                (string)itemDef.Element("apidata").Attribute("subsubgroup") == "operator")
            {
                result = HierarchyType.Operator;
            }
            if (result == HierarchyType.Property &&
                (string)itemDef.Element("apidata").Attribute("subsubgroup") == "attachedProperty")
            {
                result = HierarchyType.AttachedProperty;
            }
            if (result == HierarchyType.Event &&
                (string)itemDef.Element("apidata").Attribute("subsubgroup") == "attachedEvent")
            {
                result = HierarchyType.AttachedEvent;
            }
            if (result == HierarchyType.MethodOverloads &&
                (string)itemDef.Element("apidata").Attribute("subsubgroup") == "operator")
            {
                result = HierarchyType.OperatorOverloads;
            }
            return result;
        }

    }
}

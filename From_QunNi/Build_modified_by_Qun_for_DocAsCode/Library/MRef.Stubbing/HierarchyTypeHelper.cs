namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class HierarchyTypeHelper
    {
        private static readonly Dictionary<Tuple<string, string, string, string, string>, HierarchyType> s_dict =
            new Dictionary<Tuple<string, string, string, string, string>, HierarchyType>
            {
                { Tuple.Create("namespace", (string)null, "api", (string)null, (string)null), HierarchyType.Namespace },
                { Tuple.Create("topNamespaceGroup", "namespaces", "list","namespaces",(string)null), HierarchyType.RootNamespace },
                { Tuple.Create("namespaceGroup", "namespaces", "list","namespaces",(string)null), HierarchyType.NamespaceGroup },

                { Tuple.Create("type", "class", "api", (string)null,(string)null), HierarchyType.Class },
                { Tuple.Create("type", "structure", "api", (string)null,(string)null), HierarchyType.Structure },
                { Tuple.Create("type", "interface", "api", (string)null,(string)null), HierarchyType.Interface },
                { Tuple.Create("type", "enumeration", "api", (string)null,(string)null), HierarchyType.Enumeration },
                { Tuple.Create("type", "delegate","api", (string)null, (string)null), HierarchyType.Delegate },

                //{ Tuple.Create("list", "members","api", (string)null, (string)null), HierarchyType.AllMembers },

                { Tuple.Create("type", "class", "list", "Methods", (string)null), HierarchyType.Methods },
                { Tuple.Create("type", "structure", "list", "Methods", (string)null), HierarchyType.Methods },
                { Tuple.Create("type", "interface", "list", "Methods", (string)null), HierarchyType.Methods },
                { Tuple.Create("type", "enumeration", "list", "Methods", (string)null), HierarchyType.Methods },
                { Tuple.Create("type", "delegate", "list", "Methods", (string)null), HierarchyType.Methods },

                { Tuple.Create("type", "class", "list", "Properties", (string)null), HierarchyType.Properties },
                { Tuple.Create("type", "structure", "list", "Properties", (string)null), HierarchyType.Properties },
                { Tuple.Create("type", "interface", "list", "Properties", (string)null), HierarchyType.Properties },
                { Tuple.Create("type", "enumeration", "list", "Properties", (string)null), HierarchyType.Properties },
                { Tuple.Create("type", "delegate", "list", "Properties", (string)null), HierarchyType.Properties },

                { Tuple.Create("type", "class", "list", "Fields", (string)null), HierarchyType.Fields },
                { Tuple.Create("type", "structure", "list", "Fields", (string)null), HierarchyType.Fields },
                { Tuple.Create("type", "interface", "list", "Fields", (string)null), HierarchyType.Fields },
                { Tuple.Create("type", "enumeration", "list", "Fields", (string)null), HierarchyType.Fields },
                { Tuple.Create("type", "delegate", "list", "Fields", (string)null), HierarchyType.Fields },

                { Tuple.Create("type", "class","list", "Events", (string)null), HierarchyType.Events },
                { Tuple.Create("type", "structure","list", "Events", (string)null), HierarchyType.Events },
                { Tuple.Create("type", "interface","list", "Events", (string)null), HierarchyType.Events },
                { Tuple.Create("type", "enumeration","list", "Events", (string)null), HierarchyType.Events },
                { Tuple.Create("type", "delegate","list", "Events", (string)null), HierarchyType.Events },

                { Tuple.Create("type", "class","list", "Operators", (string)null), HierarchyType.Operators },
                { Tuple.Create("type", "structure","list", "Operators", (string)null), HierarchyType.Operators },
                { Tuple.Create("type", "interface","list", "Operators", (string)null), HierarchyType.Operators },
                { Tuple.Create("type", "enumeration","list", "Operators", (string)null), HierarchyType.Operators },
                { Tuple.Create("type", "delegate","list", "Operators", (string)null), HierarchyType.Operators },

                { Tuple.Create("member", "constructor", "list", "overload", "constructor"), HierarchyType.Constructors },
                { Tuple.Create("member", "method", "list", "overload", "method"), HierarchyType.MethodOverloads },
                { Tuple.Create("member", "property", "list", "overload", "property"), HierarchyType.PropertyOverloads },

                { Tuple.Create("member", "constructor", "api", (string)null, (string)null), HierarchyType.Constructor },
                { Tuple.Create("member", "method", "api", (string)null, (string)null), HierarchyType.Method },
                { Tuple.Create("member", "method", "api", (string)null, "extension"), HierarchyType.ExtensionMethod },
                { Tuple.Create("member", "property", "api", (string)null, (string)null), HierarchyType.Property },
                { Tuple.Create("member", "field", "api", (string)null, (string)null), HierarchyType.Field },
                { Tuple.Create("member", "event", "api", (string)null, (string)null), HierarchyType.Event },

                { Tuple.Create("type", "class", "list", "AttachedEvents", (string)null), HierarchyType.AttachedEvents },
                { Tuple.Create("type", "structure", "list", "AttachedEvents", (string)null), HierarchyType.AttachedEvents },
                { Tuple.Create("type", "interface", "list", "AttachedEvents", (string)null), HierarchyType.AttachedEvents },
                { Tuple.Create("type", "enumeration", "list", "AttachedEvents", (string)null), HierarchyType.AttachedEvents },
                { Tuple.Create("type", "delegate", "list", "AttachedEvents", (string)null), HierarchyType.AttachedEvents },

                { Tuple.Create("type", "class", "list", "AttachedProperties", (string)null), HierarchyType.AttachedProperties },
                { Tuple.Create("type", "structure", "list", "AttachedProperties", (string)null), HierarchyType.AttachedProperties },
                { Tuple.Create("type", "interface", "list", "AttachedProperties", (string)null), HierarchyType.AttachedProperties },
                { Tuple.Create("type", "enumeration", "list", "AttachedProperties", (string)null), HierarchyType.AttachedProperties },
                { Tuple.Create("type", "delegate", "list", "AttachedProperties", (string)null), HierarchyType.AttachedProperties },

                // todo : add more case
            };

        public static HierarchyType GetHierarchyType(
            string group,
            string subgroup,
            string topicGroup,
            string topicSubGroup,
            string memberSubgroup)
        {
            HierarchyType result;
            if (s_dict.TryGetValue(Tuple.Create(group, subgroup, topicGroup, topicSubGroup, memberSubgroup), out result))
                return result;
            Debug.Fail("Unknown hierarchy type!");
            throw new ApplicationException("Unknown hierarchy type!");
        }

        public static bool IsGroup(HierarchyType type)
        {
            return (type & HierarchyType.GroupNode) == HierarchyType.GroupNode;
        }

        public static bool IsType(HierarchyType type)
        {
            return (type & HierarchyType.TypeNode) == HierarchyType.TypeNode;
        }

        public static bool IsMemberGroup(HierarchyType type)
        {
            return (type & HierarchyType.MemberGroupNode) == HierarchyType.MemberGroupNode;
        }

        public static bool IsRootMemberGroup(HierarchyType type)
        {
            return (type & HierarchyType.TypeMemberGroupNode) == HierarchyType.TypeMemberGroupNode;
        }

        public static bool IsMemberSubgroup(HierarchyType type)
        {
            return (type & HierarchyType.MemberSubgroupNode) == HierarchyType.MemberSubgroupNode;
        }

        public static bool IsMember(HierarchyType type)
        {
            return (type & HierarchyType.MemberNode) == HierarchyType.MemberNode;
        }

        public static string GetDdueTemplateName(HierarchyType type)
        {
            switch (type)
            {
                case HierarchyType.NamespaceGroup:
                    return "Namespace group";
                case HierarchyType.Namespace:
                    return "Namespace";
                case HierarchyType.Class:
                    return "Class";
                case HierarchyType.Structure:
                    return "Structure";
                case HierarchyType.Enumeration:
                    return "Enumeration";
                case HierarchyType.Delegate:
                    return "Delegate";
                case HierarchyType.Interface:
                    return "Interface";
                case HierarchyType.Constructors:
                    return "Constructors";
                case HierarchyType.Fields:
                    return "Fields";
                case HierarchyType.Methods:
                    return "Methods";
                case HierarchyType.Properties:
                    return "Properties";
                case HierarchyType.Events:
                    return "Events";
                case HierarchyType.AttachedProperties:
                    return "Attached Properties";
                case HierarchyType.AttachedEvents:
                    return "Attached Events";
                case HierarchyType.Operators:
                    return "Operators & Type Conversions";
                case HierarchyType.MethodOverloads:
                    return "Method Overloads";
                case HierarchyType.OperatorOverloads:
                    return "Operator Overloads";
                case HierarchyType.PropertyOverloads:
                    return "Property Overloads";
                case HierarchyType.Field:
                    return "Field";
                case HierarchyType.Constructor:
                    return "Constructor";
                case HierarchyType.Method:
                    return "Method";
                case HierarchyType.Property:
                    return "Property";
                case HierarchyType.Event:
                    return "Event";
                case HierarchyType.Operator:
                    return "Operator";
                case HierarchyType.AttachedProperty:
                    return "Attached Property";
                case HierarchyType.AttachedEvent:
                    return "Attached Event";
                case HierarchyType.RootNamespace:
                case HierarchyType.AllMembers:
                    // todo : not supported in Dec Release!
                    return null;
                default:
                    return null;
            }
        }

        public static int ToLevel(this HierarchyType type)
        {
            return (int)type;
            //switch (type)
            //{
            //    case HierarchyType.RootNamespace:
            //        return 1;
            //    case HierarchyType.NamespaceGroup:
            //    case HierarchyType.Namespace:
            //        return 2;
            //    case HierarchyType.Class:
            //    case HierarchyType.Structure:
            //    case HierarchyType.Enumeration:
            //    case HierarchyType.Delegate:
            //    case HierarchyType.Interface:
            //        return 3;
            //    case HierarchyType.AllMembers:
            //        return 4;
            //    case HierarchyType.Constructors:
            //        return 5;
            //    case HierarchyType.Constructor:
            //        return 6;
            //    case HierarchyType.Fields:
            //        return 7;
            //    case HierarchyType.Methods:
            //        return 8;
            //    case HierarchyType.Properties:
            //        return 9;
            //    case HierarchyType.Events:
            //        return 10;
            //    case HierarchyType.Operators:
            //        return 11;
            //    case HierarchyType.MethodOverloads:
            //    case HierarchyType.PropertyOverloads:
            //        return 12;
            //    case HierarchyType.Field:
            //    case HierarchyType.Method:
            //    case HierarchyType.Property:
            //    case HierarchyType.Event:
            //    case HierarchyType.Operator:
            //        return 13;
            //    default:
            //        break;
            //}
        }
    }
}

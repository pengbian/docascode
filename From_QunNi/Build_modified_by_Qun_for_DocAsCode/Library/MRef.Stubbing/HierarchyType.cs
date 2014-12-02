namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using System.ComponentModel;

    public enum HierarchyType
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        GroupNode = 0x100,
        // ns
        [EditorBrowsable(EditorBrowsableState.Never)]
        NamespaceNode = 0x110,
        RootNamespace,
        NamespaceGroup,
        Namespace,
        // Type level
        [EditorBrowsable(EditorBrowsableState.Never)]
        TypeNode = 0x120,
        Class,
        Structure,
        Enumeration,
        Delegate,
        Interface,
        // member group level
        [EditorBrowsable(EditorBrowsableState.Never)]
        MemberGroupNode = 0x200,
        [EditorBrowsable(EditorBrowsableState.Never)]
        TypeMemberGroupNode = 0x240,
        AllMembers,
        Constructors,
        Fields,
        Methods,
        Properties,
        Events,
        Operators,
        AttachedEvents,
        AttachedProperties,
        [EditorBrowsable(EditorBrowsableState.Never)]
        MemberSubgroupNode = 0x280,
        MethodOverloads,
        OperatorOverloads,
        PropertyOverloads,
        // member
        [EditorBrowsable(EditorBrowsableState.Never)]
        MemberNode = 0x400,
        Field,
        Constructor,
        Method,
        Property,
        Event,
        Operator,
        ExtensionMethod,
        AttachedEvent,
        AttachedProperty,
    }
}

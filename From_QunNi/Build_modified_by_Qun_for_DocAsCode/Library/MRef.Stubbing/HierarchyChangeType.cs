namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    public enum HierarchyChangeType
    {
        /// <summary>
        /// This is item do not require for hierarchy, but it is used by IdMap
        /// </summary>
        None,
        /// <summary>
        /// an new item for hierarchy
        /// </summary>
        Add,
        /// <summary>
        /// removed the item from hierarchy
        /// </summary>
        Remove,
        /// <summary>
        /// update the item for hierarchy
        /// </summary>
        Update,
        /// <summary>
        /// move the item for hierarchy
        /// </summary>
        Move,
    }
}

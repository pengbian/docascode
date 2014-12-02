namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.DataAccessorInterface;
    using Microsoft.Content.BuildEngine.MRef.Caching;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    public abstract class HierarchyComparer
    {
        protected readonly IBlobAccessor m_accessor;

        public HierarchyComparer(IBlobAccessor accessor)
        {
            m_accessor = accessor;
        }

        public static HierarchyComparer GetComparer(IBlobAccessor accessor, string currentVersion, string baseVersion = null)
        {
            if (baseVersion == null)
                return new HierarchyComparerForInitializing(accessor, currentVersion);
            throw new NotSupportedException("Upgrade is not supported in this version.");
        }

        public abstract IEnumerable<HierarchyChanges> ScanHierarchy(IdMap map);
    }
}

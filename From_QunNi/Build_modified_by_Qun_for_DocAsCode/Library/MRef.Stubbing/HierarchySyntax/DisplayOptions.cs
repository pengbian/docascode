namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using System;

    [Flags]
    public enum DisplayOptions
    {
        Basic = 0,
        Template = 1,
        Overload = 2,
        Hierarchy = 4,
        ShowCMod = 8,
        Default = Template | Overload | Hierarchy,
    }
}
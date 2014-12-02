namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    internal static class SR
    {
        public const string ChangesFileNameFormatter = "changes-{0}";

        public const string ChangesXml_RootElement_Name = "r";
        public const string ChangesXml_Element_Name = "e";
        public const string ChangesXml_Attribute_ChangeType = "c";
        public const string ChangesXml_Attribute_ElementType = "t";
        public const string ChangesXml_Attribute_InternalName = "i";
        public const string ChangesXml_Attribute_FriendlyName = "n";
        public const string ChangesXml_Attribute_Assembly = "a";
        public const string ChangesXml_Attribute_File = "f";
        public const string ChangesXml_Attribute_OldParent = "o";
        public const string ChangesXml_Attribute_NewParent = "p";

        public const string TaskKey_ChangesItem = "ChangesItem";
        public const string TaskPhase_Stubbing = "Stubbing";
        public const string TaskStatus_Pending = "Pending";
        public const string TaskStatus_Processing = "Processing";
        public const string TaskStatus_Successed = "Successed";
        public const string TaskStatus_Failed = "Failed";

        public const string DdueTemplate_PlaceHolder_AssemblyName = "{Template:assemblyName}";
        public const string DdueTemplate_PlaceHolder_InternalName = "{Template:codeEntityReference}";

        public const string StepName_Diff = "Scaning difference";
        public const string StepName_Preprocess = "Generating hierarchy text";
        public const string StepName_Submit = "Submitting articles";
        public const string StepName_DbValidation = "Validating data";
        public const string StepName_Commit = "Committing data";
        public const string StepName_Cleanup = "Cleaning up";
    }
}

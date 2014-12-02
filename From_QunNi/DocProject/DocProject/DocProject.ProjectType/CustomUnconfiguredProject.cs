/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

namespace DocProject
{
    using System;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.ProjectSystem;
    using Microsoft.VisualStudio.ProjectSystem.Designers;
    using Microsoft.VisualStudio.ProjectSystem.Utilities;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    [Export]
    [PartMetadata(ProjectCapabilities.Requires, CustomProjectCapabilitiesProvider.CapabilityName)]
    // TODO: figure out why this isn't generating a pkgdef that adds the project extension to the Open Project dialog filter
    [ProjectTypeRegistration(CustomProjectPackage.ProjectTypeGuid, "DocProject", "#2", ProjectExtension, Language, resourcePackageGuid: CustomProjectPackage.PackageGuid, PossibleProjectExtensions = ProjectExtension, ProjectTemplatesDir = @"..\..\Templates\Projects\MyCustomProject")]
    [ProvideProjectItem(CustomProjectPackage.ProjectTypeGuid, "My Items", @"..\..\Templates\ProjectItems\MyCustomProject", 500)]
    internal class CustomUnconfiguredProject
    {
        internal const string ProjectExtension = "docproj";

        internal const string Language = "DocProject";

        [Import]
        internal UnconfiguredProject UnconfiguredProject { get; private set; }

        [Import]
        internal ActiveConfiguredProject<ConfiguredProject> ActiveConfiguredProject { get; private set; }

        [Import]
        internal ActiveConfiguredProject<CustomConfiguredProject> ActiveCustomConfiguredProject { get; private set; }

        [Import(ExportContractNames.VsTypes.IVsProject, typeof(IVsProject))]
        internal IVsHierarchy ProjectHierarchy { get; set; }
    }
}

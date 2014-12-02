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
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.ProjectSystem;
    using Microsoft.VisualStudio.ProjectSystem.Utilities;

    /// <summary>
    /// Adds a project capability to identify docproj projects.
    /// </summary>
    [Export(typeof(IProjectCapabilitiesProvider))]
    [SupportsFileExtension("." + CustomUnconfiguredProject.ProjectExtension)]
    internal class CustomProjectCapabilitiesProvider : IProjectCapabilitiesProvider
    {
        /// <summary>
        /// The project capability itself to include for matching project types.
        /// </summary>
        internal const string CapabilityName = "DocProject";

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomProjectCapabilitiesProvider"/> class.
        /// </summary>
        public CustomProjectCapabilitiesProvider()
        {
        }

        /// <summary>
        /// Gets the capabilities that fit the project in context that this provider contributes.
        /// </summary>
        /// <value>A sequence, possibly empty but never null.</value>
        public Task<IEnumerable<string>> GetCapabilitiesAsync()
        {
            return Task.FromResult<IEnumerable<string>>(new[] { CapabilityName });
        }
    }
}

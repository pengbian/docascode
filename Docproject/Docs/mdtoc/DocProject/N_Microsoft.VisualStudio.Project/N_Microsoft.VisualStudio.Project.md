---
namespace: Microsoft.VisualStudio.Project
---

---
class: Microsoft.VisualStudio.Project.FileDocumentManager
---

---
method: Microsoft.VisualStudio.Project.FileDocumentManager.#ctor(Microsoft.VisualStudio.Project.FileNode)
---

---
method: Microsoft.VisualStudio.Project.FileDocumentManager.Open(System.Boolean,System.Boolean,System.Guid,Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@,Microsoft.VisualStudio.Project.WindowFrameShowAction)
---

---
method: Microsoft.VisualStudio.Project.FileDocumentManager.Open(System.Guid@,System.IntPtr,Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@,Microsoft.VisualStudio.Project.WindowFrameShowAction)
---

---
method: Microsoft.VisualStudio.Project.FileDocumentManager.Open(System.Boolean,System.Boolean,Microsoft.VisualStudio.Project.WindowFrameShowAction)
---

---
method: Microsoft.VisualStudio.Project.FileDocumentManager.OpenWithSpecific(System.UInt32,System.Guid@,System.String,System.Guid@,System.IntPtr,Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@,Microsoft.VisualStudio.Project.WindowFrameShowAction)
---

---
method: Microsoft.VisualStudio.Project.FileDocumentManager.Open(System.Boolean,System.Boolean,System.Guid@,System.IntPtr,Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@,Microsoft.VisualStudio.Project.WindowFrameShowAction)
---

---
class: Microsoft.VisualStudio.Project.DefaultSortOrderNode
---

---
field: Microsoft.VisualStudio.Project.DefaultSortOrderNode.FolderNode
---

---
field: Microsoft.VisualStudio.Project.DefaultSortOrderNode.ReferenceContainerNode
---

---
field: Microsoft.VisualStudio.Project.DefaultSortOrderNode.NestedProjectNode
---

---
field: Microsoft.VisualStudio.Project.DefaultSortOrderNode.HierarchyNode
---

---
class: Microsoft.VisualStudio.Project.SolutionListenerForProjectReferenceUpdate
---

---
method: Microsoft.VisualStudio.Project.SolutionListenerForProjectReferenceUpdate.#ctor(System.IServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.SolutionListenerForProjectReferenceUpdate.OnBeforeUnloadProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.SolutionListenerForProjectReferenceUpdate.OnBeforeCloseProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.SolutionListenerForProjectReferenceUpdate.OnAfterLoadProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.SolutionListenerForProjectReferenceUpdate.OnAfterRenameProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
class: Microsoft.VisualStudio.Project.SolutionListenerForProjectOpen
---

---
method: Microsoft.VisualStudio.Project.SolutionListenerForProjectOpen.#ctor(System.IServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.SolutionListenerForProjectOpen.OnAfterOpenProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.Int32)
---

---
class: Microsoft.VisualStudio.Project.OutputGroup
---

---
property: Microsoft.VisualStudio.Project.OutputGroup.TargetName
---

---
property: Microsoft.VisualStudio.Project.OutputGroup.Project
---

---
property: Microsoft.VisualStudio.Project.OutputGroup.ProjectCfg
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.#ctor(System.String,System.String,Microsoft.VisualStudio.Project.ProjectNode,Microsoft.VisualStudio.Project.ProjectConfig)
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.get_KeyOutput(System.String@)
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.Refresh
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.get_KeyOutputObject(Microsoft.VisualStudio.Shell.Interop.IVsOutput2@)
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.get_ProjectCfg(Microsoft.VisualStudio.Shell.Interop.IVsProjectCfg2@)
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.get_DeployDependencies(System.UInt32,Microsoft.VisualStudio.Shell.Interop.IVsDeployDependency[],System.UInt32[])
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.get_Description(System.String@)
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.get_Outputs(System.UInt32,Microsoft.VisualStudio.Shell.Interop.IVsOutput2[],System.UInt32[])
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.get_CanonicalName(System.String@)
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.get_Property(System.String,System.Object@)
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.get_DisplayName(System.String@)
---

---
method: Microsoft.VisualStudio.Project.OutputGroup.InvalidateGroup
---

---
class: Microsoft.VisualStudio.Project.DocumentManager
---

---
property: Microsoft.VisualStudio.Project.DocumentManager.Node
---

---
method: Microsoft.VisualStudio.Project.DocumentManager.#ctor(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.DocumentManager.CloseWindowFrame(Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@)
---

---
method: Microsoft.VisualStudio.Project.DocumentManager.GetFullPathForDocument
---

---
method: Microsoft.VisualStudio.Project.DocumentManager.OpenWithSpecific(System.UInt32,System.Guid@,System.String,System.Guid@,System.IntPtr,Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@,Microsoft.VisualStudio.Project.WindowFrameShowAction)
---

---
method: Microsoft.VisualStudio.Project.DocumentManager.GetOwnerCaption
---

---
method: Microsoft.VisualStudio.Project.DocumentManager.RenameDocument(System.IServiceProvider,System.String,System.String,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.DocumentManager.Save(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.DocumentManager.UpdateCaption(System.IServiceProvider,System.String,System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.DocumentManager.Open(System.Guid@,System.IntPtr,Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@,Microsoft.VisualStudio.Project.WindowFrameShowAction)
---

---
method: Microsoft.VisualStudio.Project.DocumentManager.Close(Microsoft.VisualStudio.Shell.Interop.__FRAMECLOSE)
---

---
class: Microsoft.VisualStudio.Project.SolutionListener
---

---
property: Microsoft.VisualStudio.Project.SolutionListener.ServiceProvider
---

---
property: Microsoft.VisualStudio.Project.SolutionListener.EventsCookie
---

---
property: Microsoft.VisualStudio.Project.SolutionListener.Solution
---

---
property: Microsoft.VisualStudio.Project.SolutionListener.InteropSafeIVsSolutionEvents
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.#ctor(System.IServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnAfterOpenSolution(System.Object,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnQueryUnloadProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnBeforeCloseSolution(System.Object)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.Init
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnAfterAsynchOpenProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.Dispose(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.Dispose
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnAfterClosingChildren(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnQueryChangeProjectParent(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnBeforeCloseProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnAfterRenameProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnBeforeOpeningChildren(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnAfterMergeSolution(System.Object)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnAfterOpenProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnBeforeClosingChildren(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnBeforeUnloadProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnAfterCloseSolution(System.Object)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnAfterOpeningChildren(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnQueryCloseSolution(System.Object,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnAfterChangeProjectParent(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnQueryCloseProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.Int32,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.SolutionListener.OnAfterLoadProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
class: Microsoft.VisualStudio.Project.MsBuildGeneratedItemType
---

---
field: Microsoft.VisualStudio.Project.MsBuildGeneratedItemType.ReferenceCopyLocalPaths
---

---
field: Microsoft.VisualStudio.Project.MsBuildGeneratedItemType.ComReferenceWrappers
---

---
class: Microsoft.VisualStudio.Project.AssemblyReferenceNode
---

---
property: Microsoft.VisualStudio.Project.AssemblyReferenceNode.Url
---

---
property: Microsoft.VisualStudio.Project.AssemblyReferenceNode.Caption
---

---
method: Microsoft.VisualStudio.Project.AssemblyReferenceNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,System.String)
---

---
method: Microsoft.VisualStudio.Project.AssemblyReferenceNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.AssemblyReferenceNode.BindReferenceData
---

---
method: Microsoft.VisualStudio.Project.AssemblyReferenceNode.CanShowDefaultIcon
---

---
method: Microsoft.VisualStudio.Project.AssemblyReferenceNode.Dispose(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.AssemblyReferenceNode.Close
---

---
method: Microsoft.VisualStudio.Project.AssemblyReferenceNode.ResolveReference
---

---
class: Microsoft.VisualStudio.Project.DesignTimeAssemblyResolution
---

---
property: Microsoft.VisualStudio.Project.DesignTimeAssemblyResolution.EnableLogging
---

---
method: Microsoft.VisualStudio.Project.DesignTimeAssemblyResolution.Initialize(Microsoft.VisualStudio.Project.ProjectNode)
---

---
method: Microsoft.VisualStudio.Project.DesignTimeAssemblyResolution.Resolve(System.Collections.Generic.IEnumerable{System.String})
---

---
class: Microsoft.VisualStudio.Project.ComReferenceNode
---

---
property: Microsoft.VisualStudio.Project.ComReferenceNode.Caption
---

---
property: Microsoft.VisualStudio.Project.ComReferenceNode.MinorVersionNumber
---

---
property: Microsoft.VisualStudio.Project.ComReferenceNode.WrapperTool
---

---
property: Microsoft.VisualStudio.Project.ComReferenceNode.Url
---

---
property: Microsoft.VisualStudio.Project.ComReferenceNode.LCID
---

---
property: Microsoft.VisualStudio.Project.ComReferenceNode.MajorVersionNumber
---

---
property: Microsoft.VisualStudio.Project.ComReferenceNode.TypeGuid
---

---
property: Microsoft.VisualStudio.Project.ComReferenceNode.InstalledFilePath
---

---
property: Microsoft.VisualStudio.Project.ComReferenceNode.EmbedInteropTypes
---

---
method: Microsoft.VisualStudio.Project.ComReferenceNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ComReferenceNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,Microsoft.VisualStudio.Shell.Interop.VSCOMPONENTSELECTORDATA,System.String)
---

---
method: Microsoft.VisualStudio.Project.ComReferenceNode.CanShowDefaultIcon
---

---
method: Microsoft.VisualStudio.Project.ComReferenceNode.BindReferenceData
---

---
method: Microsoft.VisualStudio.Project.ComReferenceNode.CreatePropertiesObject
---

---
class: Microsoft.VisualStudio.Project.ActiveConfigurationChangedEventArgs
---

---
class: Microsoft.VisualStudio.Project.ProjectPackage
---

---
property: Microsoft.VisualStudio.Project.ProjectPackage.ProductUserContext
---

---
method: Microsoft.VisualStudio.Project.ProjectPackage.Dispose(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectPackage.Initialize
---

---
class: Microsoft.VisualStudio.Project.ProjectConfig
---

---
property: Microsoft.VisualStudio.Project.ProjectConfig.OutputGroups
---

---
property: Microsoft.VisualStudio.Project.ProjectConfig.ProjectMgr
---

---
property: Microsoft.VisualStudio.Project.ProjectConfig.ConfigName
---

---
property: Microsoft.VisualStudio.Project.ProjectConfig.ConfigurationProperties
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.#ctor(Microsoft.VisualStudio.Project.ProjectNode,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.GetProjectDesignerPages(Microsoft.VisualStudio.OLE.Interop.CAUUID[])
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.GetProjectItem(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy@,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.EnumOutputs(Microsoft.VisualStudio.Shell.Interop.IVsEnumOutputs@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.GetConfigurationProperty(System.String,System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_DisplayName(System.String@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.DebugLaunch(System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.OpenOutput(System.String,Microsoft.VisualStudio.Shell.Interop.IVsOutput@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.GetPages(Microsoft.VisualStudio.OLE.Interop.CAUUID[])
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_Platform(System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_BuildableProjectCfg(Microsoft.VisualStudio.Shell.Interop.IVsBuildableProjectCfg@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_IsPackaged(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_CfgType(System.Guid@,System.IntPtr@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.PrepareBuild(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_TargetCodePage(System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_IsSpecifyingOutputSupported(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_IsPrivate(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.SetConfigurationProperty(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.OutputsRequireAppRoot(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.GetCfg(Microsoft.VisualStudio.Shell.Interop.IVsCfg@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_IsReleaseOnly(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.CreateOutputGroup(Microsoft.VisualStudio.Project.ProjectNode,System.Collections.Generic.KeyValuePair{System.String,System.String})
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_UpdateSequenceNumber(Microsoft.VisualStudio.OLE.Interop.ULARGE_INTEGER[])
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_VirtualRoot(System.String@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_ProjectCfgProvider(Microsoft.VisualStudio.Shell.Interop.IVsProjectCfgProvider@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.OpenOutputGroup(System.String,Microsoft.VisualStudio.Shell.Interop.IVsOutputGroup@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.QueryDebugLaunch(System.UInt32,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_CanonicalName(System.String@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_OutputGroups(System.UInt32,Microsoft.VisualStudio.Shell.Interop.IVsOutputGroup[],System.UInt32[])
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_IsDebugOnly(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectConfig.get_RootURL(System.String@)
---

---
class: Microsoft.VisualStudio.Project.FileNode
---

---
property: Microsoft.VisualStudio.Project.FileNode.Caption
---

---
property: Microsoft.VisualStudio.Project.FileNode.FileName
---

---
property: Microsoft.VisualStudio.Project.FileNode.ItemTypeGuid
---

---
property: Microsoft.VisualStudio.Project.FileNode.MenuCommandId
---

---
property: Microsoft.VisualStudio.Project.FileNode.ImageIndex
---

---
property: Microsoft.VisualStudio.Project.FileNode.Url
---

---
method: Microsoft.VisualStudio.Project.FileNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.FileNode.RenameFileNode(System.String,System.String,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.FileNode.ExecCommandOnNode(System.Guid,System.UInt32,System.UInt32,System.IntPtr,System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.FileNode.AfterSaveItemAs(System.IntPtr,System.String)
---

---
method: Microsoft.VisualStudio.Project.FileNode.DoDefaultAction
---

---
method: Microsoft.VisualStudio.Project.FileNode.CreateSingleFileGenerator
---

---
method: Microsoft.VisualStudio.Project.FileNode.RecoverFromRenameFailure(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.FileNode.CreatePropertiesObject
---

---
method: Microsoft.VisualStudio.Project.FileNode.RenameChildNodes(Microsoft.VisualStudio.Project.FileNode)
---

---
method: Microsoft.VisualStudio.Project.FileNode.GetIconHandle(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.FileNode.GetAutomationObject
---

---
method: Microsoft.VisualStudio.Project.FileNode.RenameInStorage(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.FileNode.CanShowDefaultIcon
---

---
method: Microsoft.VisualStudio.Project.FileNode.SetEditLabel(System.String)
---

---
method: Microsoft.VisualStudio.Project.FileNode.CanDeleteItem(Microsoft.VisualStudio.Shell.Interop.__VSDELETEITEMOPERATION)
---

---
method: Microsoft.VisualStudio.Project.FileNode.GetMkDocument
---

---
method: Microsoft.VisualStudio.Project.FileNode.QueryStatusOnNode(System.Guid,System.UInt32,System.IntPtr,Microsoft.VisualStudio.Project.QueryStatusResult@)
---

---
class: Microsoft.VisualStudio.Project.OutputTypeConverter
---

---
method: Microsoft.VisualStudio.Project.OutputTypeConverter.#ctor
---

---
method: Microsoft.VisualStudio.Project.OutputTypeConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)
---

---
method: Microsoft.VisualStudio.Project.OutputTypeConverter.GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext)
---

---
method: Microsoft.VisualStudio.Project.OutputTypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object,System.Type)
---

---
method: Microsoft.VisualStudio.Project.OutputTypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object)
---

---
method: Microsoft.VisualStudio.Project.OutputTypeConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Type)
---

---
class: Microsoft.VisualStudio.Project.DebugModeConverter
---

---
method: Microsoft.VisualStudio.Project.DebugModeConverter.#ctor
---

---
method: Microsoft.VisualStudio.Project.DebugModeConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Type)
---

---
method: Microsoft.VisualStudio.Project.DebugModeConverter.GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext)
---

---
method: Microsoft.VisualStudio.Project.DebugModeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object)
---

---
method: Microsoft.VisualStudio.Project.DebugModeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object,System.Type)
---

---
method: Microsoft.VisualStudio.Project.DebugModeConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)
---

---
class: Microsoft.VisualStudio.Project.NodeProperties
---

---
property: Microsoft.VisualStudio.Project.NodeProperties.Name
---

---
property: Microsoft.VisualStudio.Project.NodeProperties.Node
---

---
property: Microsoft.VisualStudio.Project.NodeProperties.ExtenderCATID
---

---
method: Microsoft.VisualStudio.Project.NodeProperties.#ctor(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.NodeProperties.GetCfgProvider(Microsoft.VisualStudio.Shell.Interop.IVsCfgProvider@)
---

---
method: Microsoft.VisualStudio.Project.NodeProperties.ExtenderNames
---

---
method: Microsoft.VisualStudio.Project.NodeProperties.GetComponentName
---

---
method: Microsoft.VisualStudio.Project.NodeProperties.GetProperty(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.NodeProperties.GetPages(Microsoft.VisualStudio.OLE.Interop.CAUUID[])
---

---
method: Microsoft.VisualStudio.Project.NodeProperties.GetProjectItem(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy@,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.NodeProperties.GetProjectDesignerPages(Microsoft.VisualStudio.OLE.Interop.CAUUID[])
---

---
method: Microsoft.VisualStudio.Project.NodeProperties.SetProperty(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.NodeProperties.Extender(System.String)
---

---
class: Microsoft.VisualStudio.Project.BuildActionConverter
---

---
method: Microsoft.VisualStudio.Project.BuildActionConverter.#ctor
---

---
method: Microsoft.VisualStudio.Project.BuildActionConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object)
---

---
method: Microsoft.VisualStudio.Project.BuildActionConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object,System.Type)
---

---
method: Microsoft.VisualStudio.Project.BuildActionConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Type)
---

---
method: Microsoft.VisualStudio.Project.BuildActionConverter.GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext)
---

---
method: Microsoft.VisualStudio.Project.BuildActionConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)
---

---
class: Microsoft.VisualStudio.Project.BuildableProjectConfig
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.#ctor(Microsoft.VisualStudio.Project.ProjectConfig)
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.QueryStatus(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.Wait(System.UInt32,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.Stop(System.Int32)
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.QueryStartClean(System.UInt32,System.Int32[],System.Int32[])
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.QueryStartUpToDateCheck(System.UInt32,System.Int32[],System.Int32[])
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.AdviseBuildStatusCallback(Microsoft.VisualStudio.Shell.Interop.IVsBuildStatusCallback,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.get_ProjectCfg(Microsoft.VisualStudio.Shell.Interop.IVsProjectCfg@)
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.StartClean(Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.StartBuild(Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.StartUpToDateCheck(Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.UnadviseBuildStatusCallback(System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.BuildableProjectConfig.QueryStartBuild(System.UInt32,System.Int32[],System.Int32[])
---

---
class: Microsoft.VisualStudio.Project.OleServiceProvider
---

---
method: Microsoft.VisualStudio.Project.OleServiceProvider.#ctor
---

---
method: Microsoft.VisualStudio.Project.OleServiceProvider.Dispose(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.OleServiceProvider.QueryService(System.Guid@,System.Guid@,System.IntPtr@)
---

---
method: Microsoft.VisualStudio.Project.OleServiceProvider.RemoveService(System.Type)
---

---
method: Microsoft.VisualStudio.Project.OleServiceProvider.AddService(System.Type,System.Object,System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.OleServiceProvider.AddService(System.Type,Microsoft.VisualStudio.Project.OleServiceProvider.ServiceCreatorCallback,System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.OleServiceProvider.Dispose
---

---
class: Microsoft.VisualStudio.Project.ConfigProvider
---

---
property: Microsoft.VisualStudio.Project.ConfigProvider.NewConfigProperties
---

---
property: Microsoft.VisualStudio.Project.ConfigProvider.ProjectMgr
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.#ctor(Microsoft.VisualStudio.Project.ProjectNode)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.DeleteCfgsOfPlatformName(System.String)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.AddCfgsOfCfgName(System.String,System.String,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.GetCfgOfName(System.String,System.String,Microsoft.VisualStudio.Shell.Interop.IVsCfg@)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.GetCfgNames(System.UInt32,System.String[],System.UInt32[])
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.GetCfgs(System.UInt32,Microsoft.VisualStudio.Shell.Interop.IVsCfg[],System.UInt32[],System.UInt32[])
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.UnadviseCfgProviderEvents(System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.RenameCfgsOfCfgName(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.OpenProjectCfg(System.String,Microsoft.VisualStudio.Shell.Interop.IVsProjectCfg@)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.AddCfgsOfPlatformName(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.AdviseCfgProviderEvents(Microsoft.VisualStudio.Shell.Interop.IVsCfgProviderEvents,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.GetPlatformNames(System.UInt32,System.String[],System.UInt32[])
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.GetSupportedPlatformNames(System.UInt32,System.String[],System.UInt32[])
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.CreateProjectConfiguration(System.String)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.GetProjectConfiguration(System.String)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.GetCfgProviderProperty(System.Int32,System.Object@)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.DeleteCfgsOfCfgName(System.String)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.GetAutomationObject(System.String,System.Object@)
---

---
method: Microsoft.VisualStudio.Project.ConfigProvider.get_UsesIndependentConfigurations(System.Int32@)
---

---
class: Microsoft.VisualStudio.Project.DependentFileNodeProperties
---

---
property: Microsoft.VisualStudio.Project.DependentFileNodeProperties.FileName
---

---
property: Microsoft.VisualStudio.Project.DependentFileNodeProperties.FullPath
---

---
property: Microsoft.VisualStudio.Project.DependentFileNodeProperties.BuildAction
---

---
method: Microsoft.VisualStudio.Project.DependentFileNodeProperties.#ctor(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.DependentFileNodeProperties.GetClassName
---

---
class: Microsoft.VisualStudio.Project.ComReferenceProperties
---

---
property: Microsoft.VisualStudio.Project.ComReferenceProperties.EmbedInteropTypes
---

---
method: Microsoft.VisualStudio.Project.ComReferenceProperties.#ctor(Microsoft.VisualStudio.Project.ComReferenceNode)
---

---
class: Microsoft.VisualStudio.Project.ReferenceContainerNode
---

---
property: Microsoft.VisualStudio.Project.ReferenceContainerNode.MenuCommandId
---

---
property: Microsoft.VisualStudio.Project.ReferenceContainerNode.ItemTypeGuid
---

---
property: Microsoft.VisualStudio.Project.ReferenceContainerNode.Url
---

---
property: Microsoft.VisualStudio.Project.ReferenceContainerNode.SupportedReferenceTypes
---

---
property: Microsoft.VisualStudio.Project.ReferenceContainerNode.SortPriority
---

---
property: Microsoft.VisualStudio.Project.ReferenceContainerNode.Caption
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.QueryStatusOnNode(System.Guid,System.UInt32,System.IntPtr,Microsoft.VisualStudio.Project.QueryStatusResult@)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CreateComReferenceNode(Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CreateReferenceNode(System.String,Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.ExecCommandOnNode(System.Guid,System.UInt32,System.UInt32,System.IntPtr,System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.GetIconHandle(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.GetEditLabel
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CreateProjectReferenceNode(Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.EnumReferences
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.LoadReferencesFromBuildProject(Microsoft.Build.Evaluation.Project)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.AddReferenceFromSelectorData(Microsoft.VisualStudio.Shell.Interop.VSCOMPONENTSELECTORDATA,System.String)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CreateComReferenceNode(Microsoft.VisualStudio.Shell.Interop.VSCOMPONENTSELECTORDATA,System.String)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CreateAssemblyReferenceNode(System.String)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CreateAssemblyReferenceNode(Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CanDeleteItem(Microsoft.VisualStudio.Shell.Interop.__VSDELETEITEMOPERATION)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.GetAutomationObject
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.ExcludeFromProject
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CreateReferenceNode(Microsoft.VisualStudio.Shell.Interop.VSCOMPONENTSELECTORDATA,System.String)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CreateProjectReferenceNode(Microsoft.VisualStudio.Shell.Interop.VSCOMPONENTSELECTORDATA)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CreateFileComponent(Microsoft.VisualStudio.Shell.Interop.VSCOMPONENTSELECTORDATA,System.String)
---

---
method: Microsoft.VisualStudio.Project.ReferenceContainerNode.CanShowDefaultIcon
---

---
class: Microsoft.VisualStudio.Project.UIHierarchyUtilities
---

---
method: Microsoft.VisualStudio.Project.UIHierarchyUtilities.GetUIHierarchyWindow(System.IServiceProvider,System.Guid)
---

---
class: Microsoft.VisualStudio.Project.BuildPropertyPage
---

---
property: Microsoft.VisualStudio.Project.BuildPropertyPage.OutputPath
---

---
method: Microsoft.VisualStudio.Project.BuildPropertyPage.#ctor
---

---
method: Microsoft.VisualStudio.Project.BuildPropertyPage.GetClassName
---

---
method: Microsoft.VisualStudio.Project.BuildPropertyPage.ApplyChanges
---

---
method: Microsoft.VisualStudio.Project.BuildPropertyPage.BindProperties
---

---
class: Microsoft.VisualStudio.Project.DependentFileNode
---

---
property: Microsoft.VisualStudio.Project.DependentFileNode.ImageIndex
---

---
method: Microsoft.VisualStudio.Project.DependentFileNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.DependentFileNode.QueryStatusOnNode(System.Guid,System.UInt32,System.IntPtr,Microsoft.VisualStudio.Project.QueryStatusResult@)
---

---
method: Microsoft.VisualStudio.Project.DependentFileNode.CreatePropertiesObject
---

---
method: Microsoft.VisualStudio.Project.DependentFileNode.GetEditLabel
---

---
method: Microsoft.VisualStudio.Project.DependentFileNode.GetIconHandle(System.Boolean)
---

---
class: Microsoft.VisualStudio.Project.ConnectionPointContainer
---

---
class: Microsoft.VisualStudio.Project.DesignPropertyDescriptor
---

---
property: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.DisplayName
---

---
property: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.PropertyType
---

---
property: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.ComponentType
---

---
property: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.IsReadOnly
---

---
property: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.Converter
---

---
method: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.#ctor(System.ComponentModel.PropertyDescriptor)
---

---
method: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.SetValue(System.Object,System.Object)
---

---
method: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.ResetValue(System.Object)
---

---
method: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.GetTypeFromNameProperty(System.String)
---

---
method: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.GetValue(System.Object)
---

---
method: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.CanResetValue(System.Object)
---

---
method: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.ShouldSerializeValue(System.Object)
---

---
method: Microsoft.VisualStudio.Project.DesignPropertyDescriptor.GetEditor(System.Type)
---

---
class: Microsoft.VisualStudio.Project.ProjectNodeProperties
---

---
property: Microsoft.VisualStudio.Project.ProjectNodeProperties.FileName
---

---
property: Microsoft.VisualStudio.Project.ProjectNodeProperties.FullPath
---

---
property: Microsoft.VisualStudio.Project.ProjectNodeProperties.ProjectFolder
---

---
property: Microsoft.VisualStudio.Project.ProjectNodeProperties.ProjectFile
---

---
method: Microsoft.VisualStudio.Project.ProjectNodeProperties.#ctor(Microsoft.VisualStudio.Project.ProjectNode)
---

---
method: Microsoft.VisualStudio.Project.ProjectNodeProperties.GetEditor(System.Type)
---

---
method: Microsoft.VisualStudio.Project.ProjectNodeProperties.GetCfgProvider(Microsoft.VisualStudio.Shell.Interop.IVsCfgProvider@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNodeProperties.GetClassName
---

---
class: Microsoft.VisualStudio.Project.ProjectPropertyChangedArgs
---

---
property: Microsoft.VisualStudio.Project.ProjectPropertyChangedArgs.OldValue
---

---
property: Microsoft.VisualStudio.Project.ProjectPropertyChangedArgs.NewValue
---

---
property: Microsoft.VisualStudio.Project.ProjectPropertyChangedArgs.PropertyName
---

---
class: Microsoft.VisualStudio.Project.ReferenceNodeProperties
---

---
property: Microsoft.VisualStudio.Project.ReferenceNodeProperties.Name
---

---
property: Microsoft.VisualStudio.Project.ReferenceNodeProperties.CopyToLocal
---

---
property: Microsoft.VisualStudio.Project.ReferenceNodeProperties.FullPath
---

---
method: Microsoft.VisualStudio.Project.ReferenceNodeProperties.#ctor(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.ReferenceNodeProperties.GetClassName
---

---
class: Microsoft.VisualStudio.Project.EnumDependencies
---

---
method: Microsoft.VisualStudio.Project.EnumDependencies.#ctor(System.Collections.Generic.IList{Microsoft.VisualStudio.Shell.Interop.IVsDependency})
---

---
method: Microsoft.VisualStudio.Project.EnumDependencies.#ctor(System.Collections.Generic.IList{Microsoft.VisualStudio.Shell.Interop.IVsBuildDependency})
---

---
method: Microsoft.VisualStudio.Project.EnumDependencies.Clone(Microsoft.VisualStudio.Shell.Interop.IVsEnumDependencies@)
---

---
method: Microsoft.VisualStudio.Project.EnumDependencies.Skip(System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.EnumDependencies.Next(System.UInt32,Microsoft.VisualStudio.Shell.Interop.IVsDependency[],System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.EnumDependencies.Reset
---

---
class: Microsoft.VisualStudio.Project.ProjectNode
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ProjectFile
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.BuildLogger
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.BaseURI
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.Caption
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ConfigProvider
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.InteropSafeIVsHierarchy
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ErrorString
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.BuildDependencies
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ProjectDesignerEditor
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.InteropSafeIVsUIHierarchy
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.SupportsProjectDesigner
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ItemTypeGuid
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.IsSccDisabled
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ProjectIDGuid
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ServiceProvider
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.InteropSafeIVsProject3
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.OutputBaseRelativePath
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.WarningString
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.FileTemplateProcessor
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ShowProjectInSolutionPage
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.InteropSafeIVsComponentUser
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ProjectFolder
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.IsNewProject
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.LastModifiedTime
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.FileName
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.IsProjectEventsListener
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.TaskProvider
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.BuildInProgress
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.InteropSafeIVsSccProject2
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ProjectType
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.DisableQueryEdit
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.InteropSafeIVsUIHierWinClipboardHelperEvents
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.TargetFrameworkMoniker
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.MenuCommandId
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ProjectGuid
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.IsIdeInCommandLineMode
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.IsClosed
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ImageHandler
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.Site
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.IsProjectFileDirty
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.ImageIndex
---

---
property: Microsoft.VisualStudio.Project.ProjectNode.Url
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.#ctor
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetOutputAssembly(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.IsEmbeddedResource(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetReferenceContainer
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetCfgProvider(Microsoft.VisualStudio.Shell.Interop.IVsCfgProvider@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.NodeHasDesigner(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetFormatList(System.String@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OnDropNotify(System.Int32,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.ResumeMSBuild(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SccGlyphChanged(System.Int32,System.UInt32[],Microsoft.VisualStudio.Shell.Interop.VsStateIcon[],System.UInt32[])
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.IsDirty(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CallMSBuild(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.InitializeForOuter(System.String,System.String,System.String,System.UInt32,System.Guid@,System.IntPtr@,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CanOverwriteExistingItem(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.DragLeave
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetOutputLogger(Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.DragOver(System.UInt32,System.UInt32,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetMkDocument
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetOutputPath(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddCATIDMapping(System.Type,System.Guid)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CreateDependentFileNode(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.RemoveBuildDependency(Microsoft.VisualStudio.Shell.Interop.IVsBuildDependency)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OnAggregationComplete
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddDependentFileNode(System.Collections.Generic.IDictionary{System.String,Microsoft.Build.Evaluation.ProjectItem},System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.ResumeMSBuild(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetTargetFramework(System.String@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Save(System.String,System.Int32,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddFileToMsBuild(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OnHandleConfigurationRelatedGlobalProperties(System.Object,Microsoft.VisualStudio.Project.ActiveConfigurationChangedEventArgs)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetCompiler
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CreateConfigProvider
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CancelBatchEdit
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetBoolAttr(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Build(System.String,Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddProjectReference
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetSccFiles(System.UInt32,Microsoft.VisualStudio.OLE.Interop.CALPOLESTR[],Microsoft.VisualStudio.OLE.Interop.CADWORD[])
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.RaiseProjectPropertyChanged(System.String,System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddItem(System.UInt32,Microsoft.VisualStudio.Shell.Interop.VSADDITEMOPERATION,System.String,System.UInt32,System.String[],System.IntPtr,Microsoft.VisualStudio.Shell.Interop.VSADDRESULT[])
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Close
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetAssemblyName(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetAggregateProjectTypeGuids(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OnOpenItem(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CreateMsBuildFileItem(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.PrepareBuild(System.String,System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.IsCodeFile(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.RenameProjectFile(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.VerifySubFolderExists(System.String,Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.ResumeMSBuild(System.String,Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetFile(System.Int32,System.UInt32,System.UInt32@,System.String@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.IsItemTypeFileType(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SaveMSBuildProjectFileAs(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetAggregateProjectTypeGuids(System.String@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CallMSBuild(System.String,Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.NodeFromItemId(System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetHostObject(System.String,System.String,System.Object)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetBuildSystemKind(System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Build(System.String,Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Remove(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.FilterItemTypeToBeAddedToHierarchy(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Dispose(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.DragEnter(Microsoft.VisualStudio.OLE.Interop.IDataObject,System.UInt32,System.UInt32,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetProjectProperty(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.DoMSBuildSubmission(Microsoft.VisualStudio.Project.BuildKind,System.String,System.Action{Microsoft.VisualStudio.Project.MSBuildResult,System.String})
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.IsFlavorDirty
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetPriorityProjectDesignerPages
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.PersistXMLFragments
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CreateFolderNodes(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.UpgradeProject(System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.EnumDependencies(Microsoft.VisualStudio.Shell.Interop.IVsEnumDependencies@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OnTargetFrameworkMonikerChanged(Microsoft.VisualStudio.Project.ProjectOptions,System.Runtime.Versioning.FrameworkName,System.Runtime.Versioning.FrameworkName)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CanDeleteItem(Microsoft.VisualStudio.Shell.Interop.__VSDELETEITEMOPERATION)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetGuidProperty(System.Int32,System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.UnRegisterProject
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetSccLocation(System.String,System.String,System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OnClear(System.Int32)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetInnerProject(System.Object)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.EndBatchEdit
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetItemContext(System.UInt32,Microsoft.VisualStudio.OLE.Interop.IServiceProvider@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.ExecCommandOnNode(System.Guid,System.UInt32,System.UInt32,System.IntPtr,System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetProjectFileDirty(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetProjectElement(Microsoft.Build.Evaluation.ProjectItem)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetProjectOptions(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CreateProjectOptions
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.InitNew(System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetConfigurationDependentPropertyPages
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CreateReferenceContainerNode
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddFileFromTemplate(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddBuildDependency(Microsoft.VisualStudio.Shell.Interop.IVsBuildDependency)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetProperty(System.Int32)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddNewFileNodeToHierarchy(Microsoft.VisualStudio.Project.HierarchyNode,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OpenItemWithSpecific(System.UInt32,System.UInt32,System.Guid@,System.String,System.Guid@,System.IntPtr,Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Load(System.String,System.UInt32,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetProjectProperty(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddComponent(Microsoft.VisualStudio.Shell.Interop.VSADDCOMPOPERATION,System.UInt32,System.IntPtr[],System.IntPtr,Microsoft.VisualStudio.Shell.Interop.VSADDCOMPRESULT[])
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SuspendMSBuild
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.RunWizard(Microsoft.VisualStudio.Project.HierarchyNode,System.String,System.String,System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetInner
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Load(System.String,System.String,System.String,System.UInt32,System.Guid@,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GenerateUniqueItemName(System.UInt32,System.String,System.String,System.String@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Build(System.UInt32,System.String,Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CleanProject
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.RegisterSccProject
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OnBeforeDropNotify(Microsoft.VisualStudio.OLE.Interop.IDataObject,System.UInt32,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.UpdateTargetFramework(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetGuidProperty(System.Int32,System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Build(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetDropInfo(System.UInt32@,Microsoft.VisualStudio.OLE.Interop.IDataObject@,Microsoft.VisualStudio.Shell.Interop.IDropSource@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OpenDependency(System.String,Microsoft.VisualStudio.Shell.Interop.IVsDependency@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CallMSBuild(System.String,Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.IsDocumentInProject(System.String,System.Int32@,Microsoft.VisualStudio.Shell.Interop.VSDOCUMENTPRIORITY[],System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddFolderToMsBuild(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CreatePropertiesObject
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.ResolveAssemblyPathInTargetFx(System.String[],System.UInt32,Microsoft.VisualStudio.Shell.Interop.VsResolvedAssemblyPath[],System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetProjectProperty(System.String,System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetAutomationObject
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetProperty(System.Int32,System.Object)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SaveCompleted(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Reload
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.QueryStatusOnNode(System.Guid,System.UInt32,System.IntPtr,Microsoft.VisualStudio.Project.QueryStatusResult@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetEditLabel(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.InitializeProjectProperties
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SaveAs(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.InvokeMsBuild(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OnPaste(System.Int32,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CreateFileNode(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OpenItem(System.UInt32,System.Guid@,System.IntPtr,Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CallMSBuild(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.SetBuildConfigurationProperties(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Drop(Microsoft.VisualStudio.OLE.Interop.IDataObject,System.UInt32,System.UInt32,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddNodeIfTargetExistInStorage(Microsoft.VisualStudio.Project.HierarchyNode,System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.ReopenItem(System.UInt32,System.Guid@,System.String,System.Guid@,System.IntPtr,Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.Build(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetSccSpecialFiles(System.UInt32,System.String,Microsoft.VisualStudio.OLE.Interop.CALPOLESTR[],Microsoft.VisualStudio.OLE.Interop.CADWORD[])
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetClassID(System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.AddItemWithSpecific(System.UInt32,Microsoft.VisualStudio.Shell.Interop.VSADDITEMOPERATION,System.String,System.UInt32,System.String[],System.IntPtr,System.UInt32,System.Guid@,System.String,System.Guid@,Microsoft.VisualStudio.Shell.Interop.VSADDRESULT[])
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.TransferItem(System.String,System.String,Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.BuildTarget(System.String,System.Boolean@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetProjectPropertyUnevaluated(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.OverwriteExistingItem(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.RemoveItem(System.UInt32,System.UInt32,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.WalkSourceProjectAndAdd(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.UInt32,Microsoft.VisualStudio.Project.HierarchyNode,System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetMkDocument(System.UInt32,System.String@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CreateFileNode(Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.StartBatchEdit
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.CreateDependentFileNode(Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetCurFile(System.String@,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.FlushBuildLoggerContent
---

---
method: Microsoft.VisualStudio.Project.ProjectNode.GetConfigurationIndependentPropertyPages
---

---
class: Microsoft.VisualStudio.Project.FileNodeProperties
---

---
property: Microsoft.VisualStudio.Project.FileNodeProperties.FileName
---

---
property: Microsoft.VisualStudio.Project.FileNodeProperties.BuildAction
---

---
property: Microsoft.VisualStudio.Project.FileNodeProperties.FullPath
---

---
property: Microsoft.VisualStudio.Project.FileNodeProperties.Extension
---

---
method: Microsoft.VisualStudio.Project.FileNodeProperties.#ctor(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.FileNodeProperties.GetClassName
---

---
class: Microsoft.VisualStudio.Project.SingleFileGeneratorNodeProperties
---

---
property: Microsoft.VisualStudio.Project.SingleFileGeneratorNodeProperties.CustomTool
---

---
property: Microsoft.VisualStudio.Project.SingleFileGeneratorNodeProperties.CustomToolNamespace
---

---
method: Microsoft.VisualStudio.Project.SingleFileGeneratorNodeProperties.#ctor(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
class: Microsoft.VisualStudio.Project.ProjectFileAttributeValue
---

---
field: Microsoft.VisualStudio.Project.ProjectFileAttributeValue.Component
---

---
field: Microsoft.VisualStudio.Project.ProjectFileAttributeValue.Designer
---

---
field: Microsoft.VisualStudio.Project.ProjectFileAttributeValue.UserControl
---

---
field: Microsoft.VisualStudio.Project.ProjectFileAttributeValue.Form
---

---
field: Microsoft.VisualStudio.Project.ProjectFileAttributeValue.Code
---

---
class: Microsoft.VisualStudio.Project.PropertyPageTypeConverterAttribute
---

---
property: Microsoft.VisualStudio.Project.PropertyPageTypeConverterAttribute.ConverterType
---

---
method: Microsoft.VisualStudio.Project.PropertyPageTypeConverterAttribute.#ctor(System.Type)
---

---
class: Microsoft.VisualStudio.Project.ReplacePairToken
---

---
property: Microsoft.VisualStudio.Project.ReplacePairToken.Replacement
---

---
property: Microsoft.VisualStudio.Project.ReplacePairToken.Token
---

---
method: Microsoft.VisualStudio.Project.ReplacePairToken.#ctor(System.String,System.String)
---

---
class: Microsoft.VisualStudio.Project.LocalizableProperties
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetPropertyOwner(System.ComponentModel.PropertyDescriptor)
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetDefaultProperty
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetProperties(System.Attribute[])
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetConverter
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetProperties
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetEvents(System.Attribute[])
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetClassName
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetComponentName
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetEditor(System.Type)
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.CreateDesignPropertyDescriptor(System.ComponentModel.PropertyDescriptor)
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetEvents
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetDefaultEvent
---

---
method: Microsoft.VisualStudio.Project.LocalizableProperties.GetAttributes
---

---
class: Microsoft.VisualStudio.Project.PropertiesEditorLauncher
---

---
method: Microsoft.VisualStudio.Project.PropertiesEditorLauncher.#ctor(Microsoft.VisualStudio.Shell.ServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.PropertiesEditorLauncher.EditComponent(System.ComponentModel.ITypeDescriptorContext,System.Object)
---

---
class: Microsoft.VisualStudio.Project.AutomationBrowsableAttribute
---

---
property: Microsoft.VisualStudio.Project.AutomationBrowsableAttribute.Browsable
---

---
method: Microsoft.VisualStudio.Project.AutomationBrowsableAttribute.#ctor(System.Boolean)
---

---
class: Microsoft.VisualStudio.Project.ProjectContainerNode
---

---
property: Microsoft.VisualStudio.Project.ProjectContainerNode.BuildNestedProjectsOnBuild
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.#ctor
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.ReloadNestedProjectNode(Microsoft.VisualStudio.Project.NestedProjectNode)
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.EnumNestedHierachiesForBuildDependency
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.CloseChildren
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.ReloadItem(System.UInt32,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.AddVirtualProjects
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.CreateNestedProjectNode(Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.FilterItemTypeToBeAddedToHierarchy(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.GetProjectTemplatePath(Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.OpenChildren
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.Reload
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.SaveItem(Microsoft.VisualStudio.Shell.Interop.VSSAVEFLAGS,System.String,System.UInt32,System.IntPtr,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.IsItemDirty(System.UInt32,System.IntPtr,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectContainerNode.GetNestedHierarchy(System.UInt32,System.Guid@,System.IntPtr@,System.UInt32@)
---

---
class: Microsoft.VisualStudio.Project.HierarchyNode
---

---
field: Microsoft.VisualStudio.Project.HierarchyNode.SolutionExplorer
---

---
field: Microsoft.VisualStudio.Project.HierarchyNode.NoImage
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.ID
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.SortPriority
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.ItemsDraggedOrCutOrCopied
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.NodeProperties
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.Parent
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.CanExecuteCommand
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.ImageIndex
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.ExcludeNodeFromScc
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.ProjectMgr
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.IsExpanded
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.DocCookie
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.VirtualNodeName
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.Caption
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.FirstChild
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.NameRelationSeparator
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.HasDesigner
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.PreviousSibling
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.SourceDraggedOrCutOrCopied
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.StateIconIndex
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.LastChild
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.ItemTypeGuid
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.Url
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.ItemNode
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.HasParentNodeNameRelation
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.NextSibling
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.MenuCommandId
---

---
property: Microsoft.VisualStudio.Project.HierarchyNode.OleServiceProvider
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.#ctor
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Dispose(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetDropInfo(System.UInt32@,Microsoft.VisualStudio.OLE.Interop.IDataObject@,Microsoft.VisualStudio.Shell.Interop.IDropSource@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Unused3
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.SetGuidProperty(System.UInt32,System.Int32,System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.RemoveChild(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetMkDocument
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.IsItemReloadable(System.UInt32,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.CreatePropertiesObject
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.QueryClose(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.DoDefaultAction
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.OnPropertyChanged(Microsoft.VisualStudio.Project.HierarchyNode,System.Int32,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetRelationalName
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Unused1
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.DisplayContextMenu(System.Collections.Generic.IList{Microsoft.VisualStudio.Project.HierarchyNode},System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.AddItemToHierarchy(Microsoft.VisualStudio.Project.HierarchyAddType)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Exec(System.Guid@,System.UInt32,System.UInt32,System.IntPtr,System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.ExecCommandOnNode(System.Guid,System.UInt32,System.UInt32,System.IntPtr,System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.OnInvalidateItems(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.DragOver(System.UInt32,System.UInt32,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetIconHandle(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetRelationNameExtension
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.OnItemDeleted
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.IgnoreItemFileChanges(System.UInt32,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.AfterSaveItemAs(System.IntPtr,System.String)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.CanShowDefaultIcon
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Unused2
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.ParseCanonicalName(System.String,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.ReloadItem(System.UInt32,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.SetGuidProperty(System.Int32,System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetCanonicalName(System.UInt32,System.String@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.DragEnter(Microsoft.VisualStudio.OLE.Interop.IDataObject,System.UInt32,System.UInt32,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.ReDraw(Microsoft.VisualStudio.Project.UIHierarchyElement)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetGuidProperty(System.Int32,System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.DisableCmdInCurrentMode(System.Guid,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.QueryStatusSelectionOnNodes(System.Collections.Generic.IList{Microsoft.VisualStudio.Project.HierarchyNode},System.Guid,System.UInt32,System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.AddChild(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.InternalExecCommand(System.Guid,System.UInt32,System.UInt32,System.IntPtr,System.IntPtr,Microsoft.VisualStudio.Project.CommandOrigin)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.OnBeforeDropNotify(Microsoft.VisualStudio.OLE.Interop.IDataObject,System.UInt32,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.AdviseHierarchyEvents(Microsoft.VisualStudio.Shell.Interop.IVsHierarchyEvents,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetEditLabel
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.DragLeave
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.DeleteItem(System.UInt32,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.OnItemAdded(Microsoft.VisualStudio.Project.HierarchyNode,Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetCanonicalName
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetAutomationObject
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetProperty(System.Int32)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.OnItemsAppended(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.CloseDocumentWindow(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.SetProperty(System.UInt32,System.Int32,System.Object)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.AddNewFolder
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Dispose
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.IsItemDirty(System.UInt32,System.IntPtr,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Remove(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.QueryStatusOnNode(System.Guid,System.UInt32,System.IntPtr,Microsoft.VisualStudio.Project.QueryStatusResult@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.QueryStatusSelection(System.Guid,System.UInt32,Microsoft.VisualStudio.OLE.Interop.OLECMD[],System.IntPtr,Microsoft.VisualStudio.Project.CommandOrigin)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.OnDropNotify(System.Int32,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.DisableCommandOnNodesThatDoNotSupportMultiSelection(System.Guid,System.UInt32,System.Collections.Generic.IList{Microsoft.VisualStudio.Project.HierarchyNode},System.Boolean@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.ExecCommand(System.UInt32,System.Guid@,System.UInt32,System.UInt32,System.IntPtr,System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Drop(Microsoft.VisualStudio.OLE.Interop.IDataObject,System.UInt32,System.UInt32,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.SaveItem(Microsoft.VisualStudio.Shell.Interop.VSSAVEFLAGS,System.String,System.UInt32,System.IntPtr,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Unused0
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.ShowContextMenu(System.Int32,System.Guid,Microsoft.VisualStudio.Shell.Interop.POINTS)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.QueryDeleteItem(System.UInt32,System.UInt32,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Unused4
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.QueryStatusCommand(System.UInt32,System.Guid@,System.UInt32,Microsoft.VisualStudio.OLE.Interop.OLECMD[],System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.Close
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.QueryStatus(System.Guid@,System.UInt32,Microsoft.VisualStudio.OLE.Interop.OLECMD[],System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.ExecCommandThatDependsOnSelectedNodes(System.Guid,System.UInt32,System.UInt32,System.IntPtr,System.IntPtr,Microsoft.VisualStudio.Project.CommandOrigin,System.Collections.Generic.IList{Microsoft.VisualStudio.Project.HierarchyNode},System.Boolean@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.UnadviseHierarchyEvents(System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.FindChildByProjectElement(Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.SetProperty(System.Int32,System.Object)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.ExecCommandIndependentOfSelection(System.Guid,System.UInt32,System.UInt32,System.IntPtr,System.IntPtr,Microsoft.VisualStudio.Project.CommandOrigin,System.Boolean@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetProperty(System.UInt32,System.Int32,System.Object@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.CanDeleteItem(Microsoft.VisualStudio.Shell.Interop.__VSDELETEITEMOPERATION)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.SetEditLabel(System.String)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetNestedHierarchy(System.UInt32,System.Guid@,System.IntPtr@,System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetService(System.Type)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.GetGuidProperty(System.UInt32,System.Int32,System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.ShowInDesigner(System.Collections.Generic.IList{Microsoft.VisualStudio.Project.HierarchyNode})
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.QueryStatusCommandFromOleCommandTarget(System.Guid,System.UInt32,System.Boolean@)
---

---
method: Microsoft.VisualStudio.Project.HierarchyNode.ExcludeFromProject
---

---
class: Microsoft.VisualStudio.Project.NestedProjectBuildDependency
---

---
method: Microsoft.VisualStudio.Project.NestedProjectBuildDependency.#ctor(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectBuildDependency.get_MustUpdateBefore(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectBuildDependency.get_HelpContext(System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectBuildDependency.get_Type(System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectBuildDependency.get_HelpFile(System.String@)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectBuildDependency.get_CanonicalName(System.String@)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectBuildDependency.get_ReferredProject(System.Object@)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectBuildDependency.get_Description(System.String@)
---

---
class: Microsoft.VisualStudio.Project.BuildDependency
---

---
method: Microsoft.VisualStudio.Project.BuildDependency.#ctor(Microsoft.VisualStudio.Project.ProjectNode,System.Guid)
---

---
method: Microsoft.VisualStudio.Project.BuildDependency.get_Type(System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.BuildDependency.get_MustUpdateBefore(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.BuildDependency.get_CanonicalName(System.String@)
---

---
method: Microsoft.VisualStudio.Project.BuildDependency.get_HelpContext(System.UInt32@)
---

---
method: Microsoft.VisualStudio.Project.BuildDependency.get_ReferredProject(System.Object@)
---

---
method: Microsoft.VisualStudio.Project.BuildDependency.get_Description(System.String@)
---

---
method: Microsoft.VisualStudio.Project.BuildDependency.get_HelpFile(System.String@)
---

---
class: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener
---

---
property: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.SolutionBuildManager3
---

---
property: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.ServiceProvider
---

---
property: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.SolutionBuildManager2
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.#ctor(System.IServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.UpdateSolution_Done(System.Int32,System.Int32,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.OnActiveProjectCfgChange(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy)
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.UpdateSolution_Cancel
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.UpdateProjectCfg_Done(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,Microsoft.VisualStudio.Shell.Interop.IVsCfg,Microsoft.VisualStudio.Shell.Interop.IVsCfg,System.UInt32,System.Int32,System.Int32)
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.UpdateProjectCfg_Begin(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,Microsoft.VisualStudio.Shell.Interop.IVsCfg,Microsoft.VisualStudio.Shell.Interop.IVsCfg,System.UInt32,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.Dispose(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.Dispose
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.OnAfterActiveSolutionCfgChange(Microsoft.VisualStudio.Shell.Interop.IVsCfg,Microsoft.VisualStudio.Shell.Interop.IVsCfg)
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.UpdateSolution_Begin(System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.OnBeforeActiveSolutionCfgChange(Microsoft.VisualStudio.Shell.Interop.IVsCfg,Microsoft.VisualStudio.Shell.Interop.IVsCfg)
---

---
method: Microsoft.VisualStudio.Project.UpdateSolutionEventsListener.UpdateSolution_StartUpdate(System.Int32@)
---

---
class: Microsoft.VisualStudio.Project.ProjectFactory
---

---
property: Microsoft.VisualStudio.Project.ProjectFactory.Package
---

---
property: Microsoft.VisualStudio.Project.ProjectFactory.BuildProject
---

---
property: Microsoft.VisualStudio.Project.ProjectFactory.Site
---

---
property: Microsoft.VisualStudio.Project.ProjectFactory.BuildEngine
---

---
method: Microsoft.VisualStudio.Project.ProjectFactory.#ctor(Microsoft.VisualStudio.Shell.Package)
---

---
method: Microsoft.VisualStudio.Project.ProjectFactory.PreCreateForOuter(System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.ProjectFactory.CreateProject
---

---
method: Microsoft.VisualStudio.Project.ProjectFactory.CanCreateProjectAsynchronously(System.Guid@,System.String,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ProjectFactory.CreateProject(System.String,System.String,System.String,System.UInt32,System.Guid@,System.IntPtr@,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.ProjectFactory.ProjectTypeGuids(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectFactory.OnBeforeCreateProjectAsync(System.Guid@,System.String,System.String,System.String,System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.ProjectFactory.CreateProjectAsync(System.Guid@,System.String,System.String,System.String,System.UInt32)
---

---
class: Microsoft.VisualStudio.Project.FrameworkNameConverter
---

---
method: Microsoft.VisualStudio.Project.FrameworkNameConverter.#ctor
---

---
method: Microsoft.VisualStudio.Project.FrameworkNameConverter.GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext)
---

---
method: Microsoft.VisualStudio.Project.FrameworkNameConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Type)
---

---
method: Microsoft.VisualStudio.Project.FrameworkNameConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object)
---

---
method: Microsoft.VisualStudio.Project.FrameworkNameConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object,System.Type)
---

---
method: Microsoft.VisualStudio.Project.FrameworkNameConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)
---

---
class: Microsoft.VisualStudio.Project.SettingsPage
---

---
property: Microsoft.VisualStudio.Project.SettingsPage.ProjectMgr
---

---
property: Microsoft.VisualStudio.Project.SettingsPage.Grid
---

---
property: Microsoft.VisualStudio.Project.SettingsPage.IsDirty
---

---
property: Microsoft.VisualStudio.Project.SettingsPage.ThePanel
---

---
property: Microsoft.VisualStudio.Project.SettingsPage.Name
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.Dispose
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.SetPageSite(Microsoft.VisualStudio.OLE.Interop.IPropertyPageSite)
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.SetConfigProperty(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.TranslateAccelerator(Microsoft.VisualStudio.OLE.Interop.MSG[])
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.UpdateObjects
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.GetPageInfo(Microsoft.VisualStudio.OLE.Interop.PROPPAGEINFO[])
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.GetTypedProperty(System.String,System.Type)
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.Help(System.String)
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.SetObjects(System.UInt32,System.Object[])
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.IsPageDirty
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.Move(Microsoft.VisualStudio.OLE.Interop.RECT[])
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.Activate(System.IntPtr,Microsoft.VisualStudio.OLE.Interop.RECT[],System.Int32)
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.Deactivate
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.GetProperty(System.String)
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.GetProjectConfigurations
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.ApplyChanges
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.GetConfigProperty(System.String)
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.BindProperties
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.Show(System.UInt32)
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.GetTypedConfigProperty(System.String,System.Type)
---

---
method: Microsoft.VisualStudio.Project.SettingsPage.Apply
---

---
class: Microsoft.VisualStudio.Project.FolderNodeProperties
---

---
property: Microsoft.VisualStudio.Project.FolderNodeProperties.FullPath
---

---
property: Microsoft.VisualStudio.Project.FolderNodeProperties.FolderName
---

---
property: Microsoft.VisualStudio.Project.FolderNodeProperties.FileName
---

---
method: Microsoft.VisualStudio.Project.FolderNodeProperties.#ctor(Microsoft.VisualStudio.Project.HierarchyNode)
---

---
method: Microsoft.VisualStudio.Project.FolderNodeProperties.GetClassName
---

---
class: Microsoft.VisualStudio.Project.ProjectDocumentsListener
---

---
property: Microsoft.VisualStudio.Project.ProjectDocumentsListener.ProjectDocumentTracker2
---

---
property: Microsoft.VisualStudio.Project.ProjectDocumentsListener.ServiceProvider
---

---
property: Microsoft.VisualStudio.Project.ProjectDocumentsListener.EventsCookie
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.#ctor(Microsoft.VisualStudio.Shell.ServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.Init
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnAfterSccStatusChanged(System.Int32,System.Int32,Microsoft.VisualStudio.Shell.Interop.IVsProject[],System.Int32[],System.String[],System.UInt32[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnQueryRemoveFiles(Microsoft.VisualStudio.Shell.Interop.IVsProject,System.Int32,System.String[],Microsoft.VisualStudio.Shell.Interop.VSQUERYREMOVEFILEFLAGS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYREMOVEFILERESULTS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYREMOVEFILERESULTS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnAfterRemoveFiles(System.Int32,System.Int32,Microsoft.VisualStudio.Shell.Interop.IVsProject[],System.Int32[],System.String[],Microsoft.VisualStudio.Shell.Interop.VSREMOVEFILEFLAGS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnAfterRenameDirectories(System.Int32,System.Int32,Microsoft.VisualStudio.Shell.Interop.IVsProject[],System.Int32[],System.String[],System.String[],Microsoft.VisualStudio.Shell.Interop.VSRENAMEDIRECTORYFLAGS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnAfterRenameFiles(System.Int32,System.Int32,Microsoft.VisualStudio.Shell.Interop.IVsProject[],System.Int32[],System.String[],System.String[],Microsoft.VisualStudio.Shell.Interop.VSRENAMEFILEFLAGS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.Dispose(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnAfterAddFilesEx(System.Int32,System.Int32,Microsoft.VisualStudio.Shell.Interop.IVsProject[],System.Int32[],System.String[],Microsoft.VisualStudio.Shell.Interop.VSADDFILEFLAGS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnQueryAddDirectories(Microsoft.VisualStudio.Shell.Interop.IVsProject,System.Int32,System.String[],Microsoft.VisualStudio.Shell.Interop.VSQUERYADDDIRECTORYFLAGS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYADDDIRECTORYRESULTS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYADDDIRECTORYRESULTS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnQueryRemoveDirectories(Microsoft.VisualStudio.Shell.Interop.IVsProject,System.Int32,System.String[],Microsoft.VisualStudio.Shell.Interop.VSQUERYREMOVEDIRECTORYFLAGS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYREMOVEDIRECTORYRESULTS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYREMOVEDIRECTORYRESULTS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnAfterRemoveDirectories(System.Int32,System.Int32,Microsoft.VisualStudio.Shell.Interop.IVsProject[],System.Int32[],System.String[],Microsoft.VisualStudio.Shell.Interop.VSREMOVEDIRECTORYFLAGS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.Dispose
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnQueryRenameFiles(Microsoft.VisualStudio.Shell.Interop.IVsProject,System.Int32,System.String[],System.String[],Microsoft.VisualStudio.Shell.Interop.VSQUERYRENAMEFILEFLAGS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYRENAMEFILERESULTS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYRENAMEFILERESULTS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnQueryRenameDirectories(Microsoft.VisualStudio.Shell.Interop.IVsProject,System.Int32,System.String[],System.String[],Microsoft.VisualStudio.Shell.Interop.VSQUERYRENAMEDIRECTORYFLAGS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYRENAMEDIRECTORYRESULTS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYRENAMEDIRECTORYRESULTS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnQueryAddFiles(Microsoft.VisualStudio.Shell.Interop.IVsProject,System.Int32,System.String[],Microsoft.VisualStudio.Shell.Interop.VSQUERYADDFILEFLAGS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYADDFILERESULTS[],Microsoft.VisualStudio.Shell.Interop.VSQUERYADDFILERESULTS[])
---

---
method: Microsoft.VisualStudio.Project.ProjectDocumentsListener.OnAfterAddDirectoriesEx(System.Int32,System.Int32,Microsoft.VisualStudio.Shell.Interop.IVsProject[],System.Int32[],System.String[],Microsoft.VisualStudio.Shell.Interop.VSADDDIRECTORYFLAGS[])
---

---
class: Microsoft.VisualStudio.Project.ProjectReferencesProperties
---

---
property: Microsoft.VisualStudio.Project.ProjectReferencesProperties.FullPath
---

---
method: Microsoft.VisualStudio.Project.ProjectReferencesProperties.#ctor(Microsoft.VisualStudio.Project.ProjectReferenceNode)
---

---
class: Microsoft.VisualStudio.Project.AfterProjectFileOpenedEventArgs
---

---
class: Microsoft.VisualStudio.Project.MsBuildTarget
---

---
field: Microsoft.VisualStudio.Project.MsBuildTarget.ResolveProjectReferences
---

---
field: Microsoft.VisualStudio.Project.MsBuildTarget.Clean
---

---
field: Microsoft.VisualStudio.Project.MsBuildTarget.ResolveAssemblyReferences
---

---
field: Microsoft.VisualStudio.Project.MsBuildTarget.Rebuild
---

---
field: Microsoft.VisualStudio.Project.MsBuildTarget.ResolveComReferences
---

---
field: Microsoft.VisualStudio.Project.MsBuildTarget.Build
---

---
class: Microsoft.VisualStudio.Project.ReplaceBetweenPairToken
---

---
property: Microsoft.VisualStudio.Project.ReplaceBetweenPairToken.TokenReplacement
---

---
property: Microsoft.VisualStudio.Project.ReplaceBetweenPairToken.TokenEnd
---

---
property: Microsoft.VisualStudio.Project.ReplaceBetweenPairToken.TokenStart
---

---
property: Microsoft.VisualStudio.Project.ReplaceBetweenPairToken.TokenIdentifier
---

---
method: Microsoft.VisualStudio.Project.ReplaceBetweenPairToken.#ctor(System.String,System.String,System.String,System.String)
---

---
class: Microsoft.VisualStudio.Project.ProjectConfigProperties
---

---
property: Microsoft.VisualStudio.Project.ProjectConfigProperties.OutputPath
---

---
method: Microsoft.VisualStudio.Project.ProjectConfigProperties.#ctor(Microsoft.VisualStudio.Project.ProjectConfig)
---

---
class: Microsoft.VisualStudio.Project.ProjectElement
---

---
property: Microsoft.VisualStudio.Project.ProjectElement.ItemName
---

---
method: Microsoft.VisualStudio.Project.ProjectElement.Rename(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectElement.GetEvaluatedMetadata(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectElement.GetMetadataAndThrow(System.String,System.Exception)
---

---
method: Microsoft.VisualStudio.Project.ProjectElement.Equals(System.Object)
---

---
method: Microsoft.VisualStudio.Project.ProjectElement.RefreshProperties
---

---
method: Microsoft.VisualStudio.Project.ProjectElement.GetFullPathForElement
---

---
method: Microsoft.VisualStudio.Project.ProjectElement.GetMetadata(System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectElement.GetHashCode
---

---
method: Microsoft.VisualStudio.Project.ProjectElement.RemoveFromProjectFile
---

---
method: Microsoft.VisualStudio.Project.ProjectElement.SetMetadata(System.String,System.String)
---

---
class: Microsoft.VisualStudio.Project.BeforeProjectFileClosedEventArgs
---

---
class: Microsoft.VisualStudio.Project.ProjectReferenceNode
---

---
property: Microsoft.VisualStudio.Project.ProjectReferenceNode.Caption
---

---
property: Microsoft.VisualStudio.Project.ProjectReferenceNode.Url
---

---
method: Microsoft.VisualStudio.Project.ProjectReferenceNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ProjectReferenceNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,System.String,System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.ProjectReferenceNode.BindReferenceData
---

---
method: Microsoft.VisualStudio.Project.ProjectReferenceNode.CanAddReference(Microsoft.VisualStudio.Project.ReferenceNode.CannotAddReferenceErrorMessage@)
---

---
method: Microsoft.VisualStudio.Project.ProjectReferenceNode.CanShowDefaultIcon
---

---
method: Microsoft.VisualStudio.Project.ProjectReferenceNode.Remove(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ProjectReferenceNode.AddReference
---

---
method: Microsoft.VisualStudio.Project.ProjectReferenceNode.CreatePropertiesObject
---

---
class: Microsoft.VisualStudio.Project.ImageHandler
---

---
property: Microsoft.VisualStudio.Project.ImageHandler.ImageList
---

---
method: Microsoft.VisualStudio.Project.ImageHandler.#ctor(System.Windows.Forms.ImageList)
---

---
method: Microsoft.VisualStudio.Project.ImageHandler.#ctor
---

---
method: Microsoft.VisualStudio.Project.ImageHandler.#ctor(System.IO.Stream)
---

---
method: Microsoft.VisualStudio.Project.ImageHandler.GetIconHandle(System.Int32)
---

---
method: Microsoft.VisualStudio.Project.ImageHandler.AddImage(System.Drawing.Image)
---

---
method: Microsoft.VisualStudio.Project.ImageHandler.Dispose
---

---
method: Microsoft.VisualStudio.Project.ImageHandler.Close
---

---
class: Microsoft.VisualStudio.Project.TokenProcessor
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.#ctor
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.DeleteTokens(System.String@,Microsoft.VisualStudio.Project.DeleteToken)
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.GuidToForm1(System.Guid)
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.ReplaceBetweenTokens(System.String@,Microsoft.VisualStudio.Project.ReplaceBetweenPairToken)
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.AddReplace(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.IsValidIdentifierStartChar(System.Char)
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.IsValidIdentifierChar(System.Char)
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.UntokenFile(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.ReplaceTokens(System.String@,Microsoft.VisualStudio.Project.ReplacePairToken)
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.AddDelete(System.String)
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.AddReplaceBetween(System.String,System.String,System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.Reset
---

---
method: Microsoft.VisualStudio.Project.TokenProcessor.GetFileNamespace(System.String,Microsoft.VisualStudio.Project.ProjectNode)
---

---
class: Microsoft.VisualStudio.Project.NestedProjectNode
---

---
property: Microsoft.VisualStudio.Project.NestedProjectNode.ItemTypeGuid
---

---
property: Microsoft.VisualStudio.Project.NestedProjectNode.VirtualProjectFlags
---

---
property: Microsoft.VisualStudio.Project.NestedProjectNode.CanExecuteCommand
---

---
property: Microsoft.VisualStudio.Project.NestedProjectNode.IsDisposed
---

---
property: Microsoft.VisualStudio.Project.NestedProjectNode.Url
---

---
property: Microsoft.VisualStudio.Project.NestedProjectNode.Caption
---

---
property: Microsoft.VisualStudio.Project.NestedProjectNode.SortPriority
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.#ctor
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.OnRequestEdit(System.Int32)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.CreateProjectDirectory
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.Close
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.IsItemDirty(System.UInt32,System.IntPtr,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.Dispose(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.GetEditLabel
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.SetEditLabel(System.String)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.UnlockRDTEntry
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.RenameNestedProjectInParentProject(System.String)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.OnChanged(System.Int32)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.GetProperty(System.Int32)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.CanDeleteItem(Microsoft.VisualStudio.Shell.Interop.__VSDELETEITEMOPERATION)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.SaveItem(Microsoft.VisualStudio.Shell.Interop.VSSAVEFLAGS,System.String,System.UInt32,System.IntPtr,System.Int32@)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.SaveNestedProjectItemInProjectFile(System.String)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.Init(System.String,System.String,System.String,Microsoft.VisualStudio.Shell.Interop.__VSCREATEPROJFLAGS)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.LockRDTEntry
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.GetIconHandle(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.GetMkDocument
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.GetGuidProperty(System.Int32,System.Guid@)
---

---
method: Microsoft.VisualStudio.Project.NestedProjectNode.GetAutomationObject
---

---
class: Microsoft.VisualStudio.Project.FolderNode
---

---
property: Microsoft.VisualStudio.Project.FolderNode.StateIconIndex
---

---
property: Microsoft.VisualStudio.Project.FolderNode.Caption
---

---
property: Microsoft.VisualStudio.Project.FolderNode.SortPriority
---

---
property: Microsoft.VisualStudio.Project.FolderNode.Url
---

---
property: Microsoft.VisualStudio.Project.FolderNode.ItemTypeGuid
---

---
property: Microsoft.VisualStudio.Project.FolderNode.MenuCommandId
---

---
method: Microsoft.VisualStudio.Project.FolderNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,System.String,Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.FolderNode.CreateDirectory
---

---
method: Microsoft.VisualStudio.Project.FolderNode.QueryStatusOnNode(System.Guid,System.UInt32,System.IntPtr,Microsoft.VisualStudio.Project.QueryStatusResult@)
---

---
method: Microsoft.VisualStudio.Project.FolderNode.DeleteFolder(System.String)
---

---
method: Microsoft.VisualStudio.Project.FolderNode.CreatePropertiesObject
---

---
method: Microsoft.VisualStudio.Project.FolderNode.GetIconHandle(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.FolderNode.CanDeleteItem(Microsoft.VisualStudio.Shell.Interop.__VSDELETEITEMOPERATION)
---

---
method: Microsoft.VisualStudio.Project.FolderNode.GetAutomationObject
---

---
method: Microsoft.VisualStudio.Project.FolderNode.RenameDirectory(System.String)
---

---
method: Microsoft.VisualStudio.Project.FolderNode.SetEditLabel(System.String)
---

---
method: Microsoft.VisualStudio.Project.FolderNode.GetMkDocument
---

---
method: Microsoft.VisualStudio.Project.FolderNode.CreateDirectory(System.String)
---

---
class: Microsoft.VisualStudio.Project.ProjectFileConstants
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.AssemblyName
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Generator
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Folder
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.ProjectTypeGuids
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.SubProject
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.CustomToolNamespace
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Lcid
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.TypeGuid
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.RootNamespace
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.EmbeddedResource
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.EmbedInteropTypes
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Page
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Reference
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.SccProvider
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.SccAuxPath
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Include
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Content
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Resource
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.ProjectGuid
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.VersionMajor
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Isolated
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.VisualStudio
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.LinkedIntoProjectAt
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.OutputType
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Project
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.AvailablePlatforms
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.ApplicationDefinition
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.SccProjectName
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Guid
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.ResolvedProjectReferencePaths
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Name
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Platform
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.WebReference
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.COMReference
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Configuration
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Link
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.FinalOutputPath
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.WebReferenceFolder
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.SubType
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.InstanceGuid
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.ReferencePath
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.BuildingInsideVisualStudio
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Private
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.FlavorProperties
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.DependentUpon
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.ProjectReference
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.BuildVerbosity
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.WrapperTool
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.HintPath
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.User
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Compile
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.VersionMinor
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.SccLocalPath
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.None
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.Template
---

---
field: Microsoft.VisualStudio.Project.ProjectFileConstants.BuildAction
---

---
class: Microsoft.VisualStudio.Project.ReferenceNode
---

---
property: Microsoft.VisualStudio.Project.ReferenceNode.Url
---

---
property: Microsoft.VisualStudio.Project.ReferenceNode.Caption
---

---
property: Microsoft.VisualStudio.Project.ReferenceNode.MenuCommandId
---

---
property: Microsoft.VisualStudio.Project.ReferenceNode.ItemTypeGuid
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode)
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.#ctor(Microsoft.VisualStudio.Project.ProjectNode,Microsoft.VisualStudio.Project.ProjectElement)
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.ExecCommandOnNode(System.Guid,System.UInt32,System.UInt32,System.IntPtr,System.IntPtr)
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.GetIconHandle(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.ResolveReference
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.CanAddReference(Microsoft.VisualStudio.Project.ReferenceNode.CannotAddReferenceErrorMessage@)
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.CreatePropertiesObject
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.CanDeleteItem(Microsoft.VisualStudio.Shell.Interop.__VSDELETEITEMOPERATION)
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.ExcludeFromProject
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.IsAlreadyAdded
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.QueryStatusOnNode(System.Guid,System.UInt32,System.IntPtr,Microsoft.VisualStudio.Project.QueryStatusResult@)
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.GetMkDocument
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.AddReference
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.BindReferenceData
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.GetAutomationObject
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.ShowObjectBrowser
---

---
method: Microsoft.VisualStudio.Project.ReferenceNode.GetEditLabel
---

---
class: Microsoft.VisualStudio.Project.Utilities
---

---
method: Microsoft.VisualStudio.Project.Utilities.GetMsBuildPath(System.IServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.Utilities.ContainsInvalidFileNameChars(System.String)
---

---
method: Microsoft.VisualStudio.Project.Utilities.GetMsBuildPath(System.IServiceProvider,System.String)
---

---
method: Microsoft.VisualStudio.Project.Utilities.SetStringValueFromConvertedEnum``1(``0,System.Globalization.CultureInfo)
---

---
method: Microsoft.VisualStudio.Project.Utilities.IsInAutomationFunction(System.IServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.Utilities.IsFileNameInvalid(System.String)
---

---
method: Microsoft.VisualStudio.Project.Utilities.CreateSemicolonDelimitedListOfStringFromGuids(System.Guid[])
---

---
method: Microsoft.VisualStudio.Project.Utilities.GetImageList(System.Object)
---

---
method: Microsoft.VisualStudio.Project.Utilities.RecursivelyCopyDirectory(System.String,System.String)
---

---
method: Microsoft.VisualStudio.Project.Utilities.GetImageList(System.IO.Stream)
---

---
method: Microsoft.VisualStudio.Project.Utilities.IsVisualStudioInDesignMode(System.IServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.Utilities.ValidateFileName(System.IServiceProvider,System.String)
---

---
method: Microsoft.VisualStudio.Project.Utilities.CreateCALPOLESTR(System.Collections.Generic.IList{System.String})
---

---
method: Microsoft.VisualStudio.Project.Utilities.CreateCADWORD(System.Collections.Generic.IList{Microsoft.VisualStudio.Shell.Interop.tagVsSccFilesFlags})
---

---
method: Microsoft.VisualStudio.Project.Utilities.ConvertToType``1(``0,System.Type,System.Globalization.CultureInfo)
---

---
method: Microsoft.VisualStudio.Project.Utilities.ConvertFromType``1(System.String,System.Globalization.CultureInfo)
---

---
method: Microsoft.VisualStudio.Project.Utilities.IsSameComObject(System.Object,System.Object)
---

---
method: Microsoft.VisualStudio.Project.Utilities.GuidsArrayFromSemicolonDelimitedStringOfGuids(System.String)
---

---
class: Microsoft.VisualStudio.Project.SelectionListener
---

---
property: Microsoft.VisualStudio.Project.SelectionListener.SelectionMonitor
---

---
property: Microsoft.VisualStudio.Project.SelectionListener.ServiceProvider
---

---
property: Microsoft.VisualStudio.Project.SelectionListener.EventsCookie
---

---
method: Microsoft.VisualStudio.Project.SelectionListener.#ctor(Microsoft.VisualStudio.Shell.ServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.SelectionListener.Dispose(System.Boolean)
---

---
method: Microsoft.VisualStudio.Project.SelectionListener.Dispose
---

---
method: Microsoft.VisualStudio.Project.SelectionListener.OnSelectionChanged(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.UInt32,Microsoft.VisualStudio.Shell.Interop.IVsMultiItemSelect,Microsoft.VisualStudio.Shell.Interop.ISelectionContainer,Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.UInt32,Microsoft.VisualStudio.Shell.Interop.IVsMultiItemSelect,Microsoft.VisualStudio.Shell.Interop.ISelectionContainer)
---

---
method: Microsoft.VisualStudio.Project.SelectionListener.Init
---

---
method: Microsoft.VisualStudio.Project.SelectionListener.OnElementValueChanged(System.UInt32,System.Object,System.Object)
---

---
method: Microsoft.VisualStudio.Project.SelectionListener.OnCmdUIContextChanged(System.UInt32,System.Int32)
---

---
class: Microsoft.VisualStudio.Project.ProjectOptions
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.EmitManifest
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.ModuleKind
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.FileAlignment
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.Win32Icon
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.AdditionalSearchPaths
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.UserLocaleId
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.AllowUnsafeCode
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.CheckedArithmetic
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.BaseAddress
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.PdbOnly
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.XmlDocFileName
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.HeuristicReferenceResolution
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.NoStandardLibrary
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.EncodeOutputInUtf8
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.DisplayCommandLineHelp
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.DefinedPreprocessorSymbols
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.ReferencedModules
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.RecursiveWildcard
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.Config
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.TargetFrameworkMoniker
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.CompileAndExecute
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.Optimize
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.SuppressedWarnings
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.SuppressLogo
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.CodePage
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.RootNamespace
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.BugReportFileName
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.IncrementalCompile
---

---
property: Microsoft.VisualStudio.Project.ProjectOptions.FullyQualifyPaths
---

---
method: Microsoft.VisualStudio.Project.ProjectOptions.#ctor
---

---
method: Microsoft.VisualStudio.Project.ProjectOptions.GetOptionHelp
---

---
class: Microsoft.VisualStudio.Project.DeleteToken
---

---
property: Microsoft.VisualStudio.Project.DeleteToken.StringToDelete
---

---
method: Microsoft.VisualStudio.Project.DeleteToken.#ctor(System.String)
---

---
class: Microsoft.VisualStudio.Project.VsMenus
---

---
field: Microsoft.VisualStudio.Project.VsMenus.guidStandardCommandSet2K
---

---
field: Microsoft.VisualStudio.Project.VsMenus.IDM_VS_CTXT_REFERENCE
---

---
field: Microsoft.VisualStudio.Project.VsMenus.guidVSUISet
---

---
field: Microsoft.VisualStudio.Project.VsMenus.IDM_VS_CTXT_FOLDERNODE
---

---
field: Microsoft.VisualStudio.Project.VsMenus.IDM_VS_CTXT_NOCOMMANDS
---

---
field: Microsoft.VisualStudio.Project.VsMenus.IDM_VS_CTXT_REFERENCEROOT
---

---
field: Microsoft.VisualStudio.Project.VsMenus.guidVsVbaPkg
---

---
field: Microsoft.VisualStudio.Project.VsMenus.IDM_VS_CTXT_PROJNODE
---

---
field: Microsoft.VisualStudio.Project.VsMenus.IDM_VS_CTXT_ITEMNODE
---

---
field: Microsoft.VisualStudio.Project.VsMenus.IDM_VS_CTXT_XPROJ_MULTIITEM
---

---
field: Microsoft.VisualStudio.Project.VsMenus.guidStandardCommandSet97
---

---
field: Microsoft.VisualStudio.Project.VsMenus.guidVsUIHierarchyWindowCmds
---

---
field: Microsoft.VisualStudio.Project.VsMenus.IDM_VS_CTXT_CODEWIN
---

---
field: Microsoft.VisualStudio.Project.VsMenus.VSCmdOptQueryParameterList
---

---
field: Microsoft.VisualStudio.Project.VsMenus.IDM_VS_CTXT_XPROJ_PROJITEM
---

---
field: Microsoft.VisualStudio.Project.VsMenus.guidSHLMainMenu
---

---
class: Microsoft.VisualStudio.Project.SingleFileGeneratorFactory
---

---
property: Microsoft.VisualStudio.Project.SingleFileGeneratorFactory.ServiceProvider
---

---
property: Microsoft.VisualStudio.Project.SingleFileGeneratorFactory.ProjectGuid
---

---
method: Microsoft.VisualStudio.Project.SingleFileGeneratorFactory.#ctor(System.Guid,System.IServiceProvider)
---

---
method: Microsoft.VisualStudio.Project.SingleFileGeneratorFactory.CreateGeneratorInstance(System.String,System.Int32@,System.Int32@,System.Int32@,Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator@)
---

---
method: Microsoft.VisualStudio.Project.SingleFileGeneratorFactory.GetDefaultGenerator(System.String,System.String@)
---

---
method: Microsoft.VisualStudio.Project.SingleFileGeneratorFactory.GetGeneratorInformation(System.String,System.Int32@,System.Int32@,System.Int32@,System.Guid@)
---

---
class: Microsoft.VisualStudio.Project.IReferenceContainerProvider
---

---
method: Microsoft.VisualStudio.Project.IReferenceContainerProvider.GetReferenceContainer
---

---
class: Microsoft.VisualStudio.Project.IReferenceContainer
---

---
method: Microsoft.VisualStudio.Project.IReferenceContainer.LoadReferencesFromBuildProject(Microsoft.Build.Evaluation.Project)
---

---
method: Microsoft.VisualStudio.Project.IReferenceContainer.AddReferenceFromSelectorData(Microsoft.VisualStudio.Shell.Interop.VSCOMPONENTSELECTORDATA,System.String)
---

---
method: Microsoft.VisualStudio.Project.IReferenceContainer.EnumReferences
---

---
class: Microsoft.VisualStudio.Project.IProjectEventsProvider
---

---
property: Microsoft.VisualStudio.Project.IProjectEventsProvider.ProjectEventsProvider
---

---
class: Microsoft.VisualStudio.Project.IBuildDependencyUpdate
---

---
property: Microsoft.VisualStudio.Project.IBuildDependencyUpdate.BuildDependencies
---

---
method: Microsoft.VisualStudio.Project.IBuildDependencyUpdate.RemoveBuildDependency(Microsoft.VisualStudio.Shell.Interop.IVsBuildDependency)
---

---
method: Microsoft.VisualStudio.Project.IBuildDependencyUpdate.AddBuildDependency(Microsoft.VisualStudio.Shell.Interop.IVsBuildDependency)
---

---
class: Microsoft.VisualStudio.Project.IProjectConfigProperties
---

---
property: Microsoft.VisualStudio.Project.IProjectConfigProperties.OutputPath
---

---
class: Microsoft.VisualStudio.Project.IProjectEventsListener
---

---
property: Microsoft.VisualStudio.Project.IProjectEventsListener.IsProjectEventsListener
---

---
class: Microsoft.VisualStudio.Project.IBuildDependencyOnProjectContainer
---

---
property: Microsoft.VisualStudio.Project.IBuildDependencyOnProjectContainer.BuildNestedProjectsOnBuild
---

---
method: Microsoft.VisualStudio.Project.IBuildDependencyOnProjectContainer.EnumNestedHierachiesForBuildDependency
---

---
class: Microsoft.VisualStudio.Project.ISingleFileGenerator
---

---
method: Microsoft.VisualStudio.Project.ISingleFileGenerator.RunGenerator(System.String)
---

---
class: Microsoft.VisualStudio.Project.IProjectEvents
---

---
event: Microsoft.VisualStudio.Project.IProjectEvents.AfterProjectFileOpened
---

---
event: Microsoft.VisualStudio.Project.IProjectEvents.BeforeProjectFileClosed
---

---
class: Microsoft.VisualStudio.Project.WrapperToolAttributeValue
---

---
class: Microsoft.VisualStudio.Project.TokenReplaceType
---

---
class: Microsoft.VisualStudio.Project.PropPageStatus
---

---
class: Microsoft.VisualStudio.Project.MSBuildResult
---

---
class: Microsoft.VisualStudio.Project.CommandOrigin
---

---
class: Microsoft.VisualStudio.Project.BuildAction
---

---
class: Microsoft.VisualStudio.Project.GlobalProperty
---

---
class: Microsoft.VisualStudio.Project.OutputType
---

---
class: Microsoft.VisualStudio.Project.QueryStatusResult
---

---
class: Microsoft.VisualStudio.Project.HierarchyAddType
---

---
class: Microsoft.VisualStudio.Project.BuildKind
---

---
class: Microsoft.VisualStudio.Project.ModuleKindFlags
---

---
class: Microsoft.VisualStudio.Project.DebugMode
---

---
class: Microsoft.VisualStudio.Project.UIHierarchyElement
---

---
class: Microsoft.VisualStudio.Project.WindowFrameShowAction
---


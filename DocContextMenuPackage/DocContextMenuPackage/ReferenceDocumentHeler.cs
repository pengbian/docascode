using EnvDTE;
using Microsoft.VisualStudio.Project.Automation;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using VSLangProj;

namespace Company.MyPackage
{
    class ReferenceDocumentHeler
    {
        private Project _project;
        private Project _docProject;
        public ProjectItem _docsFolder;
        public ProjectItem _projectDocFolder;

        public ReferenceDocumentHeler(Project project, Project docProject)
        {
            _project = project;
            _docProject = docProject;
            _projectDocFolder = getProjectDocFolder(project, docProject);
        }

        private ProjectItem getProjectDocFolder(Project project, Project docProject)
        {
            bool flag = false;
            for (int index = 1; index <= docProject.ProjectItems.Count; index++)
            {
                ProjectItem item = docProject.ProjectItems.Item(index);
                if (item.Name.Equals("Docs"))
                {
                    _docsFolder = docProject.ProjectItems.Item("Docs");
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                _docsFolder = docProject.ProjectItems.AddFolder("Docs");
            }

            if (!Directory.Exists(_docsFolder.Properties.Item("FullPath").Value.ToString() + _project.Name))
            {
                Directory.CreateDirectory(_docsFolder.Properties.Item("FullPath").Value.ToString() + _project.Name);
            }

            for (int index = 1; index <= _docsFolder.ProjectItems.Count; index++)
            {
                ProjectItem item = _docsFolder.ProjectItems.Item(index);
                if (item.Name.Equals(project.Name))
                {
                    return _docsFolder.ProjectItems.Item(index);
                }
            }

            return _docsFolder.ProjectItems.AddFolder(project.Name);
        }

        public void extractReference()
        {
            var docProject = (Microsoft.VisualStudio.Project.ProjectNode)(_docProject.Object);
            var containerNode = new Microsoft.VisualStudio.Project.ReferenceContainerNode(docProject);
            var references = new OAReferences(containerNode);
            references.AddProject(_project);
        }

    }
}

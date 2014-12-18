using EnvDTE;
using Microsoft.VisualStudio.Project.Automation;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using VSLangProj;

namespace Company.DocContextMenuPackage
{
    class ReferenceDocumentHeler
    {
        private Project _project;
        private Project _docProject;

        public ReferenceDocumentHeler(Project project, Project docProject)
        {
            _project = project;
            _docProject = docProject;
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

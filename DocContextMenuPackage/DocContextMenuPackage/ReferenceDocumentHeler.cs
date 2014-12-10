using EnvDTE;
using System;

namespace Company.MyPackage
{
    class ReferenceDocumentHeler
    {
        private Project _project;
        private Project _docProject;
        private ProjectItem _projectDocFolder;

        public ReferenceDocumentHeler(Project project, Project docProject)
        {
            _project = project;
            _docProject = docProject;
            _projectDocFolder = getProjectDocFolder(project, docProject);
        }

        private ProjectItem getProjectDocFolder(Project project, Project docProject)
        {
            ProjectItem docsfolder = null;
            bool flag = false;
            for (int index = 1; index <= docProject.ProjectItems.Count; index++)
            {
                ProjectItem item = docProject.ProjectItems.Item(index);
                if (item.Name.Equals("Docs"))
                {
                    docsfolder = docProject.ProjectItems.Item("Docs");
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                docsfolder = docProject.ProjectItems.AddFolder("Docs");
            }
            for (int index = 1; index <= docsfolder.ProjectItems.Count; index++)
            {
                ProjectItem item = docsfolder.ProjectItems.Item(index);
                if (item.Name.Equals(project.Name))
                {
                    return docsfolder.ProjectItems.Item(index);
                }
            }

            return docsfolder.ProjectItems.AddFolder(project.Name);
        }

        public void extractReference()
        {

        }
    }
}

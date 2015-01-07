using EnvDTE;
using EnvDTE80;
using System.IO;
using System.Threading.Tasks;

namespace Company.DocContextMenuPackage
{
    class AddDocProjectOperation
    {
        private DTE _dte;
        private Project _selectedProject;
        private Project _docProject;
        private ProjectItem _docsFolder;
        private ProjectItem _projectDocFolder;

        public AddDocProjectOperation(DTE dte, Project selectedProject)
        {
            _selectedProject = selectedProject;
            _dte = dte;
            _docProject = getDocProject();
            _projectDocFolder = getProjectDocFolder(_selectedProject, _docProject);
        }

        private Project getDocProject()
        {
            for (int i = 1; i <= _dte.Solution.Projects.Count; i++)
            {
                Project project = (Project)_dte.Solution.Projects.Item(i);

                if (project.FileName.EndsWith(".docproj"))
                {
                    return project;
                }
            }
            return createDocProject();
        }

        private Project createDocProject()
        {
            Solution sln = _dte.Solution;
            string templatePath = ((Solution2)sln).GetProjectTemplate(@"DocProject.zip", "DocProject");
            string projectPath = System.IO.Path.GetDirectoryName(sln.FullName) + "\\DocProject";
            Project docProject = sln.AddFromTemplate(templatePath, projectPath, "DocProject", false);

            if (docProject != null)
            {
                docProject.ProjectItems.AddFolder("Docs");
            }

            for (int i = 1; i <= _dte.Solution.Projects.Count; i++)
            {
                Project project = (Project)_dte.Solution.Projects.Item(i);

                if (project.FileName.EndsWith("DocProject.docproj"))
                {
                    return project;
                }
            }
            return docProject;
        }

        private ProjectItem getProjectDocFolder(Project selectedProject, Project docProject)
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
            string projectDocFolderPath = _docsFolder.Properties.Item("FullPath").Value.ToString() + selectedProject.Name;
            if (!Directory.Exists(projectDocFolderPath))
            {
                Directory.CreateDirectory(projectDocFolderPath);
            }

            for (int index = 1; index <= _docsFolder.ProjectItems.Count; index++)
            {
                ProjectItem item = _docsFolder.ProjectItems.Item(index);
                if (item.Name.Equals(selectedProject.Name))
                {
                    return _docsFolder.ProjectItems.Item(index);
                }
            }

            return _docsFolder.ProjectItems.AddFolder(selectedProject.Name);
        }

        public async Task operate()
        {
            ReferenceDocumentHeler helper = new ReferenceDocumentHeler(_selectedProject, _docProject);
            helper.extractReference();

            // MDGenerateHelper mdHelper = new MDGenerateHelper(_dte, _selectedProject, _docsFolder, _projectDocFolder);
            // await mdHelper.GenMarkDownFileAsync(100000);
        }
    }
}

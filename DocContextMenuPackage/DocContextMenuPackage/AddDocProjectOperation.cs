using EnvDTE;
using EnvDTE80;
using System.Windows.Forms;

namespace Company.MyPackage
{
    class AddDocProjectOperation
    {
        private Project _selectedProject;
        private DTE _dte;
        private Project _docProject;

        public AddDocProjectOperation(DTE dte, Project selectedProject)
        {
            _selectedProject = selectedProject;
            _dte = dte;
            _docProject = getDocProject();
        }

        private Project getDocProject()
        {
             for(int i = 1; i <= _dte.Solution.Projects.Count; i++)
             {
                 Project project = _dte.Solution.Projects.Item(i) as Project;

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

            if(docProject != null)
            {
                docProject.ProjectItems.AddFolder("Docs");
            }

            for (int i = 1; i <= _dte.Solution.Projects.Count; i++)
            {
                Project project = _dte.Solution.Projects.Item(i) as Project;

                if (project.FileName.EndsWith("DocProject.docproj"))
                {
                    return project;
                }
            }
            return docProject;
        }

        public void operate()
        {
            ReferenceDocumentHeler helper = new ReferenceDocumentHeler(_selectedProject, _docProject);
            helper.extractReference();
        }
    }
}

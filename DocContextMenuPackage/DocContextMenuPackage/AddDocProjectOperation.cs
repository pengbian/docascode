using EnvDTE;
using EnvDTE80;
using System;
using System.Diagnostics;
using System.IO;

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

        private void GenDocMetadata(ProjectItem docsFolder, ProjectItem projectDocsFolder)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = System.IO.Directory.GetCurrentDirectory() + @"\..\..\..\..\GenDoc\GenDocMetadata\bin\Debug\GenDocMetadata.exe";
            //startInfo.FileName = System.IO.Path.GetDirectoryName(_dte.Solution.Projects.i)
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            string docMetadataPath = System.IO.Path.GetDirectoryName(docsFolder.Properties.Item("FullPath").Value.ToString());
            startInfo.Arguments = string.Format("\"{0}\" /o:\"{1}\" /t:\"Markdown\"", _dte.Solution.FullName, docMetadataPath);

            try
            {
                using (System.Diagnostics.Process exeProcess = System.Diagnostics.Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();

                    foreach (string file in Directory.GetFiles(docMetadataPath + "\\" + _selectedProject.Name))
                    {
                        File.Delete(file);
                    }
                    for (int i = 1; i <= projectDocsFolder.ProjectItems.Count;)
                    {
                        projectDocsFolder.ProjectItems.Item(i).Remove();

                    }
                    foreach (string dir in Directory.GetDirectories(docMetadataPath + "\\mdtoc\\" + _selectedProject.Name))
                    {
                        foreach (string file in Directory.GetFiles(dir))
                        {
                            projectDocsFolder.ProjectItems.AddFromFile(file);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void operate()
        {
            ReferenceDocumentHeler helper = new ReferenceDocumentHeler(_selectedProject, _docProject);
            helper.extractReference();
            GenDocMetadata(helper._docsFolder, helper._projectDocFolder);
        }
    }
}

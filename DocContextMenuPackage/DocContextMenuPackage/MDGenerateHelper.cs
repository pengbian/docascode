using EnvDTE;
using System.IO;
using System.Threading.Tasks;
using DocAsCode.Utility;

namespace Company.DocContextMenuPackage
{
    class MDGenerateHelper
    {
        private DTE _dte;
        private Project _project;
        private ProjectItem _docsFolder;
        private ProjectItem _projectDocFolder;

        public MDGenerateHelper(DTE dte, Project project, ProjectItem docsFolder, ProjectItem projectDocFolder)
        {
            _dte = dte;
            _project = project;
            _docsFolder = docsFolder;
            _projectDocFolder = projectDocFolder;
        }

        public async Task GenMarkDownFileAsync(int timeoutInMilliseconds)
        {
            string executorPath = typeof(DocAsCode.GenDocMetadata.Program).Assembly.Location;
            string workingDirectory = Path.GetDirectoryName(executorPath);
            string projectName = Path.GetFileName(_project.FullName);
            string docMetadataPath = Path.GetDirectoryName(_docsFolder.Properties.Item("FullPath").Value.ToString());
            string arguments = string.Format("\"{0}\" /o:\"{1}\" /p:\"{2}\" /t:\"Markdown\"", _dte.Solution.FullName, docMetadataPath, projectName);

            var processingDetail = await ProcessUtility.ExecuteWin32ProcessAsync(executorPath, arguments, workingDirectory, 100000);

            if (processingDetail.ExitCode != 0)
            {
                throw new System.ApplicationException(string.Format("Error executing {0} {1} : {2}", executorPath, arguments, processingDetail.StandardOutput + processingDetail.StandardError));
            }

            string projectDocfolderPath = Path.GetDirectoryName(_projectDocFolder.Properties.Item("FullPath").Value.ToString());
            foreach (string file in Directory.GetFiles(projectDocfolderPath))
            {
                File.Delete(file);
            }

            int itemCount = _projectDocFolder.ProjectItems.Count;
            for (int i = itemCount; i >= 1; i--)
            {
                _projectDocFolder.ProjectItems.Item(i).Remove();
            }

            string tempMDPath = docMetadataPath + "\\mdtoc\\" + _project.Properties.Item("AssemblyName").Value.ToString();
            foreach (string dir in Directory.GetDirectories(tempMDPath))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    _projectDocFolder.ProjectItems.AddFromFile(file);
                }
            }
        }

    }
}
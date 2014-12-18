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
            string projectName = extractProjectName(_project);
            string docMetadataPath = Path.GetDirectoryName(_docsFolder.Properties.Item("FullPath").Value.ToString());
            string arguments = string.Format("\"{0}\" /o:\"{1}\" /p:\"{2}\" /t:\"Markdown\"", _dte.Solution.FullName, docMetadataPath, projectName);

            await ProcessUtility.ExecuteWin32ProcessAsync(executorPath, arguments, workingDirectory, 100000);
            foreach (string file in Directory.GetFiles(docMetadataPath + "\\" + _project.Name))
            {
               File.Delete(file);
            }

            int itemCount = _projectDocFolder.ProjectItems.Count;
            for (int i=itemCount; i>=1; i--)
            {
               _projectDocFolder.ProjectItems.Item(i).Remove();
            }
            foreach (string dir in Directory.GetDirectories(docMetadataPath + "\\mdtoc\\" + _project.Name))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    _projectDocFolder.ProjectItems.AddFromFile(file);
                }
            }
      }

       private string extractProjectName(Project project)
       {
           string fullName = project.FullName;
           int index = fullName.LastIndexOf("\\");
           return fullName.Substring(index + 1);
       }
   }
}
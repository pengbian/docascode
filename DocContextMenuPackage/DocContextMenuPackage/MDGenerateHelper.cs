using EnvDTE;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

        public async void GenMarkDownFile(int timeoutInMilliseconds)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            string exeFilePath = typeof(DocAsCode.GenDocMetadata.Program).Assembly.Location;
            string projectName = extractProjectName(_project);
            string docMetadataPath = System.IO.Path.GetDirectoryName(_docsFolder.Properties.Item("FullPath").Value.ToString());

            startInfo.FileName = exeFilePath;
            startInfo.Arguments = string.Format("\"{0}\" /o:\"{1}\" /p:\"{2}\" /t:\"Markdown\"", _dte.Solution.FullName, docMetadataPath, projectName);
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;

            var process = new System.Diagnostics.Process { StartInfo = startInfo };
            using (process)
            {
                var output = new StringBuilder();
                var error = new StringBuilder();
                process.OutputDataReceived += (s, e) => output.AppendLine(e.Data);
                process.ErrorDataReceived += (s, e) => error.AppendLine(e.Data);
                try
                {
                    process.Start();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await WaitForExitAsync(process, timeoutInMilliseconds);
                //await process.WaitForExitAsunc
                foreach (string file in Directory.GetFiles(docMetadataPath + "\\" + _project.Name))
                {
                    File.Delete(file);
                }

                for (int i = 1; i<= _projectDocFolder.ProjectItems.Count;)
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
        }

        public Task WaitForExitAsync(System.Diagnostics.Process process, int timeoutInMilliseconds)
        {
            return Task.Run(
                () =>
                {
                    if (!process.WaitForExit(timeoutInMilliseconds))
                    {
                        string processName = string.Empty;
                        Exception killProcessException = null;
                        try
                        {
                            processName = process.ProcessName;
                            process.Kill();
                        }
                        catch (InvalidOperationException e)
                        {
                            killProcessException = e;
                        }
                        catch (Exception e)
                        {
                            killProcessException = e;
                        }
                        throw new TimeoutException(
                            string.Format(
                                "Executing {0} exceeds {1} milliseconds, aborted. {2}",
                                processName,
                                timeoutInMilliseconds,
                                killProcessException == null ? string.Empty : killProcessException.Message));
                    }
                });
        }

        private string extractProjectName(Project project)
        {
            string fullName = project.FullName;
            int index = fullName.LastIndexOf("\\");
            return fullName.Substring(index + 1);
        }
    }
}
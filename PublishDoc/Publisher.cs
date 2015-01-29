using DocAsCode.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocAsCode.PublishDoc
{
    public class Publisher
    {
        
        public static void PublishToGithub(string mtaFilePath)
        {
            GithubConfigurationForm githubConfigurationForm = new GithubConfigurationForm();
            if(githubConfigurationForm.ShowDialog()==false) return;
            string mtaFile = mtaFilePath;
            string delimitedMdFiles = @"TestData\T_GenDocMetadata.AssemblyDocMetadata.md";
            string templateDirectory = "Templates";
            string remoteGitPath = githubConfigurationForm.remoteGitPath;
            string localGitPath = githubConfigurationForm.localGitPath;
            string publishUrl = githubConfigurationForm.publishUrl;
            string userName = githubConfigurationForm.userName;
            string passWord = githubConfigurationForm.passWord;
            string acessUrl = githubConfigurationForm.acessUrl;
            bool clearLocalGit = githubConfigurationForm.clearLocalGit;
            bool openSite = githubConfigurationForm.openSite;
            //Clear the local git directoryw33
            if (clearLocalGit && Directory.Exists(localGitPath))
            {
                ClearDirectorys(localGitPath);
            }
            //Create local git directory
            if (!Directory.Exists(localGitPath))
            {
                Directory.CreateDirectory(localGitPath);
            }
            //Generate the htmls
            string executorPath = typeof(DocAsCode.MergeDoc.Program).Assembly.Location;
            string workingDirectory = Path.GetDirectoryName(executorPath);
            string arguments = string.Format("\"{0}\" /outputDirectory:\"{1}\" /templateDirectory:\"{2}\" /delimitedMdFiles:\"{3}\" /publishBaseUrl:\"{4}\"",
                                            mtaFile, localGitPath, templateDirectory, delimitedMdFiles, publishUrl);
            ProcessDetail processingDetail = null;
            Task.Run(async () =>
            {
                processingDetail = await ProcessUtility.ExecuteWin32ProcessAsync(executorPath, arguments, workingDirectory, 100000);
            }).Wait();
            if (processingDetail.ExitCode != 0)
            {
                throw new System.ApplicationException(string.Format("Error executing {0} {1} : {2}", executorPath, arguments, processingDetail.StandardOutput + processingDetail.StandardError));
            }
            //publish
            GitRepositoryPublisher.PublishToGit(remoteGitPath, localGitPath, publishUrl, userName, passWord);
            //open browser
            if(openSite) System.Diagnostics.Process.Start(acessUrl);
        }

        static void ClearDirectorys(string dirPath)
        {
            foreach (string subFileName in Directory.GetFiles(dirPath))
            {
                File.SetAttributes(subFileName, FileAttributes.Normal);
                File.Delete(subFileName);
            }
            foreach (string subDirName in Directory.GetDirectories(dirPath))
            {
                ClearDirectorys(subDirName);
                Directory.Delete(subDirName);
            }
        }
    }
}

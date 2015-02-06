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
        
        public static void PublishToGithub(string filesDirectory)
        {

            // Step 1. Show UI and configure
            GithubConfigurationForm githubConfigurationForm = new GithubConfigurationForm();
            githubConfigurationForm.localGitPath = filesDirectory;
            if (githubConfigurationForm.ShowDialog()==false) return;

            string remoteGitPath = githubConfigurationForm.remoteGitPath;
            string localGitPath = githubConfigurationForm.localGitPath;
            string publishUrl = githubConfigurationForm.publishUrl;
            string userName = githubConfigurationForm.userName;
            string passWord = githubConfigurationForm.passWord;
            string acessUrl = githubConfigurationForm.acessUrl;
            bool clearLocalGit = githubConfigurationForm.clearLocalGit;
            bool openSite = githubConfigurationForm.openSite;

            if (!Directory.Exists(localGitPath))
            {
                return;
            }
            //publish
            GitRepositoryPublisher.PublishToGit(remoteGitPath, localGitPath, userName, passWord);
            //open browser
            if(openSite) System.Diagnostics.Process.Start(acessUrl);
        }
    }
}

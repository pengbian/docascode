using DocAsCode.Utility;
using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Logging;

namespace Company.DocContextMenuPackage
{
    class PublishToGithubOperation
    {
        public static void operate(Project selectedProject)
        {
            Microsoft.Build.Evaluation.ProjectCollection projectCollection = new Microsoft.Build.Evaluation.ProjectCollection();
            projectCollection.RegisterLogger(new ConsoleLogger());
            Microsoft.Build.Evaluation.Project docProject = projectCollection.LoadProject(selectedProject.FullName);
            if (docProject == null)
            {
                docProject = new Microsoft.Build.Evaluation.Project(selectedProject.FullName);
            }
            bool result = docProject.Build(target: "PublishDoc");
        }
    }
}

using DocAsCode.Utility;
using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DocContextMenuPackage
{
    class PublishToGithubOperation
    {
        public static void operate(Project selectedProject)
        {
            //Generate the mtafile
            string executorPath = typeof(DocAsCode.GenDocMetadata.Program).Assembly.Location;
            string workingDirectory = Path.GetDirectoryName(executorPath);
            string projectName = selectedProject.FullName;
            string docMetadataPath = System.Environment.CurrentDirectory + "\\..\\..\\..\\..\\PublishDoc\\bin\\Debug\\testData3";
            string arguments = string.Format("\"{0}\" /o:\"{1}\"", projectName, docMetadataPath);
            ProcessDetail processingDetail = null;
            Task.Run(async () =>
            {
                processingDetail = await ProcessUtility.ExecuteWin32ProcessAsync(executorPath, arguments, workingDirectory, 100000);
            }).Wait();
            if (processingDetail.ExitCode != 0)
            {
                throw new System.ApplicationException(string.Format("Error executing {0} {1} : {2}", executorPath, arguments, processingDetail.StandardOutput + processingDetail.StandardError));
            }

            string mtaFile = docMetadataPath + "\\mta\\" + selectedProject.Name + ".docmta";
            //publish
            //typeof(DocAsCode.PublishDoc.Program).Assembly.Location;
            executorPath = System.Environment.CurrentDirectory + "\\..\\..\\..\\..\\PublishDoc\\bin\\Debug\\PublishDoc.exe";
            workingDirectory = Path.GetDirectoryName(executorPath);
            arguments = string.Format("{0}", mtaFile);
            processingDetail = null;
            Task.Run(async () =>
            {
                processingDetail = await ProcessUtility.ExecuteWin32ProcessAsync(executorPath, arguments, workingDirectory, 100000);
            }).Wait();
            if (processingDetail.ExitCode != 0)
            {
                throw new System.ApplicationException(string.Format("Error executing {0} {1} : {2}", executorPath, arguments, processingDetail.StandardOutput + processingDetail.StandardError));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using EnvDTE;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace DocBuilding
{
    public class DocBuilding : Task
    {
        private string mSBuildProjectDirectory;
        public string MSBuildProjectDirectory
        {
            get { return mSBuildProjectDirectory; }
            set { mSBuildProjectDirectory = value; }
        }

        private string msbuildDirectory;
        public string MsbuildDirectory
        {
            get { return msbuildDirectory; }
            set { msbuildDirectory = value; }
        }

        private string mrefUtilityDirectory;
        public string MrefUtilityDirectory
        {
            get { return mrefUtilityDirectory; }
            set { mrefUtilityDirectory = value; }
        }

        /*
        public override bool Execute()
        {

            bool executeResult = false;
            int totalApiXmlCount = 0;
            int currentApiXmlCount = 0;
            var item = activeProjects.GetEnumerator();
            while (item.MoveNext())
            {
                var project = item.Current as Project;

                var projectItem = project.ProjectItems.GetEnumerator();
                while (projectItem.MoveNext())
                {
                    var folder = item.Current as ProjectItem;
                    if (folder.ToString() == "Docs")
                    {
                        var assemblyFolders = folder.ProjectItems;
                        foreach (var assemblyFolder in assemblyFolders)
                        {
                            string assemblyXMLDirectoryPath = Path.Combine(Path.GetDirectoryName(project.FileName), "Docs", project.Name);
                            if (getCurrentApiXmlCount(assemblyXMLDirectoryPath, out currentApiXmlCount) &&
                                getTotalApiXmlCount(assemblyXMLDirectoryPath, out totalApiXmlCount))
                            {
                                if (currentApiXmlCount == totalApiXmlCount)
                                {
                                    executeMefUtility(project);
                                    executeResult = true;
                                }
                            }
                        }
                    }
                }

            }

            return executeResult;
        }
        */


        public override bool Execute()
        {
            System.Diagnostics.Process mrefUtility = new System.Diagnostics.Process();
            mrefUtility.StartInfo.FileName = MrefUtilityDirectory;

            string projectId = "1";
            string outputPath = "HTML";
            string assemblyName = getAssemblyName(mSBuildProjectDirectory);
            string sourcePath = Path.Combine(mSBuildProjectDirectory,"..",assemblyName, @"bin\Debug");
            string dependencyPath = msbuildDirectory;
            string commentsPath = Path.Combine(mSBuildProjectDirectory, "Docs", assemblyName);
            mrefUtility.StartInfo.Arguments = projectId + " " + outputPath + " " + sourcePath + " " + dependencyPath + " " + commentsPath;

            mrefUtility.Start();
            return true;
        }

        private string getAssemblyName(string mSBuildProjectDirectory)
        {
            DirectoryInfo mSBuildProjectDirectoryInfo = new DirectoryInfo(mSBuildProjectDirectory);
            DirectoryInfo[] assemblyDirectoryInfos = mSBuildProjectDirectoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
            return Path.GetDirectoryName(assemblyDirectoryInfos[0].FullName);
        }

        /*
        private bool getCurrentApiXmlCount(string assemblyXMLDirectoryPath, out int currentApiXmlCount)
        {
            string[] allFilesinAssemblyXMLDirectoryPath = Directory.GetFiles(assemblyXMLDirectoryPath);
            currentApiXmlCount = 0;
            XmlDocument xml = new XmlDocument();
            bool ifSuccess = false;

            foreach (var fileName in allFilesinAssemblyXMLDirectoryPath)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                string fileExtension = Path.GetExtension(fileInfo.FullName);
                if (fileExtension == "xml")
                {
                    xml.Load(fileInfo.FullName);
                    XmlNode root = xml.SelectSingleNode("/codeEntityDocument");
                    if (root != null)
                    {
                        currentApiXmlCount++;
                    }
                    ifSuccess = true;
                }
            }

            return ifSuccess;
        }

        private bool getTotalApiXmlCount(string assemblyXMLDirectoryPath, out int totalApiXmlCount)
        {
            XmlDocument xml = new XmlDocument();
            string apiRecordFile = Path.Combine(assemblyXMLDirectoryPath, "APIRecord");

            if (!File.Exists(apiRecordFile))
            {
                totalApiXmlCount = 0;
                return false;
            }

            xml.Load(apiRecordFile);
            var root = xml.SelectSingleNode("/APIRecords") as XmlElement;
            totalApiXmlCount = Int32.Parse(root.GetAttribute("totalCount"));

            return true;
        }
        */
    }
}
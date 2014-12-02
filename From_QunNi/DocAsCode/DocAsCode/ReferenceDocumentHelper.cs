using EnvDTE;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace DocAsCode
{
    internal class ReferenceDocumentHeler
    {
        private IServiceProvider serviceProvider;
        private DTE dte;
        private EnvDTE.Project activeDocProject;
        //private DeveloperCommentsTransform ddueTransformer;

        public ReferenceDocumentHeler(IServiceProvider sp)
        {
            serviceProvider = sp;
            dte = serviceProvider.GetService(typeof(DTE)) as DTE;
            activeDocProject = GetDocProject(dte);
            //ddueTransformer = InitDdueTransformer();
        }

        /*
        private DeveloperCommentsTransform InitDdueTransformer()
        {
            var transform = new XslCompiledTransform();
            var settings = new XsltSettings(true, true);
            var transformXslPath = getTransformXslPath();

            using (var fs = File.OpenRead(Path.Combine(transformXslPath, "DeveloperCommentsTransform.xsl")))
            {
                transform.Load(XmlReader.Create(fs), settings, new XmlUrlResolver());
            }
            return new DeveloperCommentsTransform(transform);
        }

        private string getTransformXslPath()
        {
            var solutionPath = Directory.GetCurrentDirectory() + @"..\..\..\..\";
            var transformXslPath = solutionPath + @"source\Tansform\";
            if (Directory.Exists(transformXslPath) == false)
            {
                Directory.CreateDirectory(transformXslPath);
            }

            return transformXslPath;
        }
        */

        internal static EnvDTE.Project GetActiveProject(DTE dte)
        {
            EnvDTE.Project activeProject = null;
            activeProject = dte.Solution.Projects.Item(1);
            return activeProject;
        }

        internal static EnvDTE.Project GetDocProject(DTE dte)
        {
            EnvDTE.Project activeDocProject = null;

            int activeProjectsCount = dte.Solution.Projects.Count;
            int index = 1;
            while (index <= activeProjectsCount)
            {
                EnvDTE.Project project = dte.Solution.Projects.Item(index) as EnvDTE.Project;
                if (project.FileName.EndsWith(".docproj"))
                {
                    activeDocProject = project;
                }
                index++;
            }
            return activeDocProject;
        }

        internal bool XMLFileExist(string XMLFileName)
        {
            if (activeDocProject == null)
                return false;

            string activeProjectDirectory = Path.GetDirectoryName(activeDocProject.FileName);
            DirectoryInfo activeProjectDirectoryInfo = new DirectoryInfo(activeProjectDirectory);

            FileInfo[] XMLFileInfo = activeProjectDirectoryInfo.GetFiles(XMLFileName + ".xml", SearchOption.AllDirectories);

            if (XMLFileInfo.Length == 0)
                return false;
            else
                return true;
        }

        internal void CreateReferenceDocument(string assembly, SyntaxNode node, SemanticModel model, string fileName, string apiName)
        {
            if (activeDocProject == null)
            {
                return;
            }
                
            string assemblyXMLDirectoryPath = Path.Combine(Path.GetDirectoryName(activeDocProject.FileName), "Docs", assembly);
            if (!Directory.Exists(assemblyXMLDirectoryPath))
            {
                Directory.CreateDirectory(assemblyXMLDirectoryPath);
            }

            ProjectItem XMLFolder = getDocsFolderInTree();
            

            string fileNameFullPath = Path.Combine(assemblyXMLDirectoryPath, fileName + ".xml");
            File.WriteAllText(fileNameFullPath, string.Format(DdueTemplates.template, assembly, apiName, DdueTemplates.templateOfSummary, DdueTemplates.templateOfGenericParameters,
                                                                DdueTemplates.templateOfParameters, DdueTemplates.templateOfReturnValue, DdueTemplates.templateOfExceptions,
                                                                DdueTemplates.templateOfRemarks, DdueTemplates.templateOfCodeExamples, DdueTemplates.templateOfRelatedTopics));

            ProjectItem assemblyFolderItem = null;
            foreach (ProjectItem item in XMLFolder.ProjectItems)
            {
                if (item.Name == assembly)
                {
                    assemblyFolderItem = item;
                    break;
                }
            }

            if (assemblyFolderItem == null)
            {
                assemblyFolderItem = XMLFolder.ProjectItems.AddFolder(assembly);
            }
            else
            {
                assemblyFolderItem.ProjectItems.AddFromFile(fileNameFullPath);
            }

            return;
        }

        internal static string GetQualifiedAPIName(SyntaxNode node, SemanticModel model)
        {
            ISymbol symbol = model.GetDeclaredSymbol(node);
            string qualifiedXMLName = null;

            if (symbol != null) 
            {
                qualifiedXMLName = symbol.GetDocumentationCommentId();             
            }

            return qualifiedXMLName;
        }

        internal static string GetQualifiedFileName(SyntaxNode node, SemanticModel model)
        {
            ISymbol symbol = model.GetDeclaredSymbol(node);

            string qualifiedFileName = GetQualifiedAPIName(node, model).Replace(":",DocAsCodeConstant.charToReplaceColon)
                                                                        .Replace("*",DocAsCodeConstant.charToReplaceStar);

            return qualifiedFileName;
        }

        /// <summary>
        /// Create a APIRecord.xml under each Docs/{assembly} folder to record the API names and count of this assembly.
        /// Because of the item.ProjectItems.AddFolder and item.ProjectItems.AddFromDirectory behavior problem it is omitted.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="model"></param>
        /// <returns>the Docs Folder ProjectItem</returns>
        internal ProjectItem updateAPIRecord(SyntaxNode node, SemanticModel model)
        {
            return null;

            if (activeDocProject == null)
            {
                return null;
            }

            string assembly = model.Compilation.AssemblyName;
            ISymbol symbol = model.GetDeclaredSymbol(node);
            string qualifiedFileName = GetQualifiedFileName(node, model);

            string assemblyXMLDirectoryPath = Path.Combine(Path.GetDirectoryName(activeDocProject.FileName), "Docs", assembly);
            if (!Directory.Exists(assemblyXMLDirectoryPath))
            {
                Directory.CreateDirectory(assemblyXMLDirectoryPath);
            }

            ProjectItem XMLFolder = getDocsFolderInTree();

            XmlDocument APIRecordXML = new XmlDocument();
            string APIRecordFilePath = Path.Combine(assemblyXMLDirectoryPath, "APIRecord.xml");
            XmlElement rootElement = null;
            if (!File.Exists(APIRecordFilePath))
            {
                var declaration = APIRecordXML.CreateXmlDeclaration("1.0", "utf-8", null);
                rootElement = APIRecordXML.CreateElement("APIRecords");
                rootElement.SetAttribute("totalCount", "0");
                APIRecordXML.AppendChild(rootElement);
                APIRecordXML.InsertBefore(declaration, rootElement);
            }
            else
            {
                APIRecordXML.Load(APIRecordFilePath);
                rootElement = APIRecordXML.SelectSingleNode("APIRecords") as XmlElement;
            }

            bool nodeExist = false;
            XmlElement apiSub = null;
            XmlNodeList recordNodeList = rootElement.SelectNodes("/APIRecords/APIRecord");
            foreach (XmlNode recordNode in recordNodeList)
            {
                if (recordNode.InnerText == qualifiedFileName)
                {
                    nodeExist = true;
                    apiSub = recordNode as XmlElement;
                }
            }

            if (nodeExist == false)
            {
                apiSub = APIRecordXML.CreateElement("APIRecord");
                apiSub.InnerText = qualifiedFileName;
                rootElement.SetAttribute("totalCount", (Int32.Parse(rootElement.GetAttribute("totalCount")) + 1).ToString());
                rootElement.AppendChild(apiSub);

                try
                {
                    APIRecordXML.Save(APIRecordFilePath);
                }
                catch (XmlException e)
                {
                    Debug.WriteLine("Exception: " + e.Message);
                    return null;
                }
            }

            return XMLFolder;
        }

        internal ProjectItem getDocsFolderInTree()
        {
            ProjectItem XMLFolder = null;
            bool XMLFolderExist = false;

            foreach (ProjectItem projectItem in activeDocProject.ProjectItems)
            {
                if (projectItem.Name == "Docs")
                {
                    XMLFolder = projectItem;
                    XMLFolderExist = true;
                    break;
                }
            }

            if (XMLFolderExist == false)
            {
                XMLFolder = activeDocProject.ProjectItems.AddFolder("Docs");
            }

            return XMLFolder;
        }
    }
}



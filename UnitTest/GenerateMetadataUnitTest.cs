﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocAsCode.BuildMeta;
using System.IO;
using System.Threading.Tasks;

namespace UnitTest
{
    /// <summary>
    /// MEF is used for workspace host service provider, need to copy dll manually
    /// </summary>
    [TestClass]
    [DeploymentItem("Microsoft.CodeAnalysis.CSharp.Workspaces.dll")]
    [DeploymentItem("Microsoft.CodeAnalysis.CSharp.Workspaces.Desktop.dll")]
    public class GenerateMetadataUnitTest
    {
        [TestMethod]
        [DeploymentItem("Assets", "Assets")]
        [Ignore]
        public async Task TestGenereateMetadataAsync()
        {
            string slnPath = "Assets/TestClass1/TestClass1.sln";
            string outputDirectory = "output";
            await DocAsCodeUtility.GenerateMetadataAsync(@"Assets\TestClass1\TestClass1.sln", outputDirectory, null, OutputType.Metadata);

            Assert.IsTrue(Directory.Exists("output"));
        }

        [TestMethod]
        [DeploymentItem("Assets", "Assets")]
        [DeploymentItem("Assets", "Assets")]
        public async Task TestGenereateMetadataAsync_Project()
        {
            string slnPath = "Assets/TestClass1/TestClass1.sln";
            string outputDirectory = "output";
            await DocAsCodeUtility.GenerateMetadataAsync(@"Assets\TestClass1\TestClass1\TestClass1.csproj", outputDirectory, null, OutputType.Metadata);

            Assert.IsTrue(Directory.Exists("output"));
        }
    }
}
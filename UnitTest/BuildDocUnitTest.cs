using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocAsCode.BuildMeta;
using System.IO;
using System.Threading.Tasks;
using EntityModel;

namespace UnitTest
{
    /// <summary>
    /// MEF is used for workspace host service provider, need to copy dll manually
    /// </summary>
    [TestClass]
    public class BuildDocUnitTest
    {
        [TestMethod]
        [DeploymentItem("Assets/Metadata/TestClass1.docmta", "Assets/Metadata/")]
        public async Task TestParseMetadataFile()
        {
            string metadataFileName = @"D:\Repo\docascode\UnitTest\Assets\Metadata\TestClass1.docmta"; //"Assets/Metadata/TestClass1.docmta";
            ProjectMetadata projectMetadata;
            string message;
            bool success = BuildDocHelper.TryParseMetadataFile(metadataFileName, out projectMetadata, out message);
            Assert.IsTrue(string.IsNullOrEmpty(message), message);
            Assert.IsTrue(success);

            string exportMessage;
            string output = @"D:\Repo\docascode\UnitTest\Assets\Metadata\TestClass1_actual.docmta"; // Path.GetRandomFileName();
            success = BuildMetaHelper.TryExportMetadataFile(projectMetadata, output, ExportType.Json , out message);
            Assert.IsTrue(string.IsNullOrEmpty(message), message);
            Assert.IsTrue(success);

            Assert.AreEqual(File.ReadAllText(metadataFileName), File.ReadAllText(output));
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocAsCode.BuildMeta;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UnitTest
{
    /// <summary>
    /// MEF is used for workspace host service provider, need to copy dll manually
    /// </summary>
    [TestClass]
    public class MarkdownIndexBuilderUnitTest
    {
        [TestMethod]
        [DeploymentItem("Assets/Markdown/About.md")]
        public async Task TestMarkdownIndexBuilder()
        {
            string markdownFile = "About.md";
            string outputDirectory = "output";
            List<MarkdownIndex> indexes;
            var result =  BuildMarkdownIndexHelper.TryParseCustomizedMarkdown(markdownFile, null, out indexes);
            foreach(var index in indexes)
            {
                Console.WriteLine(index);
            }

            Assert.AreEqual(2, indexes.Count);
            Assert.AreEqual(ResultLevel.Warn, result.ResultLevel, result.Message);
        }
    }
}

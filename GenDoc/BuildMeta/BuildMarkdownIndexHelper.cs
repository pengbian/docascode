using DocAsCode.EntityModel;
using DocAsCode.Utility;
using EntityModel;
using EntityModel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DocAsCode.BuildMeta
{
    public class MarkdownIndex
    {
        public string Id { get; set; }

        public string FilePath { get; set; }

        public string YamlPath { get; set; }

        public int Startline { get; set; }

        public string MarkdownContent { get; set; }

        public int ContentStartIndex { get; set; }

        public int ContentEndIndex { get; set; }

        /// <summary>
        /// To override yaml settings
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as MarkdownIndex;
            if (other == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(this.Id) && string.IsNullOrEmpty(other.Id) == null)
            {
                return true;
            }
            else
            {
                return false;
            }

            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            if (this.Id == null)
            {
                return string.Empty.GetHashCode();
            }

            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            using (StringWriter writer = new StringWriter())
            {
                YamlUtility.Serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }
    }
    public class BuildMarkdownIndexHelper
    {
        public static IEnumerable<MarkdownIndex> MergeMarkdownResults(List<string> markdownFilePathList, Dictionary<string, IndexYamlItemViewModel> apiList)
        {
            Dictionary<string, MarkdownIndex> table = new Dictionary<string, MarkdownIndex>();
            foreach(var file in markdownFilePathList)
            {
                List<MarkdownIndex> indics;
                IndexYamlItemViewModel item;
                var result = TryParseCustomizedMarkdown(file, s =>
                {
                    if (apiList.TryGetValue(s.Name, out item))
                    {
                        return new ParseResult(ResultLevel.Success);
                    }
                    else
                    {
                        return new ParseResult(ResultLevel.Error, "Cannot find {0} in the documentation", s.Name);
                    }
                }, out indics);

                if (result.ResultLevel != ResultLevel.Success)
                {
                    Console.Error.WriteLine(result);
                }

                foreach (var key in indics)
                {
                    MarkdownIndex saved;
                    if (table.TryGetValue(key.Id, out saved))
                    {
                        ParseResult.WriteToConsole(ResultLevel.Error, "Already contains {0} in file {1}, current one {2} will be ignored.", key.Id, saved.FilePath, key.FilePath);
                    }
                    else
                    {
                        table.Add(key.Id, key);
                    }
                }
            }

            return table.Select(s => s.Value);
        }

        /// <summary>
        /// Not doing duplication check here, do it outside
        /// </summary>
        /// <param name="markdownFilePath"></param>
        /// <param name="markdown"></param>
        /// <returns></returns>
        public static ParseResult TryParseCustomizedMarkdown(string markdownFilePath, Func<YamlItemViewModel, ParseResult> yamlHandler, out List<MarkdownIndex> markdown)
        {
            string markdownFile = File.ReadAllText(markdownFilePath);
            int length = markdownFile.Length;
            var yamlRegex = new Regex(@"\-\-\-((?!\n)\s)*\n((?!\n)\s)*(?<content>.*)((?!\n)\s)*\n\-\-\-((?!\n)\s)*\n", RegexOptions.Compiled | RegexOptions.Multiline);
            MatchCollection matches = yamlRegex.Matches(markdownFile);
            if (matches.Count == 0)
            {
                markdown = new List<MarkdownIndex>();
                return new ParseResult(ResultLevel.Warn, "no valid yaml header is found in {0}", markdownFilePath);
            }

            int startIndex = 0;
            MarkdownIndex lastSection = null;
            List<MarkdownIndex> sections = new List<MarkdownIndex>();

            StringBuilder error = new StringBuilder();

            for(int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                string content = match.Groups["content"].Value;

                YamlItemViewModel viewModel;
                // Content to yaml
                try
                {
                    using (StringReader reader = new StringReader(content))
                    {
                        viewModel = YamlUtility.Deserializer.Deserialize<YamlItemViewModel>(reader);

                        if (string.IsNullOrEmpty(viewModel.Name))
                        {
                            throw new ArgumentException("Name for yaml header is required");
                        }

                        // TODO: override metadata, merge viewmodel?

                        if (yamlHandler != null)
                        {
                            ParseResult result = yamlHandler(viewModel);
                            if (result.ResultLevel != ResultLevel.Success)
                            {
                                throw new ArgumentException(result.Message);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    error.AppendFormat("{0} in {1} line {2} is not in a valid yaml format {3}", match.Value, markdownFilePath, markdownFile.Substring(0, startIndex).Split('\n').Length + 2, e.Message);
                    continue;
                }

                startIndex = match.Index + match.Length;
                if (lastSection != null)
                {

                    lastSection.ContentEndIndex = match.Index - 1;

                    if (lastSection.ContentEndIndex > lastSection.ContentStartIndex)
                    {
                        lastSection.MarkdownContent = markdownFile.Substring(lastSection.ContentStartIndex, lastSection.ContentEndIndex - lastSection.ContentStartIndex + 1);
                        sections.Add(lastSection);
                    }
                }

                lastSection = new MarkdownIndex { Id = viewModel.Name, ContentStartIndex = startIndex, ContentEndIndex = length - 1, YamlPath = viewModel.YamlPath, FilePath = markdownFilePath }; // endIndex should be set from next match if there is next match
                if (lastSection.ContentEndIndex > lastSection.ContentStartIndex)
                {
                    lastSection.MarkdownContent = markdownFile.Substring(lastSection.ContentStartIndex);
                }
            }

            if (lastSection != null)
            {
                sections.Add(lastSection);
            }

            markdown = sections;
            if (error.Length > 0)
            {
                return new ParseResult(ResultLevel.Warn, error.ToString());
            }

            return new ParseResult(ResultLevel.Success);
        }
    }
}

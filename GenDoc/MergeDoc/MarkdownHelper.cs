using DocAsCode.EntityModel;
using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DocAsCode.MergeDoc
{
    public class MarkdownCollectionCache : Dictionary<string, string>
    {
        public Dictionary<string, string> IdMarkdownFileMap;
        public MarkdownCollectionCache(IEnumerable<string> mdFiles)
            : base(mdFiles.Where(s => s.EndsWith(".md", StringComparison.OrdinalIgnoreCase)).SelectMany(s => MarkdownFile.Load(s).Sections).Where(s => s != null).ToDictionary(s => s.Id, s => s.MarkdownContent))
        {
            IdMarkdownFileMap = new Dictionary<string, string>();
            var markdownFiles = mdFiles.Where(s => s.EndsWith(".md", StringComparison.OrdinalIgnoreCase)).Select(s => MarkdownFile.Load(s));

            foreach (var file in markdownFiles)
            {
                foreach(var section in file.Sections)
                {
                    IdMarkdownFileMap.Add(section.Id, file.FilePath);
                }
            }
        }
    }

    public class MarkdownFile
    {
        private static CommentIdToYamlHeaderConverter _converter = new CommentIdToYamlHeaderConverter();

        public string FilePath { get; set; }

        public IReadOnlyList<MarkdownSection> Sections { get; set; }

        /// <summary>
        /// TODO: Load from md file
        /// ---
        /// method: A()
        /// ---
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static MarkdownFile Load(string filePath)
        {
            MarkdownFile file;
            TryParseCustomizedMarkdown(filePath, out file);

            return file;
        }

        public class MarkdownSection
        {
            public string Id { get; set; }

            public string MarkdownContent { get; set; }

            public int ContentStartIndex { get; set; }

            public int ContentEndIndex { get; set; }
        }

        private static bool TryParseCustomizedMarkdown(string markdownFilePath, out MarkdownFile markdown)
        {
            string markdownFile = File.ReadAllText(markdownFilePath);
            int length = markdownFile.Length;
            Dictionary<string, MarkdownSection> sections = new Dictionary<string, MarkdownSection>();
            var yamlRegex = CommentIdToYamlHeaderConverter.YamlHeaderRegex;
            MatchCollection matches = yamlRegex.Matches(markdownFile);
            if (matches.Count == 0)
            {
                markdown = null;
                return false;
            }

            int startIndex = 0;
            string lastCommentId = null;
            MarkdownSection lastSection = null;
            markdown = new MarkdownFile { FilePath = markdownFilePath };

            MarkdownSection section;

            // TODO: current assumption is : match is in order, need confirmation
            foreach (Match match in matches)
            {
                lastCommentId = (string)_converter.ConvertFrom(match.Value);
                startIndex = match.Index + match.Length;

                // If current Id is already set, ignore the duplicate one
                if (lastSection != null && !sections.TryGetValue(lastCommentId, out section))
                {
                    lastSection.ContentEndIndex = match.Index - 1;
                    if (lastSection.ContentEndIndex > lastSection.ContentStartIndex)
                    {
                        lastSection.MarkdownContent = markdownFile.Substring(lastSection.ContentStartIndex, lastSection.ContentEndIndex - lastSection.ContentStartIndex + 1);
                        sections.Add(lastSection.Id, lastSection);
                    }
                }

                lastSection = new MarkdownSection { Id = lastCommentId, ContentStartIndex = startIndex, ContentEndIndex = length }; // endIndex should be set from next match if there is next match
                if (lastSection.ContentEndIndex > lastSection.ContentStartIndex)
                {
                    lastSection.MarkdownContent = markdownFile.Substring(lastSection.ContentStartIndex);
                }
            }

            if (lastSection != null && !sections.TryGetValue(lastCommentId, out section))
            {
                sections.Add(lastCommentId, lastSection);
            }

            markdown.Sections = sections.Values.ToList();
            return true;
        }
    }

    public class MarkDownConvertor
    {
        private Markdown markdown = new Markdown();
        private Dictionary<string, string> _idPathRelativeMapping;

        public MarkDownConvertor(Dictionary<string, string> idPathRelativeMapping)
        {
            this._idPathRelativeMapping = idPathRelativeMapping;
            markdown.AutoHyperlink = true;
            markdown.AutoNewLines = true;
            markdown.EmptyElementSuffix = ">";
            markdown.EncodeProblemUrlCharacters = true;
            markdown.LinkEmails = false;
            markdown.StrictBoldItalic = true;
        }

        public string ConvertToHTML(string md)
        {
            string result = ResolveAT(md);
            result = markdown.Transform(result);
            return result;
        }
        public string ResolveAT(string md)
        {
            md = Regex.Replace(md, @"@(?<ATcontent>\S*)\s", new MatchEvaluator(ATResolver), RegexOptions.Compiled);
            return md;
        }
        private string ATResolver(Match match)
        {
            string filePath;
            string id = match.Groups["ATcontent"].Value;
            if (_idPathRelativeMapping.TryGetValue(id, out filePath))
            {
                return string.Format("[{0}]({1})", match.Value.Trim().Substring(3), filePath); ;
            }
            return match.Value;
        }
    }
}

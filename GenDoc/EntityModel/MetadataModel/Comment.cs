using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class TripleSlashComment : IComment
    {
        public string Raw { get; set; }
        public int StartLine { get; set; }
    }

    public class MarkdownComment : IComment
    {
        public string Raw { get; set; }
        public int StartLine { get; set; }
    }

    public class PlainTextComment : IComment
    {
        public string Raw { get; set; }
        public int StartLine { get; set; }
    }

    public interface IComment
    {
        string Raw { get; set; }
        int StartLine { get; set; }
    }

    public enum CommentType
    {
        TripleSlash,
        Markdown,
        PlainText
    }
}

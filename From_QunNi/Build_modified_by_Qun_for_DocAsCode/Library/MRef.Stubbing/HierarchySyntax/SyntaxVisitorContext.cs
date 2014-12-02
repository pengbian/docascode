namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using System;
    using System.IO;

    public sealed class SyntaxVisitorContext
    {
        #region Fields
        private readonly TextWriter Writer;
        private readonly DisplayOptions Options;
        #endregion

        #region Ctors

        internal SyntaxVisitorContext(TextWriter writer, DisplayOptions options)
        {
            Writer = writer;
            Options = options;
        }

        #endregion

        #region Methods

        public SyntaxVisitorContext RemoveOptions(DisplayOptions options)
        {
            if ((Options & options) == 0)
                return this;
            return new SyntaxVisitorContext(Writer, Options & ~options);
        }

        public void Write(string value)
        {
            Writer.Write(value);
        }

        public bool HasOptions(DisplayOptions options)
        {
            return (Options & options) == options;
        }

        #endregion
    }
}

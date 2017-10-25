using System;
using System.IO;

namespace sdmap.Emiter.Implements.Common
{
    internal class IndentWriter
    {
        private readonly TextWriter _writer;
        private int _indent;

        public IndentWriter(TextWriter writer, int indent)
        {
            _writer = writer;
            _indent = indent;
        }

        public void Write(string text)
        {
            _writer.Write(new string(' ', _indent * 4));
            _writer.Write(text);
        }

        public void WriteLine(string text)
        {
            _writer.Write(new string(' ', _indent * 4));
            _writer.WriteLine(text);
        }

        public void PushIndent()
        {
            _indent++;
        }

        public void PopIndent()
        {
            _indent--;
            if (_indent < 0)
                throw new InvalidOperationException("Cannot PopIndent.");
        }

        internal void Flush()
        {
            _writer.Flush();
        }
    }
}

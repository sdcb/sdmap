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
            _writer.Write(text);
        }

        public void WriteLine(string text)
        {
            _writer.WriteLine(text);
        }

        public void WriteIndentLine(string text)
        {
            WriteIndent();
            WriteLine(text);
        }

        public T UsingIndent<T>(string start, string end, Func<T> body)
        {
            WriteIndentLine(start);
            PushIndent();
            var result = body();
            PopIndent();
            WriteIndentLine(end);
            return result;
        }

        public void UsingIndent(string start, string end, Action body)
        {
            WriteIndentLine(start);
            PushIndent();
            body();
            PopIndent();
            WriteIndentLine(end);
        }

        public void UsingIndent(Action body)
        {
            PushIndent();
            body();
            PopIndent();
        }

        public T UsingIndent<T>(Func<T> body)
        {
            PushIndent();
            var result = body();
            PopIndent();
            return result;
        }

        public void WriteLine()
        {
            _writer.WriteLine();
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

        public void Flush()
        {
            _writer.Flush();
        }

        public void WriteIndent()
        {
            _writer.Write(new string(' ', _indent * 4));
        }

        public void WriteIndent(string text)
        {
            WriteIndent();
            Write(text);
        }
    }
}

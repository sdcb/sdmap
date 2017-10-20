using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using System.IO;

namespace sdmap.Emiter.Implements.CSharp
{
    public class CSharpCodeListener : SdmapParserBaseListener
    {
        private readonly TextWriter _writer;

        private readonly string _rootNs;

        public override void EnterRoot([NotNull] SdmapParser.RootContext context)
        {
            _writer.WriteLine("using System;");
        }

        public override void EnterNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            _writer.Write("namespace ");
        }

        public override void ExitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            _writer.Write("}");
        }

        public override void ExitNsSyntax([NotNull] SdmapParser.NsSyntaxContext context)
        {
            _writer.Write(context.GetText());
        }
    }
}

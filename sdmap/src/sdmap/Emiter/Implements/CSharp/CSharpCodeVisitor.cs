using sdmap.Emiter.Implements.Common;
using sdmap.Functional;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace sdmap.Emiter.Implements.CSharp
{
    internal class CSharpCodeVisitor : SdmapParserBaseVisitor<Result>
    {
        private readonly CodeEmiterConfig _config;
        private readonly CSharpDefine _define;
        private readonly IndentWriter _writer;

        internal CSharpCodeVisitor(
            TextWriter writer,
            CodeEmiterConfig config,
            CSharpDefine define)
        {
            _writer = new IndentWriter(writer, 0);
            _config = config;
            _define = define;
        }

        public Result StartVisit([NotNull] SdmapParser.RootContext context)
        {
            if (context.ChildCount == 0)
            {
                return Result.Fail("Empty source.");
            }
            return Visit(context);
        }

        public override Result VisitRoot([NotNull] SdmapParser.RootContext context)
        {
            foreach (var usingItem in _define.CommonUsings())
            {
                _writer.WriteLine($"using {usingItem};");
            }
            var result = base.VisitRoot(context);
            _writer.Flush();
            return result;
        }

        public override Result VisitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            _writer.WriteLine();
            _writer.WriteIndent();                       // _  
            _writer.Write("namespace ");                 // _ namespace
            _writer.Write(context.nsSyntax().GetText()); // _ namespace id
            _writer.WriteLine();                         // _ namespace id <CRLF>
            _writer.WriteIndent();                       // _
            _writer.WriteLine("{");                      // _ { <CRLF>

            //var result = base.VisitNamespace(context);

            _writer.WriteIndent();                       // _
            _writer.WriteLine("}");                      // _ }

            return Result.Ok();
        }
    }
}

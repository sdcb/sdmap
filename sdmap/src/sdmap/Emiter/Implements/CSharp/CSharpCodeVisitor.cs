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
            _writer.Write(context.nsSyntax().GetText()); // _ namespace {id}
            _writer.WriteLine();                         // _ namespace {id} <CRLF>
            _writer.WriteIndent();                       // _
            _writer.WriteLine("{");                      // _ { <CRLF>

            var result = base.VisitNamespace(context);

            _writer.WriteIndent();                       // _
            _writer.WriteLine("}");                      // _ }

            return result;
        }

        public override Result VisitNamedSql([NotNull] SdmapParser.NamedSqlContext context)
        {
            _writer.WriteLine();
            _writer.WriteIndent();                         // _
            _writer.Write(_config.AccessModifier);         // _ internal
            _writer.Write(" class ");                      // _ internal class
            _writer.WriteLine(context.SYNTAX().GetText()); // _ internal class {id} <CRLF>
            _writer.WriteIndentLine("{");                  // _ { <CRLF>

            _writer.PushIndent();
            var result = ClassGeneration();
            _writer.PopIndent();

            _writer.WriteIndentLine("}");                  // _ } <CRLF>
            return result;

            Result ClassGeneration()
            {
                _writer.WriteIndentLine(      // _ internal res B()
                    $"{_config.AccessModifier} Result<string> BuildText()");
                _writer.WriteIndentLine("{"); // _ {

                _writer.PushIndent();
                var r = MethodGeneration();
                _writer.PopIndent();

                _writer.WriteIndentLine("}"); // _ }
                return r;
            }

            Result MethodGeneration()
            {
                _writer.WriteIndentLine("var sb = new StringBuilder();");
                _writer.WriteIndentLine("return sb;");
                return Result.Ok();
            }
        }

        protected override Result AggregateResult(Result aggregate, Result nextResult)
        {
            return Result.Combine(new[]
            {
                aggregate,
                nextResult
            });
        }
    }
}

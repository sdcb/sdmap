using sdmap.Emiter.Implements.Common;
using sdmap.Functional;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using sdmap.Utils;
using static sdmap.Parser.G4.SdmapParser;

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

        public Result StartVisit([NotNull] RootContext context)
        {
            if (context.ChildCount == 0)
            {
                return Result.Fail("Empty source.");
            }
            return Visit(context);
        }

        public override Result VisitRoot([NotNull] RootContext context)
        {
            foreach (var usingItem in _define.CommonUsings())
            {
                _writer.WriteLine($"using {usingItem};");
            }
            var result = base.VisitRoot(context);
            _writer.Flush();
            return result;
        }

        public override Result VisitNamespace([NotNull] NamespaceContext context)
        {
            _writer.WriteLine();
            _writer.WriteIndentLine(                     // _ namespace {id} <CRLF>
                $"namespace {context.nsSyntax().GetText()}");
            return _writer.UsingIndent("{", "}", () =>
            {
                return base.VisitNamespace(context);
            });
        }

        public override Result VisitNamedSql([NotNull] NamedSqlContext context)
        {
            _writer.WriteLine();
            _writer.WriteIndentLine(
                $"{_config.AccessModifier} class {context.SYNTAX().GetText()}");
            _writer.PushIndent();                          // _ internal class {id} <CRLF>
            _writer.WriteIndentLine($": {nameof(ISdmapEmiter)}");
            _writer.PopIndent();                           // _ _ : IBase
            return _writer.UsingIndent("{", "}", () =>
            {
                return ClassGeneration();
            });
            

            Result ClassGeneration()
            {
                _writer.WriteIndentLine(      // _ internal res B()
                    $"{_config.AccessModifier} Result<string> BuildText()");
                return _writer.UsingIndent("{", "}", () =>
                {
                    return MethodGeneration();
                });
            }

            Result MethodGeneration()
            {
                _writer.WriteIndentLine("var sb = new StringBuilder();");
                var r = base.VisitNamedSql(context);
                _writer.WriteIndentLine("return sb;");
                return r;
            }
        }

        public override Result VisitPlainText([NotNull] PlainTextContext context)
        {
            var sqlText = SqlTextUtil.Parse(context.GetToken(SQLText, 0).GetText());
            var csharpText = SqlTextUtil.ToCSharpString(sqlText);
            _writer.WriteIndentLine($"sb.Append({csharpText});");
            return Result.Ok();
        }

        public override Result VisitMacro([NotNull] MacroContext context)
        {
            var parameterCtxs = context.macroParameter();
            return _writer.UsingIndent<Result>("{", "}", () =>
            {
                throw new NotImplementedException();
            });
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

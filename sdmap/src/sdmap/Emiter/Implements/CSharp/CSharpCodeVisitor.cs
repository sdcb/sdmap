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
        private readonly Dictionary<string, Func<IndentWriter, Result>> _unnamedSqls =
            new Dictionary<string, Func<IndentWriter, Result>>();

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
                _writer.WriteIndentLine($"using {usingItem};");
            }
            _writer.WriteLine();
            var result = base.VisitRoot(context);
            _writer.Flush();
            return result;
        }

        public override Result VisitNamespace([NotNull] NamespaceContext context)
        {
            _writer.WriteIndentLine(                     // _ namespace {id} <CRLF>
                $"namespace {context.nsSyntax().GetText()}");
            return _writer.UsingIndent("{", "}", () =>
            {
                return base.VisitNamespace(context);
            });
        }

        public override Result VisitNamedSql([NotNull] NamedSqlContext context)
        {
            _writer.WriteIndentLine(
                $"{_config.AccessModifier} class {context.SYNTAX().GetText()}");
            _writer.UsingIndent(() =>
            {                                              // _ internal class {id} <CRLF>
                _writer.WriteIndentLine($": {nameof(ISdmapEmiter)}");
            });                                            // _ _ : IBase
            return _writer.UsingIndent("{", "}", () =>
            {
                return ClassGeneration();
            });


            Result ClassGeneration()
            {
                _writer.WriteIndentLine(      // _ internal res B()
                    $"{_config.AccessModifier} Result<string> BuildText(object self)");
                return _writer.UsingIndent("{", "}", () =>
                {
                    return Result.Combine(
                        MethodGeneration(), 
                        UnnamedSqlGeneration());
                });
            }

            Result MethodGeneration()
            {
                _writer.WriteIndentLine("var sb = new StringBuilder();");
                var r = base.VisitNamedSql(context);
                _writer.WriteIndentLine("return Result.Ok(sb.ToString());");
                return r;
            }

            Result UnnamedSqlGeneration()
            {
                foreach (var unnamed in _unnamedSqls)
                {
                    _writer.WriteLine();
                    _writer.WriteIndentLine($"private Result<string> {unnamed.Key}(object self)");
                    var ok = _writer.UsingIndent("{", "}", () =>
                    {
                        return unnamed.Value(_writer);
                    });
                    if (ok.IsFailure) return ok;
                }
                return Result.Ok();
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
            return _writer.UsingIndent("{", "}", () =>
            {
                var id = context.SYNTAX().GetText();
                if (id == "include")
                {
                    var provider = $"{nameof(RuntimeProviders)}.{nameof(RuntimeProviders.GetEmiter)}";
                    _writer.WriteIndentLine($"var emiter = {provider}<{id}>();");
                    _writer.WriteIndentLine($"var result = emiter.{nameof(ISdmapEmiter.BuildText)}(self);");
                }
                else
                {
                    var provider = $"{nameof(RuntimeProviders)}.{nameof(RuntimeProviders.RuntimeMacros)}";
                    _writer.WriteIndent($"var result = {provider}.{context.SYNTAX()}(");
                    WriteMacroParameters(context.macroParameter());
                }

                _writer.WriteIndentLine($"if (result.{nameof(Result.IsSuccess)})");
                _writer.UsingIndent("{", "}", () =>
                {
                    _writer.WriteIndentLine(
                        $"sb.Append(result.{nameof(Result<int>.Value)});");
                });
                _writer.WriteIndentLine("else");
                _writer.UsingIndent("{", "}", () =>
                {
                    _writer.WriteIndentLine("return result;");
                });
                return Result.Ok();
            });
        }

        private Result WriteMacroParameters(MacroParameterContext[] parameterCtxs)
        {
            for (var i = 0; i < parameterCtxs.Length; ++i)
            {
                var parameter = parameterCtxs[i];

                if (parameter.nsSyntax() != null)
                {
                    _writer.Write(
                        SqlTextUtil.ToCSharpString(parameter.nsSyntax().GetText()));
                }
                else if (parameter.STRING() != null)
                {
                    var result = StringUtil.Parse(parameter.STRING().GetText());
                    if (result.IsFailure) return result;

                    _writer.Write(SqlTextUtil.ToCSharpString(result.Value));
                }
                else if (parameter.NUMBER() != null)
                {
                    // sdmap number are compatible with C# double
                    _writer.Write(parameter.NUMBER().GetText());
                }
                else if (parameter.DATE() != null)
                {
                    var result = DateUtil.Parse(parameter.DATE().GetText());
                    if (result.IsFailure) return result;
                    var date = result.Value;
                    _writer.Write(
                        $"new DateTime({date.Year}, {date.Month}, {date.Day})");
                }
                else if (parameter.Bool() != null)
                {
                    // sdmap bool are compatible with C# bool
                    _writer.Write(parameter.Bool().GetText());
                }
                else if (parameter.unnamedSql() != null)
                {
                    var parseTree = parameter.unnamedSql();
                    var id = NameUtil.GetFunctionName(parseTree);
                    if (!_unnamedSqls.ContainsKey(id))
                    {
                        _unnamedSqls[id] = (writer) =>
                        {
                            return Visit(parseTree.coreSql());
                        };
                    }
                    _writer.Write($"{id}()");
                }

                // every parameter should follow by a "," separator, 
                // except last parameter.
                if (i < parameterCtxs.Length - 1)
                {
                    _writer.Write(", ");
                }
            }
            _writer.WriteLine(");");
            return Result.Ok();
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

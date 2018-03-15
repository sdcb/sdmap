using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using sdmap.Functional;
using sdmap.Macros;
using sdmap.Parser.G4;
using sdmap.Utils;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.Parser.Visitor
{
    internal class CoreSqlVisitor : SdmapParserBaseVisitor<Result>
    {
        protected readonly SdmapCompilerContext _context;
        protected ILGenerator _il;
        protected int _stackPos;

        public EmitFunction Function { get; protected set; }

        private static MethodInfo _appendCall = typeof(List<object>)
                .GetMethod(nameof(List<object>.Add), new[] { typeof(object) });

        public CoreSqlVisitor(
            SdmapCompilerContext context)
        {
            _context = context;
        }

        public Result Process(CoreSqlContext parseRule, string functionName)
        {
            var method = new DynamicMethod(functionName,
                typeof(Result<string>), new[] { typeof(OneCallContext) });
            _il = method.GetILGenerator();

            void returnBlock()
            {
                Label rootExit = _il.DefineLabel();
                Label childExit = _il.DefineLabel();
                _il.Emit(OpCodes.Ldarg_0);                        // ctx
                _il.Emit(OpCodes.Call, OneCallContext.GetIsRoot); // isRoot
                _il.Emit(OpCodes.Ldc_I4_0);                       // if(!isRoot)
                _il.Emit(OpCodes.Beq, childExit);                 // ->goto childExit

                _il.MarkLabel(rootExit);
                {
                    MethodInfo combineDeps = typeof(CoreSqlVisitorHelper)
                        .GetMethod(nameof(CoreSqlVisitorHelper.CombineDeps));
                    _il.Emit(OpCodes.Ldarg_0);                                    // ctx
                    _il.Emit(OpCodes.Call, combineDeps);                          // result<str>
                    _il.Emit(OpCodes.Ret);                                        // 
                }
                _il.MarkLabel(childExit);
                {
                    MethodInfo combineDeps = typeof(CoreSqlVisitorHelper)
                        .GetMethod(nameof(CoreSqlVisitorHelper.CombineStrings));
                    _il.Emit(OpCodes.Ldarg_0);                                    // ctx
                    _il.Emit(OpCodes.Call, combineDeps);                          // result<str>
                    _il.Emit(OpCodes.Ret);                                        // 
                }
                
                Function = (EmitFunction)method.CreateDelegate(typeof(EmitFunction));
            };

            if (parseRule == null)
            {
                returnBlock();
                return Result.Ok();
            }

            _il.DeclareLocal(typeof(List<object>));
            _il.Emit(OpCodes.Ldarg_0);                                                // ctx
            _il.Emit(OpCodes.Call, OneCallContext.GetTempStore);                      // sb
            _il.Emit(OpCodes.Stloc_0);                                                // [empty]

            return Visit(parseRule)
                .OnSuccess(() =>                                                      // [must be empty]
                {
                    returnBlock();
                });
        }

        public override Result VisitMacro([NotNull] MacroContext context)
        {
            var macroName = context.GetToken(SYNTAX, 0).GetText();

            _il.Emit(OpCodes.Ldarg_0);                                      // ctx
            _il.Emit(OpCodes.Ldstr, macroName);                             // ctx name
            _il.Emit(OpCodes.Ldstr, _context.CurrentNs);                    // ctx name ns
            _il.Emit(OpCodes.Ldarg_0);                                      // ctx name ns ctx
            _il.Emit(OpCodes.Call, OneCallContext.GetObj);                  // ctx name ns self

            var contexts = context.GetRuleContexts<MacroParameterContext>();
            _il.Emit(OpCodes.Ldc_I4, contexts.Length);                      // ctx name ns self
            _il.Emit(OpCodes.Newarr, typeof(object));                       // ctx name ns self args
            for (var i = 0; i < contexts.Length; ++i)
            {
                var arg = contexts[i];

                _il.Emit(OpCodes.Dup);                                      // .. -> args
                _il.Emit(OpCodes.Ldc_I4, i);                                // .. -> args idx

                if (arg.nsSyntax() != null)
                {
                    _il.Emit(OpCodes.Ldstr, arg.nsSyntax().GetText());      // .. -> args idx ele
                }
                else if (arg.STRING() != null)
                {
                    var result = StringUtil.Parse(arg.STRING().GetText());  // .. -> args idx ele
                    if (result.IsSuccess)
                    {
                        _il.Emit(OpCodes.Ldstr, result.Value);              // .. -> args idx ele
                    }
                    else
                    {
                        return result;
                    }
                }
                else if (arg.NUMBER() != null)
                {
                    var result = NumberUtil.Parse(arg.NUMBER().GetText());
                    if (result.IsSuccess)
                    {
                        _il.Emit(OpCodes.Ldc_R8, result.Value);             // .. -> args idx vele
                        _il.Emit(OpCodes.Box, typeof(double));              // .. -> args idx rele
                    }
                    else
                    {
                        return result;
                    }
                }
                else if (arg.DATE() != null)
                {
                    var result = DateUtil.Parse(arg.DATE().GetText());
                    if (result.IsSuccess)
                    {
                        _il.Emit(OpCodes.Ldc_I8, result.Value.ToBinary());  // .. -> args idx int64
                        var ctor = typeof(DateTime).GetConstructor(new[] { typeof(long) });
                        _il.Emit(OpCodes.Newobj, ctor);                     // .. -> args idx date
                        _il.Emit(OpCodes.Box, typeof(DateTime));            // .. -> args idx rele
                    }
                    else
                    {
                        return result;
                    }
                }
                else if (arg.Bool() != null)
                {
                    _il.Emit(bool.Parse(arg.Bool().GetText()) ?
                        OpCodes.Ldc_I4_1 :
                        OpCodes.Ldc_I4_0);                                  // .. -> args idx bool
                    _il.Emit(OpCodes.Box, typeof(bool));                    // .. -> args idx rele
                }
                else if (arg.unnamedSql() != null)
                {
                    var parseTree = arg.unnamedSql();
                    var id = NameUtil.GetFunctionName(parseTree);
                    var result = _context.TryGetEmiter(id, _context.CurrentNs);

                    SqlEmiter emiter;
                    if (result.IsSuccess)
                    {
                        emiter = result.Value;
                    }
                    else
                    {
                        emiter = SqlEmiterUtil.CreateUnnamed(parseTree, _context.CurrentNs);
                        var ok = _context.TryAdd(id, emiter);
                        if (ok.IsFailure) return ok;
                    }

                    var compileResult = emiter.EnsureCompiled(_context);
                    if (compileResult.IsFailure)
                    {
                        return compileResult;
                    }

                    _il.Emit(OpCodes.Ldarg_0);                              // .. -> args idx ctx
                    _il.Emit(OpCodes.Call, OneCallContext.GetCompiler);// .. -> args ids compiler
                    _il.Emit(OpCodes.Ldstr, id);                            // .. -> args idx compiler id
                    _il.Emit(OpCodes.Ldstr, _context.CurrentNs);            // .. -> args idx compiler id ns
                    _il.Emit(OpCodes.Call, typeof(SqlEmiterUtil)
                        .GetMethod(nameof(SqlEmiterUtil.EmiterFromId)));    // .. -> args idx emiter
                }
                else
                {
                    throw new InvalidOperationException();
                }

                _il.Emit(OpCodes.Stelem_Ref);                               // -> ctx name ns self args
            }

            _il.Emit(OpCodes.Call, typeof(MacroManager)
                .GetMethod(nameof(MacroManager.Execute)));                  // result<str>
            _il.Emit(OpCodes.Dup);                                          // result<str> x 2
            _il.Emit(OpCodes.Call, typeof(Result)
                .GetMethod("get_" + nameof(Result.IsSuccess)));             // result<str> bool
            _il.Emit(OpCodes.Ldc_I4_1);                                     // result<str> bool true
            var ifIsSuccess = _il.DefineLabel();
            _il.Emit(OpCodes.Beq, ifIsSuccess);                             // result<str> (jmp if equal)
            _il.Emit(OpCodes.Ret);                                          // [exit-returned]

            _il.MarkLabel(ifIsSuccess);                                     // ifIsSuccess:
            _il.Emit(OpCodes.Call, typeof(Result<string>)
                .GetMethod("get_" + nameof(Result<string>.Value)));         // str
            var strValue = _il.DeclareLocal(typeof(string));
            _il.Emit(OpCodes.Stloc, strValue);                              // [empty]
            _il.Emit(OpCodes.Ldloc_0);                                      // sb
            _il.Emit(OpCodes.Ldloc, strValue);                              // sb str
            _il.Emit(OpCodes.Call, _appendCall);                            // [empty]

            return Result.Ok();
        }

        public override Result VisitPlainText([NotNull] PlainTextContext context)
        {
            var text = SqlTextUtil.Parse(context.GetToken(SQLText, 0).GetText());

            _il.Emit(OpCodes.Ldloc_0);                                             // sb
            _il.Emit(OpCodes.Ldstr, text);                                         // sb str
            _il.Emit(OpCodes.Call, _appendCall);                                   // [empty]
            return Result.Ok();
        }

        public override Result VisitIf([NotNull] IfContext context)
        {
            var coreSql = context.coreSql();
            var id = NameUtil.GetFunctionName(coreSql);
            var result = _context.TryGetEmiter(id, _context.CurrentNs);

            SqlEmiter emiter;
            if (result.IsSuccess)
            {
                emiter = result.Value;
            }
            else
            {
                emiter = SqlEmiterUtil.CreateCore(coreSql, _context.CurrentNs);
                var ok = _context.TryAdd(id, emiter);
                if (ok.IsFailure) return ok;
            }

            var compileResult = emiter.EnsureCompiled(_context);
            if (compileResult.IsFailure)
            {
                return compileResult;
            }

            return new BoolVisitor(_il).Visit(context.boolExpression())
                .OnSuccess(() =>
                {
                    var ifSkip = _il.DefineLabel();
                    _il.Emit(OpCodes.Ldc_I4_0);
                    _il.Emit(OpCodes.Beq, ifSkip);

                    _il.Emit(OpCodes.Ldarg_0);                              // ctx
                    _il.Emit(OpCodes.Call, OneCallContext.GetCompiler);     // compiler
                    _il.Emit(OpCodes.Ldstr, id);                            // compiler id
                    _il.Emit(OpCodes.Ldstr, _context.CurrentNs);            // compiler id ns
                    _il.Emit(OpCodes.Call, typeof(SqlEmiterUtil)
                        .GetMethod(nameof(SqlEmiterUtil.EmiterFromId)));    // emiter
                    _il.Emit(OpCodes.Ldarg_0);                              // emiter ctx
                    _il.Emit(OpCodes.Call, typeof(IfUtils)
                        .GetMethod(nameof(IfUtils.ExecuteEmiter)));         // result<str>

                    // convert result<str> to str
                    _il.Emit(OpCodes.Dup);                                     // result<str> x 2
                    _il.Emit(OpCodes.Call, typeof(Result).GetTypeInfo()
                        .GetMethod("get_" + nameof(Result.IsSuccess)));        // result<str> bool
                    _il.Emit(OpCodes.Ldc_I4_1);                                // result<str> bool true
                    var ifIsSuccess = _il.DefineLabel();
                    _il.Emit(OpCodes.Beq, ifIsSuccess);                        // result<str> (jmp if equal)
                    _il.Emit(OpCodes.Ret);                                     // [exit-returned]

                    _il.MarkLabel(ifIsSuccess);                                // ifIsSuccess:
                    _il.Emit(OpCodes.Call, typeof(Result<string>).GetTypeInfo()
                        .GetMethod("get_" + nameof(Result<string>.Value)));    // str

                    var strLocal = _il.DeclareLocal(typeof(string));
                    _il.Emit(OpCodes.Stloc, strLocal);
                    _il.Emit(OpCodes.Ldloc_0);
                    _il.Emit(OpCodes.Ldloc, strLocal);
                    _il.Emit(OpCodes.Call, _appendCall);                       // [empty]
                    _il.MarkLabel(ifSkip);
                    return Result.Ok();
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

        public static Result<EmitFunction> CompileCore(
            CoreSqlContext coreSql,
            SdmapCompilerContext context,
            string functionName)
        {
            var visitor = new CoreSqlVisitor(context);
            return visitor.Process(coreSql, functionName)
                .OnSuccess(() => visitor.Function);
        }
    }
}

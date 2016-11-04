using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using sdmap.Functional;
using sdmap.Macros;
using sdmap.Parser.G4;
using sdmap.Parser.Utils;
using sdmap.Runtime;
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
    public abstract class CoreSqlVisitor : SdmapParserBaseVisitor<Result>
    {
        protected readonly SdmapContext _context;
        protected ILGenerator _il;
        protected int _stackPos;

        public EmitFunction Function { get; protected set; }

        protected CoreSqlVisitor(SdmapContext context)
        {
            _context = context;
        }

        protected abstract string GetFunctionName(ParserRuleContext parseRule);

        public override Result VisitNamedSql([NotNull] NamedSqlContext context)
        {
            return Process(context);
        }

        public override Result VisitUnnamedSql([NotNull] UnnamedSqlContext context)
        {
            return Process(context);
        }

        private Result Process(ParserRuleContext parseRule)
        {
            var fullName = GetFunctionName(parseRule);

            var method = new DynamicMethod(fullName,
                typeof(Result<string>), new[] { typeof(SdmapContext), typeof(object) });
            _il = method.GetILGenerator();

            var coreSqlContext = parseRule.GetChild<CoreSqlContext>(0);

            _il.DeclareLocal(typeof(StringBuilder));
            _il.Emit(OpCodes.Newobj, typeof(StringBuilder)
                .GetTypeInfo()
                .GetConstructor(Type.EmptyTypes));                                    // sb
            _il.Emit(OpCodes.Stloc_0);                                                // [empty]
                                                                                      
            return Visit(coreSqlContext)                                              
                .OnSuccess(() =>                                                      // [must be empty]
                {                                                                     
                    _il.Emit(OpCodes.Ldloc_0);                                        // sb
                    var okMethod = typeof(Result)
                        .GetTypeInfo()
                        .GetMethods()
                        .Single(x => x.IsGenericMethod && x.Name == "Ok")
                        .MakeGenericMethod(typeof(string));
                    _il.Emit(OpCodes.Call, typeof(StringBuilder)
                        .GetTypeInfo()
                        .GetMethod(nameof(StringBuilder.ToString), Type.EmptyTypes)); // str
                    _il.Emit(OpCodes.Call, okMethod);                                 // result<str>

                    _il.Emit(OpCodes.Ret);                                            // [returned]
                    Function = (EmitFunction)method.CreateDelegate(typeof(EmitFunction));
                });
        }

        public override Result VisitMacro([NotNull] MacroContext context)
        {
            var openMacro = context.GetToken(OpenMacro, 0);
            var macroName = LexerUtil.GetOpenMacroId(openMacro.GetText());

            _il.Emit(OpCodes.Ldarg_0);                                      // ctx
            _il.Emit(OpCodes.Ldstr, macroName);                             // ctx name
            _il.Emit(OpCodes.Ldarg_1);                                      // ctx name self

            var contexts = context.GetRuleContexts<MacroParameterContext>();
            _il.Emit(OpCodes.Ldc_I4, contexts.Length);                      // ctx name self
            _il.Emit(OpCodes.Newarr, typeof(object));                       // ctx name self args
            for (var i = 0; i < contexts.Length; ++i)
            {
                var arg = contexts[i];

                _il.Emit(OpCodes.Dup);                                      // .. -> args
                _il.Emit(OpCodes.Ldc_I4, i);                                // .. -> args idx

                if (arg.SYNTAX() != null)
                {
                    _il.Emit(OpCodes.Ldstr, arg.SYNTAX().GetText());        // .. -> args idx ele
                }
                else if (arg.NSSyntax() != null)
                {
                    _il.Emit(OpCodes.Ldstr, arg.SYNTAX().GetText());        // .. -> args idx ele
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
                        _il.Emit(OpCodes.Box);                              // .. -> args idx rele
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
                        var ctor = typeof(DateTime).GetTypeInfo().GetConstructor(new[] { typeof(long) });
                        _il.Emit(OpCodes.Newobj, ctor);                     // .. -> args idx date
                        _il.Emit(OpCodes.Box);                              // .. -> args idx rele
                    }
                    else
                    {
                        return result;
                    }
                }
                else if (arg.unnamedSql() != null)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new InvalidOperationException();
                }

                _il.Emit(OpCodes.Stelem_Ref);                               // -> ctx name self args
            }

            _il.Emit(OpCodes.Call, typeof(MacroManager).GetTypeInfo()
                .GetMethod(nameof(MacroManager.Execute)));                  // result<str>
            _il.Emit(OpCodes.Dup);                                          // result<str> x 2
            _il.Emit(OpCodes.Call, typeof(Result).GetTypeInfo()
                .GetMethod("get_" + nameof(Result.IsSuccess)));             // result<str> bool
            _il.Emit(OpCodes.Ldc_I4_1);                                     // result<str> bool true
            var ifIsSuccess = _il.DefineLabel();
            _il.Emit(OpCodes.Beq, ifIsSuccess);                             // result<str> (jmp if equal)
            _il.Emit(OpCodes.Ret);                                          // [exit-returned]

            _il.MarkLabel(ifIsSuccess);                                     // ifIsSuccess:
            _il.Emit(OpCodes.Call, typeof(Result<string>).GetTypeInfo()
                .GetMethod("get_" + nameof(Result<string>.Value)));         // str
            var strValue = _il.DeclareLocal(typeof(string));
            _il.Emit(OpCodes.Stloc, strValue);                              // [empty]
            _il.Emit(OpCodes.Ldloc_0);                                      // sb
            _il.Emit(OpCodes.Ldloc, strValue);                              // sb str
            _il.Emit(OpCodes.Call, typeof(StringBuilder)
                .GetTypeInfo().GetMethod(nameof(StringBuilder.Append),
                new[] { typeof(string), }));                                // sb+str
            _il.Emit(OpCodes.Pop);                                          // [empty]

            return Result.Ok();
        }

        public override Result VisitPlainText([NotNull] PlainTextContext context)
        {
            var text = context.GetToken(SQLText, 0);

            _il.Emit(OpCodes.Ldloc_0);                                             // sb
            _il.Emit(OpCodes.Ldstr, text.GetText());                               // sb str
            _il.Emit(OpCodes.Call, typeof(StringBuilder)
                .GetTypeInfo().GetMethod(nameof(StringBuilder.Append),
                new[] { typeof(string), }));                                       // sb+str
            _il.Emit(OpCodes.Pop);                                                 // [empty]
            return Result.Ok();
        }

        protected override Result AggregateResult(Result aggregate, Result nextResult)
        {
            if (aggregate == null && nextResult != null)
            {
                return nextResult;
            }
            if (aggregate != null && nextResult == null)
            {
                return aggregate;
            }
            else if (aggregate != null && nextResult != null)
            {
                if (aggregate.IsFailure)
                    return aggregate;
                if (nextResult.IsFailure)
                    return nextResult;
                return nextResult;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}

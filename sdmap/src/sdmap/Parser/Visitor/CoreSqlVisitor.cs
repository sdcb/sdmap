using sdmap.Functional;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;
using Antlr4.Runtime.Misc;
using sdmap.Parser.Utils;
using System.Text;
using sdmap.Runtime;
using sdmap.Macros;

namespace sdmap.Parser.Visitor
{
    public class CoreSqlVisitor : SdmapParserBaseVisitor<Result>
    {
        private readonly SdmapContext _context;
        private ILGenerator _il;

        public EmitFunction Function { get; private set; }

        private CoreSqlVisitor(SdmapContext context)
        {
            _context = context;
        }

        public override Result VisitNamedSql([NotNull] NamedSqlContext context)
        {
            var openSql = context.GetToken(OpenNamedSql, 0);
            var id = LexerUtil.GetOpenSqlId(openSql.GetText());
            var fullName = _context.GetFullName(id);
            
            var method = new DynamicMethod(fullName, 
                typeof(Result<string>), new[] { typeof(SdmapContext), typeof(object) });
            _il = method.GetILGenerator();

            var coreSqlContext = context.GetChild<CoreSqlContext>(0);

            _il.DeclareLocal(typeof(string));
            _il.Emit(OpCodes.Ldstr, string.Empty);         // ""
            _il.Emit(OpCodes.Stloc_0);                     // [empty]

            return Visit(coreSqlContext)
                .OnSuccess(() =>                           // [must be empty]
                {
                    _il.Emit(OpCodes.Ldloc_0);             // str
                    var okMethod = typeof(Result)
                        .GetTypeInfo()
                        .GetMethods()
                        .Single(x => x.IsGenericMethod && x.Name == "Ok")
                        .MakeGenericMethod(typeof(string));
                    _il.Emit(OpCodes.Call, okMethod);      // result<str>
                    
                    _il.Emit(OpCodes.Ret);                 // [returned]
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
            _il.Emit(OpCodes.Callvirt, typeof(Result).GetTypeInfo()
                .GetMethod("get_" + nameof(Result.IsSuccess)));             // result<str> bool
            _il.Emit(OpCodes.Ldc_I4_1);                                     // result<str> bool true
            var ifIsSuccess = _il.DefineLabel();
            _il.Emit(OpCodes.Beq, ifIsSuccess);                             // result<str> (jmp if equal)
            _il.Emit(OpCodes.Ret);                                          // [exit-returned]

            _il.MarkLabel(ifIsSuccess);                                     // ifIsSuccess:
            _il.Emit(OpCodes.Callvirt, typeof(Result<string>).GetTypeInfo()
                .GetMethod("get_" + nameof(Result<string>.Value)));         // str
            var strValue = _il.DeclareLocal(typeof(string));
            _il.Emit(OpCodes.Stloc, strValue);                              // [empty]
            _il.Emit(OpCodes.Ldloc_0);                                      // result
            _il.Emit(OpCodes.Ldloc, strValue);                              // result str
            _il.Emit(OpCodes.Call, typeof(string).GetTypeInfo().GetMethod(
                nameof(string.Concat), 
                new[] { typeof(string), typeof(string) }));                 // result+str
            _il.Emit(OpCodes.Stloc_0);                                      // [stored]

            return Result.Ok();
        }

        public override Result VisitPlainText([NotNull] PlainTextContext context)
        {
            var text = context.GetToken(SQLText, 0);

            _il.Emit(OpCodes.Ldloc_0);                                             // str
            _il.Emit(OpCodes.Ldstr, text.GetText());                               // str str
            _il.Emit(OpCodes.Call, typeof(string).GetTypeInfo().GetMethod(
                nameof(string.Concat), new[] { typeof(string), typeof(string) })); // str+str
            _il.Emit(OpCodes.Stloc_0);                                             // [stored]
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

        public static CoreSqlVisitor Create(SdmapContext context)
        {
            return new CoreSqlVisitor(context);
        }

        public static Result<EmitFunction> Compile(NamedSqlContext parseTree, SdmapContext context)
        {
            var visitor = Create(context);
            return visitor.Visit(parseTree)
                .OnSuccess(() => visitor.Function);
        }

        public static CoreSqlVisitor CreateEmpty()
        {
            return new CoreSqlVisitor(SdmapContext.CreateEmpty());
        }
    }
}

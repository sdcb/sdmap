using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using sdmap.Functional;
using sdmap.Parser.Visitor;
using sdmap.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.Compiler
{
    public class SqlEmiter
    {
        private ParserRuleContext _parseTree;
        private string _ns;
        private readonly CompileFunction _compiler;

        public EmitFunction Emiter { get; private set; }

        public SqlEmiter(
            ParserRuleContext parseTree, 
            string ns, 
            CompileFunction compiler)
        {
            _parseTree = parseTree;
            _ns = ns;
            _compiler = compiler;
        }

        public Result EnsureCompiled(SdmapCompilerContext context)
        {
            if (Emiter != null)
                return Result.Ok();

            return CompileInternal(context)
                .OnSuccess(v =>
                {
                    Emiter = v;
                    _parseTree = null;
                });
        }

        public Result<string> TryEmit(OneCallContext ctx)
        {
            return EnsureCompiled(ctx.Compiler)
                .OnSuccess(() => Emiter(ctx))
                .Unwrap();
        }

        public string Emit(OneCallContext ctx)
        {
            return TryEmit(ctx).Value;
        }

        private Result<EmitFunction> CompileInternal(SdmapCompilerContext context)
        {
            if (_ns != "")
                context.NsStack.Push(_ns);

            var result = _compiler(context);

            if (_ns != "")
                context.NsStack.Pop();

            return result;
        }
    }    

    public delegate Result<EmitFunction> CompileFunction(
        SdmapCompilerContext context);

    public delegate Result<string> EmitFunction(OneCallContext parent);
}

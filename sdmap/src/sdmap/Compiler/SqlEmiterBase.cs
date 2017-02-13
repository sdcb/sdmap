using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using sdmap.Functional;
using sdmap.Parser.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Compiler
{
    public abstract class SqlEmiterBase
    {
        protected ParserRuleContext _parseTree;
        protected string _ns;
        public EmitFunction Emiter { get; private set; }

        protected SqlEmiterBase(ParserRuleContext parseTree, string ns)
        {
            _parseTree = parseTree;
            _ns = ns;
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

        public Result<string> TryEmit(object v, SdmapCompilerContext context)
        {
            return EnsureCompiled(context)
                .OnSuccess(() => Emiter(context, v))
                .Unwrap();
        }

        public string Emit(object v, SdmapCompilerContext context)
        {
            return TryEmit(v, context).Value;
        }

        private Result<EmitFunction> CompileInternal(SdmapCompilerContext context)
        {
            if (_ns != "")
                context.NsStack.Push(_ns);

            var result = Compile(context);

            if (_ns != "")
                context.NsStack.Pop();

            return result;
        }

        protected abstract Result<EmitFunction> Compile(SdmapCompilerContext context);
    }

    public delegate Result<string> EmitFunction(SdmapCompilerContext context, object obj);
}

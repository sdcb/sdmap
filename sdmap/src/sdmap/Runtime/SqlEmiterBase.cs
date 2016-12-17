using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using sdmap.Functional;
using sdmap.Parser.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Runtime
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

        public Result EnsureCompiled(SdmapContext context)
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

        public Result<string> TryEmit(object v, SdmapContext context)
        {
            return EnsureCompiled(context)
                .OnSuccess(() => Emiter(context, v))
                .Unwrap();
        }

        public string Emit(object v, SdmapContext context)
        {
            return TryEmit(v, context).Value;
        }

        private Result<EmitFunction> CompileInternal(SdmapContext context)
        {
            if (_ns != "")
                context.NsStack.Push(_ns);

            var result = Compile(context);

            if (_ns != "")
                context.NsStack.Pop();

            return result;
        }

        protected abstract Result<EmitFunction> Compile(SdmapContext context);
    }

    public delegate Result<string> EmitFunction(SdmapContext context, object obj);
}

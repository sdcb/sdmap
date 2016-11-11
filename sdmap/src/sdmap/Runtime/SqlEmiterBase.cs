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
        private EmitFunction _emiter;

        protected SqlEmiterBase(ParserRuleContext parseTree, string ns)
        {
            _parseTree = parseTree;
            _ns = ns;
        }

        public Result EnsureCompiled(SdmapContext context)
        {
            if (_emiter != null)
                return Result.Ok();

            return CompileInternal(context)
                .OnSuccess(v =>
                {
                    _emiter = v;
                    _parseTree = null;
                });
        }

        public Result<string> TryEmit(object v, SdmapContext context)
        {
            return EnsureCompiled(context)
                .OnSuccess(() => _emiter(context, v))
                .Unwrap();
        }

        public string Emit(object v, SdmapContext context)
        {
            return TryEmit(v, context).Value;
        }

        private Result<EmitFunction> CompileInternal(SdmapContext context)
        {
            context.NsStack.Push(_ns);
            var result = Compile(context);
            context.NsStack.Pop();
            return result;
        }

        protected abstract Result<EmitFunction> Compile(SdmapContext context);
    }

    public delegate Result<string> EmitFunction(SdmapContext context, object obj);
}

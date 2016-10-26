using Antlr4.Runtime.Tree;
using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Parser.Context
{
    public class SqlEmiter
    {
        private IParseTree _parseTree;
        private EmitFunction _emiter;

        private SqlEmiter(IParseTree parseTree)
        {
            _parseTree = parseTree;
        }

        private Result<EmitFunction> Compile(SdmapContext context)
        {
            throw new NotImplementedException();
        }

        public Result EnsureCompiled(SdmapContext context)
        {
            if (_emiter != null)
                return Result.Ok();

            return Compile(context)
                .OnSuccess(v => _emiter = v);
        }

        public Result<string> TryEmit(object v, SdmapContext context)
        {
            return EnsureCompiled(context)
                .OnSuccess(() => _emiter(v));
        }

        public string Emit(object v, SdmapContext context)
        {
            return TryEmit(v, context).Value;
        }

        public static SqlEmiter Create(IParseTree parseTree)
        {
            return new SqlEmiter(parseTree);
        }
    }

    public delegate string EmitFunction(object obj);
}

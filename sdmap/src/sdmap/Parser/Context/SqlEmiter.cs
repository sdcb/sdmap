using Antlr4.Runtime.Tree;
using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Parser.Context
{
    using Visitor;
    using static G4.SdmapParser;

    public class SqlEmiter
    {
        private NamedSqlContext _parseTree;
        private EmitFunction _emiter;

        private SqlEmiter(NamedSqlContext parseTree)
        {
            _parseTree = parseTree;
        }

        public Result EnsureCompiled(SdmapContext context)
        {
            if (_emiter != null)
                return Result.Ok();

            return Compile(_parseTree, context)
                .OnSuccess(v =>
                {
                    _emiter = v;
                    _parseTree = null;
                });
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

        public static SqlEmiter Create(NamedSqlContext parseTree)
        {
            return new SqlEmiter(parseTree);
        }

        public static Result<EmitFunction> Compile(NamedSqlContext parseTree, SdmapContext context)
        {
            return CoreSqlVisitor.Compile(parseTree, context);
        }
    }

    public delegate string EmitFunction(object obj);
}

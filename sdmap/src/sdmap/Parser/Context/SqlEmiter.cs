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
    using ContextType = SortedDictionary<string, SqlEmiter>;

    public class SqlEmiter
    {
        private CoreSqlContext _parseTree;
        private EmitFunction _emiter;

        private SqlEmiter(CoreSqlContext parseTree)
        {
            _parseTree = parseTree;
        }

        public Result EnsureCompiled(ContextType context)
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

        public Result<string> TryEmit(object v, ContextType context)
        {
            return EnsureCompiled(context)
                .OnSuccess(() => _emiter(v));
        }

        public string Emit(object v, ContextType context)
        {
            return TryEmit(v, context).Value;
        }

        public static SqlEmiter Create(CoreSqlContext parseTree)
        {
            return new SqlEmiter(parseTree);
        }

        public static Result<EmitFunction> Compile(CoreSqlContext parseTree, ContextType context)
        {
            return CoreSqlVisitor.Compile(parseTree, context);
        }
    }

    public delegate string EmitFunction(object obj);
}

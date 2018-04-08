using sdmap.Functional;
using sdmap.Macros.Implements;
using sdmap.Parser.Visitor;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.Compiler
{
    public abstract class SegmentDef
    {
        public string Id { get; }

        public bool Enabled { get; protected set; }

        protected string _finalSql = string.Empty;

        public abstract Result<TurnOnResult> TurnOn(OneCallContext ctx);

        public SegmentDef(string id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return _finalSql;
        }
    }

    public class TurnOnResult
    {
        public bool HasMore { get; set; }

        public string Sql { get; set; }
    }

    internal class RawSegmentDef : SegmentDef
    {
        public string Sql { get; }

        public RawSegmentDef(string id, string sql) : base(id)
        {
            Sql = sql;
        }

        public override Result<TurnOnResult> TurnOn(OneCallContext ctx)
        {
            Enabled = true;
            _finalSql = Sql;
            return Result.Ok(new TurnOnResult
            {
                Sql = _finalSql,
                HasMore = false
            });
        }
    }

    internal class EmiterSegmentDef : SegmentDef
    {
        public EmitFunction Emiter { get; }

        public EmiterSegmentDef(string id, EmitFunction emiter) : base(id)
        {
            Emiter = emiter;
        }

        public override Result<TurnOnResult> TurnOn(OneCallContext ctx)
        {
            Enabled = true;
            int currentCount = ctx.Deps.Count;            
            var result = MacroUtil.EvalToString(Emiter, ctx, ctx.Obj);
            bool hasMore = ctx.Deps.Count > currentCount;
            if (result.IsSuccess) _finalSql = result.Value;
            return result.OnSuccess(s => new TurnOnResult
            {
                Sql = s, 
                HasMore = hasMore
            });
        }
    }
}

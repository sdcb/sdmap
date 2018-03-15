using sdmap.Functional;
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

        public abstract (Result<string> result, bool hasMore) TurnOn(OneCallContext ctx);

        public SegmentDef(string id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return _finalSql;
        }
    }

    public class RawSegmentDef : SegmentDef
    {
        public string Sql { get; }

        public RawSegmentDef(string id, string sql) : base(id)
        {
            Sql = sql;
        }

        public override (Result<string> result, bool hasMore) TurnOn(OneCallContext ctx)
        {
            Enabled = true;
            _finalSql = Sql;
            return (Result.Ok(_finalSql), false);
        }
    }

    public class EmiterSegmentDef : SegmentDef
    {
        public EmitFunction Emiter { get; }

        public EmiterSegmentDef(string id, EmitFunction emiter) : base(id)
        {
            Emiter = emiter;
        }

        public override (Result<string> result, bool hasMore) TurnOn(OneCallContext ctx)
        {
            Enabled = true;
            int currentCount = ctx.Deps.Count;
            Result<string> result = Emiter(ctx.Dig(ctx.Obj));
            bool hasMore = ctx.Deps.Count > currentCount;
            if (result.IsSuccess) _finalSql = result.Value;

            return (result, hasMore);
        }
    }
}

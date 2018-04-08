using sdmap.Compiler;
using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sdmap.Parser.Visitor
{
    internal class CoreSqlVisitorHelper
    {
        public static Result<string> CombineDeps(OneCallContext ctx)
        {
            bool shouldContinue;
            do
            {
                shouldContinue = false;
                foreach (SegmentDef def in ctx.Defs)
                {
                    if (def.Enabled)
                        continue;

                    if (!ctx.Deps.Contains(def.Id))
                        continue;

                    var result = def.TurnOn(ctx);
                    if (result.IsFailure) return result.Map(r => r.Sql);

                    if (result.Value.HasMore) shouldContinue = true;
                }
            } while (shouldContinue);

            return CombineStrings(ctx);
        }

        public static Result<string> CombineStrings(OneCallContext ctx)
        {
            return Result.Ok(string.Concat(ctx.Fragments));
        }
    }
}

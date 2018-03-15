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
        //public static Result<string> CombineDeps(string result, OneCallContext ctx)
        //{
        //    var defs = ctx.Defs.ToList();
        //    bool shouldContinue;
        //    do
        //    {
        //        shouldContinue = false;
        //        if (ctx.Defs.Count == 0)
        //            return Result.Ok(result);

        //        var toRemove = new List<SegmentDef>();
        //        foreach (var def in ctx.Defs)
        //        {
        //            if (ctx.Deps.Contains(def.Id))
        //            {
        //                defs.Remove(def);
        //                var replaceKey = $"<?{def.Id}>";
        //                if (def.Sql != null)
        //                {
        //                    result = result.Replace(replaceKey, def.Sql);
        //                }
        //                else if (def.Emiter != null)
        //                {
        //                    int depCount = ctx.Deps.Count;
        //                    Result<string> partial = def.Emiter(ctx);
        //                    if (partial.IsFailure) return partial;

        //                    if (depCount < ctx.Deps.Count) shouldContinue = true;
        //                    result = result.Replace(replaceKey, partial.Value);
        //                }
        //            }
        //        }
        //    } while (shouldContinue);

        //    foreach (var def in defs)
        //    {
        //        var replaceKey = $"<?{def.Id}>";
        //        result = result.Replace(replaceKey, "");
        //    }

        //    return Result.Ok(result);
        //}

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

                    var (result, hasMore) = def.TurnOn(ctx);
                    if (result.IsFailure) return result;

                    if (hasMore) shouldContinue = true;
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

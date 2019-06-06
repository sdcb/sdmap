using sdmap.Compiler;
using sdmap.Functional;
using sdmap.Macros.Implements;

namespace sdmap.Utils
{
    internal static class IfUtils
    {
        public static bool PropertyExistsAndEvalToTrue(object obj, string propName)
        {
            var val = DynamicRuntimeMacros.GetPropValue(obj, propName);
            if (val is bool) return (bool)val;
            if (val is bool?) return ((bool?)val).GetValueOrDefault();

            return false;
        }

        public static object LoadProp(object obj, string propName)
            => DynamicRuntimeMacros.GetPropValue(obj, propName);

        public static Result<string> ExecuteEmiter(EmitFunction ef, OneCallContext ctx)
            => ef(ctx.Dig(ctx.Obj));
    }
}

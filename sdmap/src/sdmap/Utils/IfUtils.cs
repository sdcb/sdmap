using sdmap.Compiler;
using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace sdmap.Utils
{
    internal static class IfUtils
    {
        public static bool PropertyExistsAndEvalToTrue(object obj, string propName)
        {
            if (obj == null) return false;

            var prop = obj.GetType().GetTypeInfo().GetProperty(propName);
            if (prop == null) return false;

            var val = prop.GetValue(obj);
            if (val is bool) return (bool)val;

            return true;
        }

        public static object LoadProp(object obj, string propName)
        {
            if (obj == null) return null;

            var prop = obj.GetType().GetTypeInfo().GetProperty(propName);
            if (prop == null) return null;

            return prop.GetValue(obj);
        }

        public static Result<string> ExecuteEmiter(EmitFunction ef, SdmapCompilerContext ctx, object obj)
        {
            return ef(ctx, obj);
        }
    }
}

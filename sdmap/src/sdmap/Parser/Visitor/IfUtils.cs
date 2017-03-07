using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace sdmap.Parser.Visitor
{
    public static class IfUtils
    {
        public static bool PropertyExistsAndEvalToTrue(object obj, string propName)
        {
            if (obj == null) return false;

            var prop = obj.GetType().GetTypeInfo().GetProperty(propName);
            if (prop == null) return false;

            var val = prop.GetValue(prop);
            if (!(val is bool)) return false;
            return true;
        }
    }
}

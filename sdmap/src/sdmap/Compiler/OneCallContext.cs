using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace sdmap.Compiler
{
    public class OneCallContext
    {
        public OneCallContext(SdmapCompilerContext compilerContext, object obj)
        {
            Compiler = compilerContext;
            Obj = obj;
            Defs = new List<KeyValuePair<string, object>>();
            Deps = new HashSet<string>();
        }

        public OneCallContext(SdmapCompilerContext compilerContext, object obj,
            List<KeyValuePair<string, object>> defs, 
            HashSet<string> deps)
        {
            Compiler = compilerContext;
            Obj = obj;
            Defs = defs;
            Deps = deps;
        }

        public SdmapCompilerContext Compiler { get; }

        public object Obj { get; }

        public List<KeyValuePair<string, object>> Defs { get; }
            = new List<KeyValuePair<string, object>>(); // object must be string or EmitFunction

        public HashSet<string> Deps { get; }
            = new HashSet<string>();

        public OneCallContext DupSelf(object newSelf)
        {
            return new OneCallContext(Compiler, newSelf, Defs, Deps);
        }

        public static OneCallContext CreateEmpty()
        {
            return new OneCallContext(SdmapCompilerContext.CreateEmpty(), null);
        }

        public static OneCallContext CreateByObj(object obj)
        {
            return new OneCallContext(SdmapCompilerContext.CreateEmpty(), obj);
        }

        private static Type ThisType = typeof(OneCallContext);
        internal static MethodInfo GetCompiler = ThisType.GetMethod("get_" + nameof(Compiler));
        internal static MethodInfo GetObj = ThisType.GetMethod("get_" + nameof(Obj));
        internal static MethodInfo GetDefs = ThisType.GetMethod("get_" + nameof(Defs));
        internal static MethodInfo GetDeps = ThisType.GetMethod("get_" + nameof(Deps));
    }
}

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
            Defs = new List<SegmentDef>();
            Deps = new HashSet<string>();
        }

        public OneCallContext(SdmapCompilerContext compilerContext, object obj,
            List<SegmentDef> defs, 
            HashSet<string> deps, 
            List<object> fragments)
        {
            Compiler = compilerContext;
            Obj = obj;
            Defs = defs;
            Deps = deps;
            Fragments = fragments;
        }

        public int Level { get; private set; }

        public List<object> Fragments { get; private set; }
            = new List<object>();

        public bool IsRoot => Level == 0;

        public bool IsChild => Level > 0;

        public SdmapCompilerContext Compiler { get; }

        public object Obj { get; }

        public List<SegmentDef> Defs { get; }
            = new List<SegmentDef>();

        public HashSet<string> Deps { get; }
            = new HashSet<string>();

        public OneCallContext Dig(object newSelf)
        {
            return new OneCallContext(Compiler, newSelf, Defs, Deps, Fragments)
            {
                Level = Level + 1
            };
        }

        public OneCallContext DigNewFragments(object newSelf)
        {
            return new OneCallContext(Compiler, newSelf, Defs, Deps, new List<object>())
            {
                Level = Level + 1
            };
        }

        public OneCallContext DupNewFragments()
        {
            return new OneCallContext(Compiler, Obj, Defs, Deps, new List<object>())
            {
                Level = Level + 1
            };
        }

        public static OneCallContext CreateEmpty()
        {
            return new OneCallContext(SdmapCompilerContext.CreateEmpty(), null);
        }

        internal static OneCallContext CreateByObj(object obj)
        {
            return new OneCallContext(SdmapCompilerContext.CreateEmpty(), obj);
        }

        private static Type ThisType = typeof(OneCallContext);
        internal static MethodInfo GetIsRoot = ThisType.GetMethod("get_" + nameof(IsRoot));
        internal static MethodInfo GetIsChild = ThisType.GetMethod("get_" + nameof(IsChild));
        internal static MethodInfo GetTempStore = ThisType.GetMethod("get_" + nameof(Fragments));
        internal static MethodInfo GetCompiler = ThisType.GetMethod("get_" + nameof(Compiler));
        internal static MethodInfo GetObj = ThisType.GetMethod("get_" + nameof(Obj));
        internal static MethodInfo GetDefs = ThisType.GetMethod("get_" + nameof(Defs));
        internal static MethodInfo GetDeps = ThisType.GetMethod("get_" + nameof(Deps));
        internal static MethodInfo CallDupNewFragments = ThisType.GetMethod(nameof(DupNewFragments));
    }
}

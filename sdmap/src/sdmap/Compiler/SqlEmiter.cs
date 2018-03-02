using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using sdmap.Functional;
using sdmap.Parser.Visitor;
using sdmap.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.Compiler
{
    public class SqlEmiter
    {
        private ParserRuleContext _parseTree;
        private string _ns;
        private readonly CompileFunction _compiler;

        public EmitFunction Emiter { get; private set; }

        public SqlEmiter(
            ParserRuleContext parseTree, 
            string ns, 
            CompileFunction compiler)
        {
            _parseTree = parseTree;
            _ns = ns;
            _compiler = compiler;
        }

        public Result EnsureCompiled(SdmapCompilerContext context)
        {
            if (Emiter != null)
                return Result.Ok();

            return CompileInternal(context)
                .OnSuccess(v =>
                {
                    Emiter = v;
                    _parseTree = null;
                });
        }

        public Result<string> TryEmit(ParentEmiterContext ctx)
        {
            return EnsureCompiled(ctx.Compiler)
                .OnSuccess(() => Emiter(ctx))
                .Unwrap();
        }

        public string Emit(ParentEmiterContext ctx)
        {
            return TryEmit(ctx).Value;
        }

        private Result<EmitFunction> CompileInternal(SdmapCompilerContext context)
        {
            if (_ns != "")
                context.NsStack.Push(_ns);

            var result = _compiler(context);

            if (_ns != "")
                context.NsStack.Pop();

            return result;
        }
    }    

    public delegate Result<EmitFunction> CompileFunction(
        SdmapCompilerContext context);

    public delegate Result<string> EmitFunction(ParentEmiterContext parent);
    
    public class ParentEmiterContext
    {
        public ParentEmiterContext(SdmapCompilerContext compilerContext, object obj)
        {
            Compiler = compilerContext;
            Obj = obj;
            Defs = new List<KeyValuePair<string, Result<string>>>();
            Deps = new HashSet<string>();
        }

        public ParentEmiterContext(SdmapCompilerContext compilerContext, object obj,
            List<KeyValuePair<string, Result<string>>> defs, 
            HashSet<string> deps)
        {
            Compiler = compilerContext;
            Obj = obj;
            Defs = defs;
            Deps = deps;
        }

        public SdmapCompilerContext Compiler { get; }

        public object Obj { get; }

        public List<KeyValuePair<string, Result<string>>> Defs { get; }
            = new List<KeyValuePair<string, Result<string>>>();

        public HashSet<string> Deps { get; }
            = new HashSet<string>();

        public static ParentEmiterContext CreateEmpty()
        {
            return new ParentEmiterContext(SdmapCompilerContext.CreateEmpty(), null);
        }

        public static ParentEmiterContext CreateByObj(object obj)
        {
            return new ParentEmiterContext(SdmapCompilerContext.CreateEmpty(), obj);
        }

        private static Type ThisType = typeof(ParentEmiterContext);
        internal static MethodInfo GetCompiler = ThisType.GetMethod("get_" + nameof(Compiler));
        internal static MethodInfo GetObj = ThisType.GetMethod("get_" + nameof(Obj));
        internal static MethodInfo GetDefs = ThisType.GetMethod("get_" + nameof(Defs));
        internal static MethodInfo GetDeps = ThisType.GetMethod("get_" + nameof(Deps));
    }
}

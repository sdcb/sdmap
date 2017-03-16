using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Macros.Implements;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static sdmap.Macros.Implements.MacroUtil;

namespace sdmap.Macros
{
    public class MacroManager
    {
        public Dictionary<string, Macro> Methods { get; } = new Dictionary<string, Macro>();

        public MacroManager()
        {
            AddImplements(typeof(CommonMacros));
        }

        public void AddImplements(Type type)
        {
            var methods = GetTypeMacroMethods(type)
                .Select(ToSdmapMacro);

            foreach (var method in methods)
            {
                Methods.Add(method.Name, method);
            }
        }

        public Result Add(Macro macro)
        {
            if (macro == null)
                return Result.Fail($"{macro} requires not null.");
            if (macro.Method == null)
                return Result.Fail($"{nameof(macro.Method)} requires not null.");
            if (string.IsNullOrWhiteSpace(macro.Name))
                return Result.Fail($"{nameof(macro.Name)} requires not empty.");
            if (Methods.ContainsKey(macro.Name))
                return Result.Fail($"'{macro.Name}' already exists in macro manager.");

            if (macro.Arguments == null)
                macro.Arguments = new SdmapTypes[0];

            Methods.Add(macro.Name, macro);
            return Result.Ok();
        }

        public static Result<string> Execute(SdmapCompilerContext context, string name, 
            string ns, object self, object[] arguments)
        {
            Macro macro;
            if (!context.MacroManager.Methods.TryGetValue(name, out macro))
            {
                return Result.Fail<string>($"Macro: '{name}' cannot be found.");
            }

            var rtCheck = RuntimeCheck(arguments, macro);
            if (rtCheck.IsFailure)
            {
                return Result.Fail<string>(rtCheck.Error);
            }

            return macro.Method(context, ns, self, arguments);
        }
    }
}

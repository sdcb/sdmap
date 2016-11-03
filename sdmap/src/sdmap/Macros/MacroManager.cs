using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Macros.Implements;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static sdmap.Macros.MacroUtil;

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

        public static Result<string> Execute(SdmapContext context, string name, object self, object[] arguments)
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

            return macro.Function(context, self, arguments);
        }
    }
}

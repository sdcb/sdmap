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
    public class SdmapMacroManager
    {
        public Dictionary<string, SdmapMacro> Methods { get; } = new Dictionary<string, SdmapMacro>();

        public SdmapMacroManager()
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

        public Result<string> Execute(string name, SdmapContext context, object self, object[] arguments)
        {
            SdmapMacro macro;
            if (!Methods.TryGetValue(name, out macro))
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

using sdmap.Macros.Attributes;
using sdmap.Parser.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace sdmap.Macros
{
    public static class MacroUtil
    {
        public static IEnumerable<MethodInfo> GetTypeAvailableMethods(Type type)
        {
            var target = typeof(SdmapMacroDelegate).GetTypeInfo().GetMethod("Invoke");

            return type
                .GetTypeInfo()
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(method =>
                {
                    return
                        Enumerable.SequenceEqual(
                            method.GetParameters().Select(x => x.ParameterType),
                            target.GetParameters().Select(x => x.ParameterType)) &&
                        method.ReturnType == target.ReturnType;
                });
        }

        public static SdmapMacro ToSdmapMacro(MethodInfo method)
        {
            var nameAttr = method.GetCustomAttribute<MacroNameAttribute>();
            var argsAttr = method.GetCustomAttributes<MacroArgumentsAttribute>();

            return new SdmapMacro
            {
                Name = nameAttr?.Name ?? method.Name,
                ArgumentsGroups = argsAttr.Select(x => x.Arguments), 
                Function = (SdmapMacroDelegate)method.CreateDelegate(typeof(SdmapMacroDelegate))
            };
        }
    }
}

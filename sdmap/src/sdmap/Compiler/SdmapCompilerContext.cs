using sdmap.Functional;
using sdmap.Macros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Compiler
{
    public class SdmapCompilerContext
    {
        public SortedDictionary<string, SqlEmiterBase> Emiters { get; }

        public Stack<string> NsStack { get; }

        public MacroManager MacroManager { get; } = new MacroManager();

        private SdmapCompilerContext(SortedDictionary<string, SqlEmiterBase> emiters, Stack<string> nsStacks)
        {
            Emiters = emiters;
            NsStack = nsStacks;
        }

        public string CurrentNs => string.Join(".", NsStack.Reverse());

        public string GetFullNameInCurrentNs(string contextId)
        {
            return string.Join(".", NsStack.Reverse().Concat(new List<string> { contextId }));
        }

        public Result<SqlEmiterBase> TryGetEmiter(string contextId, string currentNs)
        {
            var nss = currentNs.Split('.');
            for (var i = nss.Length; i >= 0; --i)
            {
                var fullName = string.Join(".",
                    nss.Take(i).Concat(new List<string> { contextId }));
                if (Emiters.ContainsKey(fullName))
                {
                    return Result.Ok(Emiters[fullName]);
                }
            }
            return Result.Fail<SqlEmiterBase>($"Syntax '{contextId}' not found in current scope.");
        }

        public SqlEmiterBase GetEmiter(string contextId, string currentNs)
        {
            return TryGetEmiter(contextId, currentNs).Value;
        }

        public Result TryAdd(string contextId, SqlEmiterBase emiter)
        {
            var fullName = GetFullNameInCurrentNs(contextId);
            if (Emiters.ContainsKey(fullName))
            {
                return Result.Fail($"Syntax already defined: '{fullName}'.");
            }

            Emiters.Add(fullName, emiter);
            return Result.Ok();
        }

        public static SdmapCompilerContext CreateEmpty()
        {
            return CreateByContext(new SortedDictionary<string, SqlEmiterBase>());
        }

        public static SdmapCompilerContext CreateByContext(SortedDictionary<string, SqlEmiterBase> context)
        {
            return Create(context, new Stack<string>());
        }

        public static SdmapCompilerContext Create(SortedDictionary<string, SqlEmiterBase> emiters, Stack<string> nsStack)
        {
            return new SdmapCompilerContext(emiters, nsStack);
        }
    }
}

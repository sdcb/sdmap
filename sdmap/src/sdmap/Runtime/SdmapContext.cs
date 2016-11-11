using sdmap.Functional;
using sdmap.Macros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Runtime
{
    public class SdmapContext
    {
        public SortedDictionary<string, SqlEmiterBase> Emiters { get; }

        public Stack<string> NsStack { get; }

        public MacroManager MacroManager { get; } = new MacroManager();

        private SdmapContext(SortedDictionary<string, SqlEmiterBase> emiters, Stack<string> nsStacks)
        {
            Emiters = emiters;
            NsStack = nsStacks;
        }

        public string CurrentNs =>
            NsStack.Count == 0 ? string.Empty : NsStack.Peek();

        public string GetFullName(string contextId)
        {
            if (contextId.Contains(".") || CurrentNs == string.Empty)
            {
                return contextId;
            }
            else
            {
                return $"{CurrentNs}.{contextId}";
            }
        }

        public Result<SqlEmiterBase> TryGetEmiter(string contextId)
        {
            var fullName = GetFullName(contextId);
            if (Emiters.ContainsKey(fullName))
            {
                return Result.Ok(Emiters[fullName]);
            }
            else
            {
                return Result.Fail<SqlEmiterBase>($"Syntax '{contextId}' not found in current scope.");
            }
        }

        public SqlEmiterBase GetEmiter(string contextId)
        {
            return TryGetEmiter(contextId).Value;
        }

        public Result TryAdd(string contextId, SqlEmiterBase emiter)
        {
            var fullName = GetFullName(contextId);
            if (Emiters.ContainsKey(fullName))
            {
                return Result.Fail($"Syntax already defined: '{fullName}'.");
            }

            Emiters.Add(fullName, emiter);
            return Result.Ok();
        }

        public static SdmapContext CreateEmpty()
        {
            return CreateByContext(new SortedDictionary<string, SqlEmiterBase>());
        }

        public static SdmapContext CreateByContext(SortedDictionary<string, SqlEmiterBase> context)
        {
            return Create(context, new Stack<string>());
        }

        public static SdmapContext Create(SortedDictionary<string, SqlEmiterBase> emiters, Stack<string> nsStack)
        {
            return new SdmapContext(emiters, nsStack);
        }
    }
}

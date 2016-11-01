using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Runtime
{
    public class SdmapContext
    {
        public SortedDictionary<string, SqlEmiter> Emiters { get; }

        public Stack<string> NsStack { get; }

        private SdmapContext(SortedDictionary<string, SqlEmiter> emiters, Stack<string> nsStacks)
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

        public Result<SqlEmiter> TryGetEmiter(string contextId)
        {
            var fullName = GetFullName(contextId);
            if (Emiters.ContainsKey(fullName))
            {
                return Result.Ok(Emiters[fullName]);
            }
            else
            {
                return Result.Fail<SqlEmiter>($"Syntax '{contextId}' not found in current scope.");
            }
        }

        public Result TryAdd(string contextId, SqlEmiter emiter)
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
            return CreateByContext(new SortedDictionary<string, SqlEmiter>());
        }

        public static SdmapContext CreateByContext(SortedDictionary<string, SqlEmiter> context)
        {
            return Create(context, new Stack<string>());
        }

        public static SdmapContext Create(SortedDictionary<string, SqlEmiter> emiters, Stack<string> nsStack)
        {
            return new SdmapContext(emiters, nsStack);
        }
    }
}

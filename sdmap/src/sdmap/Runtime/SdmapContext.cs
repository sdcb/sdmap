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

        public string CurrentNs => string.Join(".", NsStack.Reverse());

        public string GetFullNameInCurrentNs(string contextId)
        {
            return string.Join(".", NsStack.Reverse().Concat(new List<string> { contextId }));
        }

        public void EnterNs(string ns)
        {
            if (ns != "")
            {
                foreach (var item in ns.Split('.').Reverse())
                {
                    NsStack.Push(item);
                }
            }
        }

        public void LeaveNs()
        {
            NsStack.Clear();
        }

        public Result<SqlEmiterBase> TryGetEmiter(string contextId)
        {
            for (var i = 0; i <= NsStack.Count; ++i)
            {
                var fullName = string.Join(".",
                    NsStack.Reverse().Skip(i).Concat(new List<string> { contextId }));
                if (Emiters.ContainsKey(fullName))
                {
                    return Result.Ok(Emiters[fullName]);
                }
            }
            return Result.Fail<SqlEmiterBase>($"Syntax '{contextId}' not found in current scope.");
        }

        public SqlEmiterBase GetEmiter(string contextId)
        {
            return TryGetEmiter(contextId).Value;
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

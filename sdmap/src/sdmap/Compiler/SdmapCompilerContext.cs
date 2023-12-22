using sdmap.Functional;
using sdmap.Macros;
using System.Collections.Generic;
using System.Linq;

namespace sdmap.Compiler
{
    public class SdmapCompilerContext
    {
        public Dictionary<string, SqlEmiter> Emiters { get; }

        public Stack<string> NsStack { get; }

        public MacroManager MacroManager { get; } = new MacroManager();

        private SdmapCompilerContext(Dictionary<string, SqlEmiter> emiters, Stack<string> nsStacks)
        {
            Emiters = emiters;
            NsStack = nsStacks;
        }

        public string CurrentNs => string.Join(".", NsStack.Reverse());

        public string GetFullNameInCurrentNs(string contextId)
        {
            return string.Join(".", NsStack.Reverse().Concat(new List<string> { contextId }));
        }

        public Result<SqlEmiter> TryGetEmiter(string contextId, string currentNs)
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
            return Result.Fail<SqlEmiter>($"Syntax '{contextId}' not found in current scope.");
        }

        public SqlEmiter GetEmiter(string contextId, string currentNs)
        {
            return TryGetEmiter(contextId, currentNs).Value;
        }

        public Result TryAdd(string contextId, SqlEmiter emiter)
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
            return CreateByContext([]);
        }

        public static SdmapCompilerContext CreateByContext(Dictionary<string, SqlEmiter> context)
        {
            return Create(context, new Stack<string>());
        }

        public static SdmapCompilerContext Create(Dictionary<string, SqlEmiter> emiters, Stack<string> nsStack)
        {
            return new SdmapCompilerContext(emiters, nsStack);
        }
    }
}

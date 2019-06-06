using Antlr4.Runtime;

namespace sdmap.Utils
{
    internal static class NameUtil
    {
        public static string GetFunctionName(ParserRuleContext context)
        {
            if (context == null) return "<Empty>";
            return "Unnamed" + HashUtil.Base64SHA256(context.GetText());
        }

        public static bool IsNamed(string name)
        {
            return !name.StartsWith("Unnamed");
        }
    }
}

namespace sdmap.Utils
{
    internal static class SqlTextUtil
    {
        public static string Parse(string sqlText)
        {
            return sqlText
                .Replace("\\#", "#")
                .Replace("\\}", "}");
        }

        public static string ToCSharpString(string text)
        {
            // " -> ""
            return "@\"" + text.Replace("\"", "\"\"") + "\"";
        }
    }
}

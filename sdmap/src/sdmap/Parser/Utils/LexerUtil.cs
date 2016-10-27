using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sdmap.Parser.Utils
{
    public static class LexerUtil
    {
        public static string GetOpenNSId(string openNamespace)
        {
            // namespace test { -> test
            // namespace A.B.C{ -> A.B.C
            return Regex.Replace(openNamespace, @"namespace\s+([_\.\w]+)\s*\{", "$1");
        }

        public static string GetOpenSqlId(string openSql)
        {
            // sql test {
            // sql test{
            return Regex.Replace(openSql, @"sql\s+([_\w]+)\s*\{", "$1");
        }
    }
}

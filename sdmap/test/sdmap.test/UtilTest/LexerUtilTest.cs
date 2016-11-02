using sdmap.Parser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.UtilTest
{
    public class LexerUtilTest
    {
        [Theory]
        [InlineData("namespace test{", "test")]
        [InlineData("namespace A.B.C {", "A.B.C")]
        [InlineData("namespace __Test.B {", "__Test.B")]
        public void GetOpenNamespaceIdTest(string openNamespace, string expected)
        {
            var actual = LexerUtil.GetOpenNSId(openNamespace);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("sql test{", "test")]
        [InlineData("sql __Hello{", "__Hello")]
        [InlineData("sql Hello3_3{", "Hello3_3")]
        public void GetOpenSqlIdTest(string openSql, string expected)
        {
            var actual = LexerUtil.GetOpenSqlId(openSql);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("#test<", "test")]
        [InlineData("#__test<", "__test")]
        [InlineData("#test2B <", "test2B")]
        public void GetOpenMacroIdTest(string openMacro, string expected)
        {
            var actual = LexerUtil.GetOpenMacroId(openMacro);
            Assert.Equal(expected, actual);
        }
    }
}

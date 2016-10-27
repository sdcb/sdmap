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
        public void GetOpenSqlIdTest(string openSql, string expected)
        {
            var actual = LexerUtil.GetOpenSqlId(openSql);
            Assert.Equal(expected, actual);
        }
    }
}

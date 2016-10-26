using Antlr4.Runtime;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using static sdmap.Parser.G4.SdmapLexerBase;

namespace sdmap.test.LexerTest
{
    public class SimpleTokenTest : LexerTestBase
    {
        [Fact]
        public void String1()
        {
            var tokens = GetAllTokens("\"Test\"");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(STRING, tokens[0].Type);
        }

        [Fact]
        public void String2()
        {
            var tokens = GetAllTokens("'Test'");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(STRING, tokens[0].Type);
        }

        [Fact]
        public void Number()
        {
            var tokens = GetAllTokens("3.14");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(NUMBER, tokens[0].Type);
        }

        [Fact]
        public void Date1()
        {
            var tokens = GetAllTokens("2016-10-26");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(DATE, tokens[0].Type);
        }

        [Fact]
        public void Date2()
        {
            var tokens = GetAllTokens("2016/10/26");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(DATE, tokens[0].Type);
        }

        [Fact]
        public void SqlText()
        {
            var tokens = GetAllTokens("sql { SELECT * FROM client_Profile; }");
            Assert.Equal(3, tokens.Count);
            Assert.Equal(BeginUnnamedSql, tokens[0].Type);
            Assert.Equal(SQLText, tokens[1].Type);
            Assert.Equal(EndSql, tokens[2].Type);
        }

        [Fact]
        public void Syntax()
        {
            var tokens = GetAllTokens("SELECT ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(SYNTAX, tokens[0].Type);
        }

        [Fact]
        public void NamedSql()
        {
            var tokens = GetAllTokens("sql OrderBy{SELECT * FROM client_Profile;}");
            Assert.Equal(new[] 
            {
                BeginNamedSql,
                SQLText, 
                EndSql
            }, tokens.Select(x => x.Type));
        }
    }
}

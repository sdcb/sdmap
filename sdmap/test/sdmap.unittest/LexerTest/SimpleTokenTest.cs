using System.Linq;
using Xunit;

using static sdmap.Parser.G4.SdmapLexer;

namespace sdmap.unittest.LexerTest
{
    public class SimpleTokenTest : LexerTestBase
    {
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
            Assert.Equal(4, tokens.Count);
            Assert.Equal(KSql, tokens[0].Type);
            Assert.Equal(SQLText, tokens[2].Type);
            Assert.Equal(CloseSql, tokens[3].Type);
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
                KSql, SYNTAX, OpenCurlyBrace, 
                    SQLText, 
                CloseSql
            }, tokens.Select(x => x.Type));
        }

        [Fact]
        public void BoolTest()
        {
            var tokens = GetAllTokens("true false");
            Assert.Equal(new[]
            {
                Bool, Bool
            }, tokens.Select(x => x.Type));
        }

        [Fact]
        public void EmptySqlTest()
        {
            var tokens = GetAllTokens("sql v1{#syntax<sql{}>}");
            Assert.Equal(new[]
            {
                KSql, SYNTAX, OpenCurlyBrace,
                    Hash, SYNTAX, OpenAngleBracket, 
                        KSql, 
                        OpenCurlyBrace, 
                        CloseSql, 
                    CloseAngleBracket, 
                CloseSql
            }, tokens.Select(x => x.Type));
        }
    }
}

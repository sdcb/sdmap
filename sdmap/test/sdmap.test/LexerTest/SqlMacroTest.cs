using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using static sdmap.Parser.G4.SdmapLexer;

namespace sdmap.test.LexerTest
{
    public class SqlMacroTest : LexerTestBase
    {
        [Fact]
        public void CanDetectMacro()
        {
            var tokens = GetAllTokens("sql{#include<>}");

            Assert.Equal(new[]
            {
                KSql,OpenCurlyBrace,
                    Hash,
                    SYNTAX, 
                    OpenAngleBracket, 
                    CloseAngleBracket, 
                CloseSql
            }, tokens.Select(x => x.Type));
        }

        [Fact]
        public void MacroParameterOk()
        {
            var tokens = GetAllTokens(
                "sql{#test< Zipcode, 65001, 'Hello World', 2015/1/1, sql { SELECT @@Version;} >}");
            Assert.Equal(new[]
            {
                KSql, OpenCurlyBrace, 
                    Hash, SYNTAX, OpenAngleBracket, 
                        SYNTAX, Comma, 
                        NUMBER, Comma,
                        STRING, Comma,
                        DATE, Comma,
                        SQL, OpenCurlyBrace, 
                            SQLText, 
                        CloseSql, 
                    CloseAngleBracket, 
                CloseSql
            }, tokens.Select(x => x.Type));
        }

        [Fact]
        public void MixSqlAndMacro()
        {
            var tokens = GetAllTokens("sql{SELECT * FROM `Test`; #include<CommonOrderBy>}");

            Assert.Equal(new[]
            {
                KSql, OpenCurlyBrace,
                    SQLText, 
                    Hash, SYNTAX, OpenAngleBracket, 
                        SYNTAX, 
                    CloseAngleBracket, 
                CloseSql
            }, tokens.Select(x => x.Type));

            Assert.Equal("SELECT * FROM `Test`; ", tokens.Single(x => x.Type == SQLText).Text);
            Assert.Equal("include", tokens.First(x => x.Type == SYNTAX).Text);
            Assert.Equal("CommonOrderBy", tokens.Last(x => x.Type == SYNTAX).Text);
        }

        [Fact]
        public void IncludeFullNamespace()
        {
            var tokens = GetAllTokens("sql{#include<Common.OrderBy>}");

            Assert.Equal(new[]
            {
                KSql, OpenCurlyBrace,
                    Hash, SYNTAX, OpenAngleBracket,
                        NSSyntax,
                    CloseAngleBracket,
                CloseSql
            }, tokens.Select(x => x.Type));
            
            Assert.Equal("Common.OrderBy", tokens.Single(x => x.Type == NSSyntax).Text);
        }
    }
}

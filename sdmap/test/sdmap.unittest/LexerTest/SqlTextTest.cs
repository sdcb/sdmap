using Antlr4.Runtime;
using sdmap.Parser.G4;
using sdmap.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

using static sdmap.Parser.G4.SdmapLexer;

namespace sdmap.unittest.LexerTest
{
    public class SqlTextTest
    {
        [Fact]
        public void DoubleHashIsHash()
        {
            var code = "sql v1{\\#}";
            var ats = new AntlrInputStream(code);
            var lexer = new SdmapLexer(ats);
            var tokens = lexer.GetAllTokens();
            Assert.Equal(
                new[] { KSql, SYNTAX, OpenCurlyBrace, SQLText, CloseSql },
                tokens.Select(x => x.Type));
        }

        [Fact]
        public void DoubleCurlyBraceWontThrow()
        {
            var code = "sql v1{}}";
            var ats = new AntlrInputStream(code);
            var lexer = new SdmapLexer(ats);
            var tokens = lexer.GetAllTokens();
        }

        [Fact]
        public void ErrorCurlyBraceWillStillWork()
        {
            var code = "sql v1{}} sql v2{}";
            var ats = new AntlrInputStream(code);
            var lexer = new SdmapLexer(ats);
            var tokens = lexer.GetAllTokens();
        }

        [Fact]
        public void ErrorCurlyInMacro()
        {
            var code = "sql v1{#test<sql{}}>}";
            var ats = new AntlrInputStream(code);
            var lexer = new SdmapLexer(ats);
            var tokens = lexer.GetAllTokens();
        }

        [Fact]
        public void SingleHashIsMacro()
        {
            var code = "sql v1{#}";
            var ats = new AntlrInputStream(code);
            var lexer = new SdmapLexer(ats);
            var tokens = lexer.GetAllTokens();
            Assert.Equal(
                new[] { KSql, SYNTAX, OpenCurlyBrace, Hash, CloseCurlyBrace },
                tokens.Select(x => x.Type));
        }

        [Fact]
        public void DoubleHashWillEmitSingleHash()
        {
            var sqlText = "\\#";
            var result = SqlTextUtil.Parse(sqlText);
            Assert.Equal("#", result);
        }
    }
}

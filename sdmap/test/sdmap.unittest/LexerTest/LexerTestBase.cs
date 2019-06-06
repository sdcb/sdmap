using Antlr4.Runtime;
using sdmap.Parser.G4;
using System.Collections.Generic;

namespace sdmap.unittest.LexerTest
{
    public class LexerTestBase
    {
        protected SdmapLexer BuildLexer(string sourceCode)
        {
            var inputStream = new AntlrInputStream(sourceCode);
            var lexer = new SdmapLexer(inputStream);
            return lexer;
        }

        protected IList<IToken> GetAllTokens(string sourceCode)
        {
            var lexer = BuildLexer(sourceCode);
            return lexer.GetAllTokens();
        }
    }
}

using Xunit;

using static sdmap.Parser.G4.SdmapLexer;

namespace sdmap.unittest.LexerTest
{
    public class StringTest : LexerTestBase
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
        public void EmptyStringIsOk()
        {
            var tokens = GetAllTokens("''");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(STRING, tokens[0].Type);
        }

        [Fact]
        public void AtString()
        {
            var tokens = GetAllTokens(@"@""Hel\/lo""");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(STRING, tokens[0].Type);
            Assert.Equal(@"@""Hel\/lo""", tokens[0].Text);
        }
    }
}

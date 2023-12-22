using System.Linq;
using Xunit;

using static sdmap.Parser.G4.SdmapLexer;

namespace sdmap.unittest.LexerTest;

public class CommentTest : LexerTestBase
{
    [Fact]
    public void LineCommentShouldBeIgnored()
    {
        var tokens = GetAllTokens("3.14//test");
        Assert.Equal(1, tokens.Count);
        Assert.Equal([NUMBER], tokens.Select(x => x.Type));
    }

    [Fact]
    public void BlockCommentShouldBeIgnored()
    {
        var tokens = GetAllTokens("3.14/*test\n\n*/");
        Assert.Equal(1, tokens.Count);
        Assert.Equal([NUMBER], tokens.Select(x => x.Type));
    }
}

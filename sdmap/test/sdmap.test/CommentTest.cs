using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test;

public class CommentTest
{
    [Fact]
    public void LineCommentShouldWorks()
    {
        var code = "// comment should works here\nsql v1{WORKS}";
        var rt = new SdmapCompiler();
        rt.AddSourceCode(code);
        var result = rt.Emit("v1", null);
        Assert.Equal("WORKS", result);
    }

    [Fact]
    public void LineCommentWorksAfterSql()
    {
        var code = "sql v1 // comment should works here\n{WORKS}";
        var rt = new SdmapCompiler();
        rt.AddSourceCode(code);
        var result = rt.Emit("v1", null);
        Assert.Equal("WORKS", result);
    }

    [Fact]
    public void BlockCommentShouldWorks()
    {
        var code = "/* comment should works here\nalso can cross lines\n*/sql v1{WORKS}";
        var rt = new SdmapCompiler();
        rt.AddSourceCode(code);
        var result = rt.Emit("v1", null);
        Assert.Equal("WORKS", result);
    }

    [Fact]
    public void BlockCommentWorksAfterSql()
    {
        var code = "sql v1 /* comment should works here\nalso can cross lines\n*/{WORKS}";
        var rt = new SdmapCompiler();
        rt.AddSourceCode(code);
        var result = rt.Emit("v1", null);
        Assert.Equal("WORKS", result);
    }
}

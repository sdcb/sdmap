using sdmap.Emiter.Implements.Common;
using sdmap.Emiter.Implements.CSharp;
using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace sdmap.test.EmiterTests.CSharpTests
{
    public class BasicTest : BaseCSharpTest
    {
        [Fact]
        public void EmptyWillEmitEmpty()
        {
            var source = "";
            var result = GetEmitText(source);

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void EmptySqlIdTest()
        {
            var source = "sql id{}";
            var result = GetEmitText(source);

            Assert.True(result.IsSuccess);
            var expected = PreUsings + @"
internal class id
    : ISdmapEmiter
{
    internal Result<string> BuildText(object self)
    {
        var sb = new StringBuilder();
        return Result.Ok(sb.ToString());
    }
}
";
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void PlainTextTest()
        {
            var source = "sql id{Hello World}";
            var result = GetEmitText(source);

            Assert.True(result.IsSuccess);
            var expected = PreUsings + @"
internal class id
    : ISdmapEmiter
{
    internal Result<string> BuildText(object self)
    {
        var sb = new StringBuilder();
        sb.Append(@""Hello World"");
        return Result.Ok(sb.ToString());
    }
}
";
            Assert.Equal(expected, result.Value);
        }
    }
}

using sdmap.Emiter.Implements.Common;
using sdmap.Emiter.Implements.CSharp;
using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace sdmap.test.EmiterTests.CSharpTests
{
    public class NamespaceTest : BaseCSharpTest
    {
        [Fact]
        public void EmptyNamespaceTest()
        {
            var source = "namespace id{}";
            var result = GetEmitText(source);

            Assert.True(result.IsSuccess);

            var expected = PreUsings + @"
namespace id
{
}
";
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void NamespaceShouldEmitNamespace()
        {
            var source = "namespace ns{sql id{Nice}}";
            var result = GetEmitText(source);

            Assert.True(result.IsSuccess);
            var expected = PreUsings + @"
namespace ns
{
    internal class id
        : ISdmapEmiter
    {
        internal Result<string> BuildText(object self)
        {
            var sb = new StringBuilder();
            sb.Append(@""Nice"");
            return Result.Ok(sb.ToString());
        }
    }
}
";
            Assert.Equal(expected, result.Value);
        }
    }
}

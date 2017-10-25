using sdmap.Emiter.Implements.Common;
using sdmap.Emiter.Implements.CSharp;
using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace sdmap.unittest.EmiterTests.CSharpTests
{
    public class CSharpEmitTest
    {
        [Fact]
        public void EmptyWillEmitEmpty()
        {
            var source = "";
            var result = GetEmitText(source);

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void EmptyNamespaceTest()
        {
            var source = "namespace id{}";
            var result = GetEmitText(source);

            Assert.True(result.IsSuccess);

            var expected = @"
namespace id
{
}
";
            Assert.Equal(PreUsings + expected, result.Value);
        }

        private readonly string PreUsings = string.Join("", new CSharpDefine().CommonUsings()
                .Select(x => $"using {x};\r\n"));

        private Result<string> GetEmitText(string source, CodeEmiterConfig config = null)
        {
            var emiter = new CSharpCodeEmiter();
            config = config ?? new CodeEmiterConfig
            {
            };
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms))
            {
                return emiter.Emit(source, writer, config)
                    .OnSuccess(() => ms.ToArray())
                    .OnSuccess(Encoding.UTF8.GetString);
            }
        }
    }
}

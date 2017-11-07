using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using sdmap.Emiter.Implements.Common;
using sdmap.Emiter.Implements.CSharp;
using sdmap.Functional;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace sdmap.test.EmiterTests.CSharpTests
{
    public class BaseCSharpTest
    {
        protected Result<string> GetEmitText(string source, CodeEmiterConfig config = null)
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

        protected readonly string PreUsings = string.Join("", new CSharpDefine().CommonUsings()
                .Select(x => $"using {x};\r\n"));
    }
}

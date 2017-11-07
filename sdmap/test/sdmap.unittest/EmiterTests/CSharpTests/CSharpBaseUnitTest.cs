using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using sdmap.Emiter.Implements.Common;
using sdmap.Emiter.Implements.CSharp;
using sdmap.Functional;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sdmap.unittest.EmiterTests.CSharpTests
{
    public class CSharpBaseUnitTest
    {
        protected Result<string> GetEmiterText(string source,
            Func<SdmapParser, IParseTree> partAccessor,
            CodeEmiterConfig config = null)
        {
            var ais = new AntlrInputStream(source);
            var lexer = new SdmapLexer(ais);
            var cts = new CommonTokenStream(lexer);
            var parser = new SdmapParser(cts);

            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms))
            {
                config = config ?? new CodeEmiterConfig();
                var visitor = new CSharpCodeVisitor(writer, config, new CSharpDefine());
                return visitor.Visit(partAccessor(parser))
                    .OnSuccess(() => writer.Flush())
                    .OnSuccess(() => ms.ToArray())
                    .OnSuccess(Encoding.UTF8.GetString);
            }
        }

        protected readonly string MacroProvider 
            = $"{nameof(RuntimeProviders)}.{nameof(RuntimeProviders.RuntimeMacros)}";

        protected readonly string EmiterProvider
            = $"{nameof(RuntimeProviders)}.{nameof(RuntimeProviders.GetEmiter)}";

        protected string TransformRuntimeProvider(string text)
        {
            return text
                .Replace(nameof(MacroProvider), MacroProvider)
                .Replace(nameof(EmiterProvider), EmiterProvider);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using sdmap.Functional;
using Antlr4.Runtime;
using sdmap.Parser.G4;
using sdmap.Emiter.Implements.Common;

namespace sdmap.Emiter.Implements.CSharp
{
    public class CSharpCodeEmiter : CodeEmiterProvider
    {
        public Result Emit(string source, TextWriter writer, 
            CodeEmiterConfig config)
        {
            var ais = new AntlrInputStream(source);
            var lexer = new SdmapLexer(ais);
            var cts = new CommonTokenStream(lexer);
            var parser = new SdmapParser(cts);

            var visitor = new CSharpCodeVisitor(writer, config, new CSharpDefine());
            return visitor.StartVisit(parser.root());
        }
    }
}

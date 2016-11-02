using Antlr4.Runtime;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.test.VisitorTest
{
    public class VisitorTestBase
    {
        protected RootContext GetParseTree(string sourceCode)
        {
            var inputStream = new AntlrInputStream(sourceCode);
            var lexer = new SdmapLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new SdmapParser(tokenStream);
            return parser.root();
        }
    }
}

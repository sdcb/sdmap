using Antlr4.Runtime;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmap.Vstool.Tagger.Antlr
{
    [Export(typeof(ISdmapLexer))]
    class SdmapLexer : ISdmapLexer
    {
        public IEnumerable<SpannedToken> Run(IEnumerable<string> segments, int offset)
        {
            var lexer = new Parser.G4.SdmapLexer(new UnbufferedCharStream(new TextSegmentsCharStream(segments)));
            while (true)
            {
                IToken current = lexer.NextToken();
                if (current.Type == Parser.G4.SdmapLexer.Eof)
                    break;
                yield return new SpannedToken(
                    current.Type,
                    new Span(current.StartIndex + offset, current.StopIndex - current.StartIndex + 1));
            }
        }
    }
}

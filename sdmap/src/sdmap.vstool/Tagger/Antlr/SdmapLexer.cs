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
    [Export(typeof(ISdmapLexerHelper))]
    class SdmapLexerHelper : ISdmapLexerHelper
    {
        public IEnumerable<SpannedToken> GetTokens(IEnumerable<string> segments, int offset)
        {
            var lexer = new Parser.G4.SdmapLexer(new UnbufferedCharStream(new TextSegmentsCharStream(segments)));
            while (true)
            {
                IToken current = null;
                try
                {
                    current = lexer.NextToken();
                    if (current.Type == Parser.G4.SdmapLexer.Eof)
                        break;
                }
                catch (InvalidOperationException e)
                    when (e.HResult == -2146233079) // stack empty
                {
                }

                if (current != null)
                {
                    yield return new SpannedToken(
                        current.Type,
                        new Span(current.StartIndex + offset, current.StopIndex - current.StartIndex + 1));
                }
            }
        }
    }
}

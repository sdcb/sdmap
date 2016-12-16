using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmap.Vstool.Tagger
{
    public struct SpannedToken
    {
        public int TokenType { get; }

        public Span Span { get; }

        public SpannedToken(int tokenType, Span span) : this()
        {
            TokenType = tokenType;
            Span = span;
        }
    }
}

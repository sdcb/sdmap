using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;

namespace sdmap.Vstool.Tagger
{
    public class TokensChangedEventArgs : EventArgs
    {
        public IReadOnlyList<TrackingToken> NewTokens { get; private set; }

        public TokensChangedEventArgs(IReadOnlyList<TrackingToken> newT)
        {
            NewTokens = newT;
        }
    }
}
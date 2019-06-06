using System.Collections.Generic;

namespace sdmap.Parser.G4
{
    public partial class SdmapLexer
    {
        private Stack<string> braceStack = new Stack<string>();
        private string bracePrefix => braceStack.Count == 0 ? null : braceStack.Peek();
    }
}

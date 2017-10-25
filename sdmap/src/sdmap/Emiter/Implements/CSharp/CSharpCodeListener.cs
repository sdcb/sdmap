using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using System.IO;
using sdmap.Emiter.Implements.Common;

namespace sdmap.Emiter.Implements.CSharp
{
    internal class CSharpCodeListener : SdmapParserBaseListener
    {
        private readonly CodeEmiterConfig _config;
        private readonly CSharpDefine _define;
        private readonly IndentWriter _writer;

        internal CSharpCodeListener(
            TextWriter writer, 
            CodeEmiterConfig config, 
            CSharpDefine define)
        {
            _writer = new IndentWriter(writer, 0);
            _config = config;
            _define = define;
        }

        #region Root
        public override void EnterRoot([NotNull] SdmapParser.RootContext context)
        {
            foreach (var usingItem in _define.CommonUsings())
            {
                _writer.WriteLine($"using {usingItem};");
            }
        }

        public override void ExitRoot([NotNull] SdmapParser.RootContext context)
        {
            _writer.Flush();
        }
        #endregion

        #region Namespace
        public override void EnterNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            _writer.Write("namespace ");
        }

        public override void ExitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            _writer.Write("}");
        }
        #endregion

        public override void ExitNsSyntax([NotNull] SdmapParser.NsSyntaxContext context)
        {
            _writer.Write(context.GetText());
        }
    }
}

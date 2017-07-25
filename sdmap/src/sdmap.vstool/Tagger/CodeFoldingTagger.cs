using Antlr4.Runtime;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using sdmap.Parser.G4;
using sdmap.Vstool.Tagger.Antlr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Antlr4.Runtime.Misc;

namespace sdmap.Vstool.Tagger
{
    internal sealed class CodeFoldingTagger : ITagger<IOutliningRegionTag>
    {
        private readonly ITextBuffer _buffer;
        private List<(int start, int end)> regions = new List<(int start, int end)>();
        ITextVersion _currentVersion;

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public CodeFoldingTagger(
            ITextBuffer buffer)
        {
            _buffer = buffer;
            _buffer.ChangedLowPriority += this.Buffer_Changed;
            ReCreateRegions();
        }

        private void Buffer_Changed(object sender, TextContentChangedEventArgs e)
        {
            ReCreateRegions();
        }

        private void ReCreateRegions()
        {
            _currentVersion = _buffer.CurrentSnapshot.Version;
            var lexer = new SdmapLexer(new AntlrInputStream(_buffer.CurrentSnapshot.GetText()));
            var cts = new CommonTokenStream(lexer);
            var parser = new SdmapParser(cts);
            var listener = new SdmapRegionListener();
            parser.AddParseListener(listener);
            regions = listener.Regions;

            try
            {
                parser.root();
            }
            catch (InvalidOperationException e)
                when (e.HResult == -2146233079) // stack empty
            {
            }
            
        }

        public IEnumerable<ITagSpan<IOutliningRegionTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (_currentVersion != _buffer.CurrentSnapshot.Version)
            {
                yield break;
            }

            foreach (var region in regions)
            {
                var text =  _buffer.CurrentSnapshot.GetText(region.start, region.end - region.start);
                yield return new TagSpan<IOutliningRegionTag>(
                    new SnapshotSpan(_buffer.CurrentSnapshot, region.start, region.end - region.start),
                    new OutliningRegionTag(true, true, "...", text));
            }
        }
    }

    class SdmapRegionListener : SdmapParserBaseListener
    {
        public List<(int start, int end)> Regions { get; } = new List<(int start, int end)>();
        
        public override void ExitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            var startToken = context.nsSyntax();
            var endToken = context.GetToken(SdmapParser.CloseCurlyBrace, 0);

            if (startToken != null && endToken != null)
            {
                Regions.Add((
                    startToken.Stop.StopIndex + 1, 
                    context.stop.StopIndex + 1)
                    );
            }
            
            base.ExitNamespace(context);
        }

        public override void ExitNamedSql([NotNull] SdmapParser.NamedSqlContext context)
        {
            var startToken = context.GetToken(SdmapParser.SYNTAX, 0);
            var endToken = context.GetToken(SdmapParser.CloseSql, 0);

            if (startToken != null && endToken != null)
            {
                Regions.Add((
                    startToken.Symbol.StopIndex + 1,
                    context.stop.StopIndex + 1)
                    );
            }

            base.ExitNamedSql(context);
        }
    }
}

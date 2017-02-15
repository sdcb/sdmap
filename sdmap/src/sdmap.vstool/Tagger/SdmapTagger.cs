using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Language.StandardClassification;
using sdmap.Parser.G4;
using Microsoft.VisualStudio.Text.Classification;
using static sdmap.Parser.G4.SdmapLexer;

namespace sdmap.Vstool.Tagger
{
    class SdmapTagger : ITagger<ClassificationTag>
    {
        private ISdmapLexer lexer;
        private ITextBuffer buffer;
        private IStandardClassificationService standardClassificationService;

#pragma warning disable CS0067
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore CS0067

        public SdmapTagger(ISdmapLexer lexer, ITextBuffer buffer, IStandardClassificationService standardClassificationService)
        {
            this.buffer = buffer;
            this.standardClassificationService = standardClassificationService;

            var state = DocumentState.CreateAndRegister(lexer, buffer);
            state.TokensChanged += TokensChanged;
        }

        private void TokensChanged(object src, TokensChangedEventArgs args)
        {
            OutliningRegionTag f;
            if (args.NewTokens.Count == 0)
                return;
            DocumentState state = src as DocumentState;
            if (state == null)
                return;
            var temp = TagsChanged;
            if (temp == null)
                return;
            int start = args.NewTokens[0].GetStart(state.CurrentSnapshot);
            int end = args.NewTokens[args.NewTokens.Count - 1].GetEnd(state.CurrentSnapshot);
            temp(this, new SnapshotSpanEventArgs(new SnapshotSpan(state.CurrentSnapshot, new Span(start, end - start))));
        }

        public IClassificationType GetClassificationTypeByToken(int token)
        {
            var svr = standardClassificationService;
            switch (token)
            {
                case KSql:
                case KNamespace:
                    return svr.Keyword;
                case OpenCurlyBrace:
                case CloseCurlyBrace:
                case CloseSql:
                    return svr.FormalLanguage;
                case STRING:
                    return svr.StringLiteral;
                case NUMBER:
                case DATE:
                    return svr.NumberLiteral;
                case SYNTAX:
                case NSSyntax:
                    return svr.Identifier;
                case WS:
                    return svr.WhiteSpace;
                case BlockComment:
                case LineComment:
                    return svr.Comment;
                case Comma:
                case OpenAngleBracket:
                case CloseAngleBracket:
                    return svr.FormalLanguage;
                case SQLText:
                    return svr.StringLiteral;
                case Hash:
                    return svr.PreprocessorKeyword;
                case Bool:
                    return svr.Keyword;
                default:
                    return svr.Other;
            }
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            DocumentState document;
            if (!DocumentState.TryGet(buffer, out document))
                yield break;
            if (document.CurrentSnapshot.Version.VersionNumber != buffer.CurrentSnapshot.Version.VersionNumber)
                yield break;

            foreach (var span in spans)
            {
                foreach (var token in document.GetTokens(span))
                {
                    if (token.IsEmpty)
                        continue;
                    var tag = new ClassificationTag(GetClassificationTypeByToken(token.Type));
                    yield return new TagSpan<ClassificationTag>(new SnapshotSpan(document.CurrentSnapshot, token.GetSpan(document.CurrentSnapshot)), tag);
                }
            }
        }
    }
}

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
    sealed class CodeHighlightTagger : ITagger<ClassificationTag>
    {
        private readonly ISdmapLexerHelper lexer;
        private readonly ITextBuffer buffer;
        private readonly IStandardClassificationService standardClassificationService;
        private readonly SortedList<int, TagSpan<ClassificationTag>> tokenBuffer = new SortedList<int, TagSpan<ClassificationTag>>();

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public CodeHighlightTagger(ISdmapLexerHelper lexer, ITextBuffer buffer, IStandardClassificationService standardClassificationService)
        {
            this.lexer = lexer;
            this.buffer = buffer;
            this.standardClassificationService = standardClassificationService;

            void WriteBuffer(TextContentChangedEventArgs args)
            {
                tokenBuffer.Clear();
                foreach (var kv in lexer.GetTokens(new[] { buffer.CurrentSnapshot.GetText() }, 0))
                {
                    tokenBuffer[kv.Span.Start] = new TagSpan<ClassificationTag>(
                        new SnapshotSpan(buffer.CurrentSnapshot, kv.Span),
                        new ClassificationTag(GetClassificationTypeByToken(kv.TokenType, standardClassificationService)));
                }

                if (TagsChanged != null && args != null && args.Changes.Count > 0)
                {
                    TagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(args.After, 
                        args.Changes[0].NewSpan)));
                }
            };

            bool commiting = false;
            TextContentChangedEventArgs lastArgs = null;
            buffer.Changed += async (o, e) =>
            {
                lastArgs = e;
                if (!commiting)
                {
                    commiting = true;
                    await Task.Delay(100);
                    WriteBuffer(lastArgs);
                    commiting = false;
                }
            };
            WriteBuffer(null);
        }

        public static IClassificationType GetClassificationTypeByToken(int token, IStandardClassificationService svr)
        {
            switch (token)
            {
                case KSql:
                case KNamespace:
                case KIf:
                case Null:
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
                case OpAnd:
                case OpOr:
                case OpNot:
                    return svr.Operator;
                default:
                    return svr.Other;
            }
        }
        
        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var span in spans)
            {
                var start = span.Start.Position;
                var end = span.End.Position;

                var keysArray = tokenBuffer.Keys.ToArray();
                var startPos = Array.BinarySearch(keysArray, start);
                var endPos = Array.BinarySearch(keysArray, end);

                if (startPos < 0) startPos = Math.Abs(startPos) - 1;
                if (endPos < 0) endPos = Math.Abs(endPos) - 1;
                startPos = startPos - 1;
                if (startPos < 0) startPos = 0;
                if (startPos >= tokenBuffer.Count) startPos = tokenBuffer.Count - 1;
                if (endPos >= tokenBuffer.Count) endPos = tokenBuffer.Count - 1;

                if (startPos == -1) yield break;
                for (var i = startPos; i <= endPos; ++i)
                {
                    yield return tokenBuffer.Values[i];
                }
            }
        }
    }
}

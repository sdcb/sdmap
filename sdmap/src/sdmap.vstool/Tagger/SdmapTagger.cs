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
            this.lexer = lexer;
            this.buffer = buffer;
            this.standardClassificationService = standardClassificationService;
        }

        public static IClassificationType GetClassificationTypeByToken(int token, IStandardClassificationService svr)
        {
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
            return lexer
                .Run(new []{ buffer.CurrentSnapshot.GetText() }, 0)
                .Select(x => new TagSpan<ClassificationTag>(
                    new SnapshotSpan(buffer.CurrentSnapshot, x.Span), 
                    new ClassificationTag(GetClassificationTypeByToken(x.TokenType, standardClassificationService))));
        }
    }
}

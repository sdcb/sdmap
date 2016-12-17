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

namespace sdmap.Vstool.Tagger
{
    class SdmapTagger : ITagger<ClassificationTag>
    {
        private ISdmapLexer lexer;
        private ITextBuffer buffer;
        private IStandardClassificationService standardClassificationService;
        private Dictionary<int, IClassificationType> tagMaps;

#pragma warning disable CS0067
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore CS0067

        public SdmapTagger(ISdmapLexer lexer, ITextBuffer buffer, IStandardClassificationService standardClassificationService)
        {
            this.lexer = lexer;
            this.buffer = buffer;
            this.standardClassificationService = standardClassificationService;
            tagMaps = new Dictionary<int, IClassificationType>()
            {
                { SdmapLexer.BlockComment, standardClassificationService.Comment },
                { SdmapLexer.LineComment, standardClassificationService.Comment },
                { SdmapLexer.WS, standardClassificationService.WhiteSpace },
                { SdmapLexer.OpenNamedSql, standardClassificationService.Keyword },
                { SdmapLexer.Close, standardClassificationService.Keyword },
                { SdmapLexer.OpenUnnamedSql, standardClassificationService.Keyword },
                { SdmapLexer.CloseSql, standardClassificationService.Keyword },
                { SdmapLexer.OpenMacro, standardClassificationService.PreprocessorKeyword },
                { SdmapLexer.SYNTAX, standardClassificationService.Identifier },
                { SdmapLexer.NUMBER, standardClassificationService.NumberLiteral },
                { SdmapLexer.SQLText, standardClassificationService.StringLiteral },
                { SdmapLexer.OpenNamespace, standardClassificationService.Keyword }
            };
        }

        public IClassificationType GetClassificationByTokenType(int tokenType)
        {
            if (tagMaps.ContainsKey(tokenType))
            {
                return tagMaps[tokenType];
            }
            else
            {
                return standardClassificationService.Other;
            }
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return lexer
                .Run(new []{ buffer.CurrentSnapshot.GetText() }, 0)
                .Select(x => new TagSpan<ClassificationTag>(
                    new SnapshotSpan(buffer.CurrentSnapshot, x.Span), 
                    new ClassificationTag(GetClassificationByTokenType(x.TokenType))));
        }
    }
}

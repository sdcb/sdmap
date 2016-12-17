using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Language.StandardClassification;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Editor;

namespace sdmap.Vstool.Tagger
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("sdmap")]
    [TagType(typeof(ClassificationTag))]
    public sealed class SdmapTaggerProvider : ITaggerProvider
    {
        [Import]
        internal IStandardClassificationService StandardClassificationService = null;

        [Import]
        internal ISdmapLexer lexer = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new SdmapTagger(lexer, buffer, StandardClassificationService) as ITagger<T>;
        }
    }
}

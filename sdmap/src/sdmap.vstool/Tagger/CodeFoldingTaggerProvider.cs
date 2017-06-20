using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmap.Vstool.Tagger
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("sdmap")]
    [TagType(typeof(IOutliningRegionTag))]
    internal sealed class CodeFoldingTaggerProvider : ITaggerProvider
    {        
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new CodeFoldingTagger(buffer) as ITagger<T>;
        }
    }
}

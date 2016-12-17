using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmap.Vstool.Definitions
{
#pragma warning disable CS0169
    class SdmapDefinition
    {
        [Export]
        [Name("sdmap")]
        [BaseDefinition("code")]
        static ContentTypeDefinition sdmapContentTypeDefinition;

        [Export]
        [FileExtension(".sdmap")]
        [ContentType("sdmap")]
        static FileExtensionToContentTypeDefinition sdmapFileExtensionDefinition;
    }
#pragma warning restore CS0169
}

using Microsoft.VisualStudio.Language.NavigateTo.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Diagnostics;
using EnvDTE;

namespace sdmap.Vstool.NavigateTo
{
    internal sealed class NavigateToItemDisplay : INavigateToItemDisplay
    {
        public System.Drawing.Icon Glyph => null;

        public string Name => nameof(Name);

        public string AdditionalInformation => nameof(AdditionalInformation);

        public string Description => nameof(Description);

        public ReadOnlyCollection<DescriptionItem> DescriptionItems =>
            new ReadOnlyCollection<DescriptionItem>(new List<DescriptionItem>());

        public void NavigateTo()
        {
            // do nothing
        }
    }
}

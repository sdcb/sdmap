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

namespace sdmap.Vstool.NavigateTo
{
    internal sealed class NavigateToItemDisplayFactory : INavigateToItemDisplayFactory
    {
        public INavigateToItemDisplay CreateItemDisplay(NavigateToItem item)
        {
            return new NavigateToItemDisplay();
        }
    }
}

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
    [Export(typeof(INavigateToItemProviderFactory))]
    internal class NavigateToItemProviderFactory : INavigateToItemProviderFactory
    {
        private static List<ProjectItem> _cachedProjectItems;

        public bool TryCreateNavigateToItemProvider(IServiceProvider serviceProvider, out INavigateToItemProvider provider)
        {
            if (_cachedProjectItems == null)
            {
                _cachedProjectItems = new List<ProjectItem>();
                System.Threading.Tasks.Task.Run(() =>
                {
                    _cachedProjectItems = Util
                       .GetSolutionAllSdmapFiles(serviceProvider)
                       .ToList();
                });
            }
            provider = new NavigateToItemProvider(serviceProvider, () => _cachedProjectItems);
            return true;
        }
    }
}

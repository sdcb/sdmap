using Microsoft.VisualStudio.Language.NavigateTo.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace sdmap.Vstool.NavigateTo
{
    [Export(typeof(INavigateToItemProviderFactory))]
    internal class NavigateToItemProviderFactory : INavigateToItemProviderFactory
    {
        public bool TryCreateNavigateToItemProvider(IServiceProvider serviceProvider, out INavigateToItemProvider provider)
        {
            provider = new NavigateToItemProvider();
            return true;
        }
    }

    internal class NavigateToItemProvider : INavigateToItemProvider
    {
        private static NavigateToItemDisplayFactory _navigateToItemDisplayFactory 
            = new NavigateToItemDisplayFactory();

        public void Dispose()
        {
        }

        public void StartSearch(INavigateToCallback callback, string searchValue)
        {
            callback.AddItem(new NavigateToItem(
                name: "name", 
                kind: "kind", 
                language: "language", 
                secondarySort: "secondarySort", 
                tag: "tag", 
                matchKind: MatchKind.Exact, 
                displayFactory: _navigateToItemDisplayFactory));
            callback.Done();
        }

        public void StopSearch()
        {
        }
    }

    internal sealed class NavigateToItemDisplayFactory : INavigateToItemDisplayFactory
    {
        public INavigateToItemDisplay CreateItemDisplay(NavigateToItem item)
        {
            return new NavigateToItemDisplay();
        }
    }

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

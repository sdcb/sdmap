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
    internal class NavigateToItemProvider : INavigateToItemProvider
    {
        private static NavigateToItemDisplayFactory _navigateToItemDisplayFactory
            = new NavigateToItemDisplayFactory();

        public void Dispose()
        {
        }

        public void StartSearch(INavigateToCallback callback, string searchValue)
        {
            var solution = (IVsSolution)Package.GetGlobalService(typeof(IVsSolution));
            foreach (Project project in Util.GetCSharpProjects(solution))
            {
                var files = project.ProjectItems
                    .OfType<ProjectItem>()
                    .SelectMany(x =>
                    {
                        var filenames = new List<string>(x.FileCount);
                        for (short i = 0; i < x.FileCount; ++i)
                        {
                            filenames.Add(x.FileNames[i]);
                        }
                        return filenames;
                    })
                    .Where(x => x.ToUpperInvariant().EndsWith(".sdmap"));
            }

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
}

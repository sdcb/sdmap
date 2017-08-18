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
using System.IO;
using Antlr4.Runtime;
using sdmap.Parser.G4;

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
            var files = Util
                .GetSolutionAllSdmapFiles()
                .ToList();
            for (var i = 0; i < files.Count; ++i)
            {
                var file = files[i];
                var code = File.ReadAllText(file);

                foreach (var match in SdmapIdListener.FindMatches(code, searchValue))
                {
                    callback.AddItem(match.ToNavigateToItem(_navigateToItemDisplayFactory));
                }

                callback.ReportProgress(i, files.Count);
            }

            callback.Done();
        }

        public void StopSearch()
        {
        }
    }
}

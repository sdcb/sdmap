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
using System.IO;
using Antlr4.Runtime;
using sdmap.Parser.G4;
using System.Threading;

namespace sdmap.Vstool.NavigateTo
{
    internal class NavigateToItemProvider : INavigateToItemProvider
    {
        private static NavigateToItemDisplayFactory _navigateToItemDisplayFactory
            = new NavigateToItemDisplayFactory();

        private CancellationTokenSource _cancellationTokenSource;

        private readonly IServiceProvider _serviceProvider;

        public NavigateToItemProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
        }

        public void StartSearch(INavigateToCallback callback, string searchValue)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            System.Threading.Tasks.Task.Run(() =>
            {
                Search();
            }, _cancellationTokenSource.Token);

            void Search()
            {
                var i = 0;
                foreach (var file in Util.GetSolutionAllSdmapFiles(_serviceProvider))
                {
                    if (_cancellationTokenSource.IsCancellationRequested)
                        break;

                    i += 1;

                    foreach (var match in SdmapIdListener.FindMatches(file, searchValue))
                    {
                        callback.AddItem(match.ToNavigateToItem(_navigateToItemDisplayFactory));
                    }

                    callback.ReportProgress(i, Math.Max(100, i + 1));
                }

                callback.Done();
            }
        }

        public void StopSearch()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}

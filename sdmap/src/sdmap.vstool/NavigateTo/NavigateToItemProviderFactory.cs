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
            var solution = (IVsSolution)Package.GetGlobalService(typeof(IVsSolution));
            foreach (Project project in GetProjects(solution))
            {
                var items = project.ProjectItems
                    .OfType<ProjectItem>()
                    .ToList();
                var fileNames = items.Where(x => x.FileCount > 0)
                    .Select(x => x.FileNames[0])
                    .ToList();
                var names = items.Select(x => x.Name).ToList();
                
                Debug.WriteLine(project.Name);
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

        public static IEnumerable<EnvDTE.Project> GetProjects(IVsSolution solution)
        {
            foreach (IVsHierarchy hier in GetProjectsInSolution(solution))
            {
                EnvDTE.Project project = GetDTEProject(hier);
                if (project != null)
                    yield return project;
            }
        }

        public static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution)
        {
            return GetProjectsInSolution(solution, __VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION);
        }

        public static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution, __VSENUMPROJFLAGS flags)
        {
            if (solution == null)
                yield break;

            IEnumHierarchies enumHierarchies;
            Guid guid = Guid.Empty;
            solution.GetProjectEnum((uint)flags, ref guid, out enumHierarchies);
            if (enumHierarchies == null)
                yield break;

            IVsHierarchy[] hierarchy = new IVsHierarchy[1];
            uint fetched;
            while (enumHierarchies.Next(1, hierarchy, out fetched) == VSConstants.S_OK && fetched == 1)
            {
                if (hierarchy.Length > 0 && hierarchy[0] != null)
                    yield return hierarchy[0];
            }
        }

        public static EnvDTE.Project GetDTEProject(IVsHierarchy hierarchy)
        {
            if (hierarchy == null)
                throw new ArgumentNullException("hierarchy");

            object obj;
            hierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out obj);
            return obj as EnvDTE.Project;
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

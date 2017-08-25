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

namespace sdmap.Vstool.NavigateTo
{
    internal static class Util
    {
        private static string _solutionName;
        private static List<string> _keyProjectNames;

        public static IEnumerable<ProjectItem> GetSolutionAllSdmapFiles(
            IServiceProvider serviceProvider)
        {
            var dte = (DTE)serviceProvider.GetService(typeof(DTE));
            var solution = (IVsSolution)serviceProvider.GetService(typeof(IVsSolution));

            return _solutionName == dte.Solution.FullName ?
                GetFromCacheProjects(solution) :
                GetAndRebuildCache(solution, dte);
        }

        private static IEnumerable<ProjectItem> GetAndRebuildCache(
            IVsSolution solution, 
            DTE dte)
        {
            var keyProjectNames = new List<string>();
            foreach (var project in GetCSharpProjects(solution))
            {
                var items = GetAllProjectItems(project.ProjectItems)
                    .AsParallel()
                    .Where(x => x.FileCount == 1)
                    .Where(x => x.FileNames[0].EndsWith(".sdmap"));

                var hasSdmap = false;
                foreach (var item in items)
                {
                    hasSdmap = true;
                    yield return item;
                }

                if (hasSdmap)
                    keyProjectNames.Add(project.FullName);
            }

            // rebuild cache
            _solutionName = dte.Solution.FullName;
            _keyProjectNames = keyProjectNames;
        }

        private static IEnumerable<ProjectItem> GetAllProjectItems(ProjectItems projectItems)
        {
            foreach (var item in projectItems.OfType<ProjectItem>())
            {
                if (item.ProjectItems.Count > 0)
                {
                    foreach (var subItem in GetAllProjectItems(item.ProjectItems))
                    {
                        yield return subItem;
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<ProjectItem> GetFromCacheProjects(IVsSolution solution)
        {
            return GetCSharpProjects(solution)
                .Where(x => _keyProjectNames.Contains(x.FullName))
                .AsParallel()
                .Select(x => x.ProjectItems)
                .SelectMany(x => GetAllProjectItems(x))
                .Where(x => x.FileCount == 1)
                .Where(x => x.FileNames[0].EndsWith(".sdmap"));
        }

        public static IEnumerable<EnvDTE.Project> GetCSharpProjects(IVsSolution solution)
        {
            foreach (IVsHierarchy hier in GetProjectsInSolution(solution))
            {
                EnvDTE.Project project = GetDTEProject(hier);
                if (project != null && IsCSharpProjectGuid(project.Kind))
                    yield return project;
            }
        }

        private static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution)
        {
            return GetProjectsInSolution(solution, __VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION);
        }

        private static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution, __VSENUMPROJFLAGS flags)
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

        private static EnvDTE.Project GetDTEProject(IVsHierarchy hierarchy)
        {
            if (hierarchy == null)
                throw new ArgumentNullException("hierarchy");

            object obj;
            hierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out obj);
            return obj as EnvDTE.Project;
        }

        private static bool IsCSharpProjectGuid(string guid)
        {
            return
                guid == VsConsts.CSharpProject ||
                guid == VsConsts.CSharpCoreProject;
        }
    }
}

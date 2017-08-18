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
    internal static class Util
    {
        public static IEnumerable<string> GetSolutionAllSdmapFiles()
        {
            var solution = (IVsSolution2)Package.GetGlobalService(typeof(IVsSolution2));
            return GetCSharpProjects(solution)
                .Select(x => x.ProjectItems)
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

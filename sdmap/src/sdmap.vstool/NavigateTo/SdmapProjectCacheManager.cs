using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmap.Vstool.NavigateTo
{
    internal static class SdmapProjectCacheManager
    {
        private static string _solutionName;
        private static List<string> _keyProjectNames;

        public static bool Exists(DTE dte)
        {
            var cacheFile = GetNavigateToCacheFile(dte);
            return File.Exists(cacheFile);
        }

        public static string[] GetSdmapProjects(DTE dte)
        {
            if (!Exists(dte))
                throw new InvalidOperationException("Sdmap project cache is not available.");

            return File.ReadAllLines(GetNavigateToCacheFile(dte));
        }

        public static void Cache(
            DTE dte, 
            List<string> sdmapProjects)
        {
            var folder = GetSdmapFolder(dte);
            Directory.CreateDirectory(folder);

            File.WriteAllLines(GetNavigateToCacheFile(dte), sdmapProjects);
        }

        private static string GetNavigateToCacheFile(DTE dte)
        {
            return Path.Combine(GetSdmapFolder(dte), "navigate-to.cache");
        }

        private static string GetSdmapFolder(DTE dte)
        {
            var solutionFolder = Path.GetDirectoryName(dte.Solution.FullName);
            var vsFolder = Path.Combine(solutionFolder, ".vs");            
            var sdmapFolder = Path.Combine(vsFolder, "sdmap");
            return sdmapFolder;
        }
    }
}

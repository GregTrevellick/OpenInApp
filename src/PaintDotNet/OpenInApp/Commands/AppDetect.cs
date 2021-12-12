using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Environment;

namespace OpenInApp
{
    internal static class AppDetect
    {
        internal static string PathToExeOnDisc()
        {
            try
            {
                var value = GetActualPathToExe();

                if (File.Exists(value))
                {
                    return value;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string GetActualPathToExe()
        {
            var searchPaths = GetSearchPathsForThirdPartyExe();

            foreach (var searchPath in searchPaths)
            {
                if (File.Exists(searchPath))
                {
                    return searchPath;
                }
            }

            return null;
        }

        internal static IEnumerable<string> GetSearchPathsForThirdPartyExe()
        {
            var searchPaths = new List<string>();

            var paths = GetSpecialFoldersPlusThirdPartyExePath(MyConstants.ExeNameIncFolderWithinProgramFiles).ToList();
            searchPaths.AddRange(paths);

            searchPaths = DoubleUpForDDrive(searchPaths).ToList();

            return searchPaths;
        }

        private static IList<string> GetSpecialFoldersPlusThirdPartyExePath(string executableFileToBrowseFor)//, string secondaryFilePathSegment)
        {
            var paths = new List<string>();

            if (!string.IsNullOrEmpty(executableFileToBrowseFor))
            {
                var initialFolderPaths = new List<string>();
                var specialFolder = (SpecialFolder)42;// 42 = ProgramFilesX86
                var initialFolderPath = GetFolderPath(specialFolder);
                initialFolderPaths.Add(initialFolderPath);

                //add in the non-x86 too
                var x86 = " (x86)";
                var initialFolderPathshWithoutx86 = initialFolderPath.Replace(x86, string.Empty);
                initialFolderPaths.Add(initialFolderPathshWithoutx86);

                foreach (var folderPath in initialFolderPaths)
                {
                    var path = Path.Combine(folderPath, executableFileToBrowseFor);
                    paths.Add(path);
                }
            }

            return paths;
        }

        private static IEnumerable<string> DoubleUpForDDrive(IEnumerable<string> searchPaths)
        {
            var dPaths = new List<string>();

            foreach (var path in searchPaths)
            {
                var dPath = path.Replace("C:", "D:");
                dPaths.Add(dPath);
            }

            return searchPaths.Union(dPaths);
        }
    }
}

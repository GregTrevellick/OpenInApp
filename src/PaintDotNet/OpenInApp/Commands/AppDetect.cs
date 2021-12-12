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
            //var actualPathToExeHelper = new ApplicationToOpenHelper();

            //var executableFilesToBrowseFor = new List<string> {MyConstants.ExeNameIncFolderWithinProgramFiles };//actualPathToExeHelper.GetExecutableFilesToBrowseFor();

            //foreach (var executableFileToBrowseFor in executableFilesToBrowseFor)
            //{
                //var paths = GetSpecialFoldersPlusThirdPartyExePath(executableFileToBrowseFor).ToList();
                var paths = GetSpecialFoldersPlusThirdPartyExePath(MyConstants.ExeNameIncFolderWithinProgramFiles).ToList();
                searchPaths.AddRange(paths);
            //}

            searchPaths = DoubleUpForDDrive(searchPaths).ToList();

            return searchPaths;
        }

        private static IList<string> GetSpecialFoldersPlusThirdPartyExePath(string executableFileToBrowseFor)//, string secondaryFilePathSegment)
        {
            var paths = new List<string>();

            if (!string.IsNullOrEmpty(executableFileToBrowseFor))
            {
                //set up array of the four special folders
                var initialFolders = new List<InitialFolderType>
                {
                    InitialFolderType.ProgramFilesX86,
                    //InitialFolderType.LocalApplicationData,
                    //InitialFolderType.Windows
                };

                var initialFolderPaths = new List<string>();
                
                foreach (var initialFolder in initialFolders)
                {
                    var specialFolder = (SpecialFolder)initialFolder;
                    var initialFolderPath = GetFolderPath(specialFolder);
                    initialFolderPaths.Add(initialFolderPath);
                
                    //if x86 add in the non-x86 too
                    if (initialFolder == InitialFolderType.ProgramFilesX86)
                    {
                        var x86 = " (x86)";
                        var initialFolderPathshWithoutx86 = initialFolderPath.Replace(x86, string.Empty);
                        initialFolderPaths.Add(initialFolderPathshWithoutx86);
                    }
                }

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

            var result = searchPaths.Union(dPaths);
            return result;
        }
    }
}

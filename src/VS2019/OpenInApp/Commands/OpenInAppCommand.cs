using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using OpenInVS2019;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenInApp
{
    internal sealed class OpenInAppCommand
    {
        private readonly Package _package;
        private readonly Options _options;

        private OpenInAppCommand(Package package, Options options)
        {
            _package = package;
            _options = options;

            var commandService = (OleMenuCommandService)ServiceProvider.GetService(typeof(IMenuCommandService));

            if (commandService != null)
            {
                var menuCommandID = new CommandID(PackageGuids.guidOpenInAppCmdSet, PackageIds.CmdIdOpenInAppItemNode);
                var menuItem = new MenuCommand(OpenApp, menuCommandID);
                commandService.AddCommand(menuItem);

                menuCommandID = new CommandID(PackageGuids.guidOpenInAppCmdSet, PackageIds.CmdIdOpenInAppCodeWin);
                menuItem = new MenuCommand(OpenApp, menuCommandID);
                commandService.AddCommand(menuItem);

                //gregt to be tested
                //menuCommandID = new CommandID(PackageGuids.guidOpenInAppCmdSet, PackageIds.CmdIdOpenInAppFolderNode);
                //menuItem = new MenuCommand(OpenApp, menuCommandID);
                //commandService.AddCommand(menuItem);

                //gregt this works
                menuCommandID = new CommandID(PackageGuids.guidOpenInAppCmdSet, PackageIds.CmdIdOpenInAppProjNode);
                menuItem = new MenuCommand(OpenApp, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static OpenInAppCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        public static void Initialize(Package package, Options options)
        {
            Instance = new OpenInAppCommand(package, options);
        }

        private void OpenApp(object sender, EventArgs e)
        {
            try
            {
                var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
                Assumes.Present(dte);

                var selectedFilesToOpenPaths = ProjectHelpers.GetSelectedFilesToOpenPaths(dte, true);

                if (selectedFilesToOpenPaths != null && selectedFilesToOpenPaths.Any())
                {
                    OpenApplication(selectedFilesToOpenPaths);
                }
                else
                {
                    MessageBox.Show("Couldn't resolve the folder");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private void OpenApplication(IList<string> actualArtefactsToBeOpened)
        {
            EnsurePathToExeExist();

            var arguments = " ";

            foreach (var actualArtefactToBeOpened in actualArtefactsToBeOpened)
            {
                arguments += actualArtefactToBeOpened;
                arguments += " ";
            }

            var start = new ProcessStartInfo()
            {
                Arguments = arguments,
                CreateNoWindow = true,
                FileName = $"\"{_options.PathToExe}\"",
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            using (System.Diagnostics.Process.Start(start))
            {
            }
        }

        private void EnsurePathToExeExist()
        {
            if (File.Exists(_options.PathToExe))
            {
                return;
            }

            var pathToExeOnDisc = AppDetect.PathToExeOnDisc();

            if (!string.IsNullOrEmpty(pathToExeOnDisc))
            {
                SaveOptions(_options, pathToExeOnDisc);
            }
            else
            {
                var box = MessageBox.Show(
                    $"Cannot locate {MyConstants.ExeName} executable. Locate it manually?",
                    Vsix.Name,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (box == DialogResult.No)
                {
                    return;
                }

                var dialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    DefaultExt = ".exe",
                    FileName = MyConstants.ExeName,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                };

                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    SaveOptions(_options, dialog.FileName);
                }
            }
        }

        private void SaveOptions(Options options, string path)
        {
            options.PathToExe = path;
            options.SaveSettingsToStorage();
        }
    }
}


//private static IList<string> GetArtefactsToBeOpened(DTE2 dte)
//{
//    var result = new List<string>();
//    foreach (SelectedItem selectedItem in dte.SelectedItems)
//    {
//        var itemName = selectedItem.ProjectItem.FileNames[0];
//        result.Add(itemName);
//    }
//    return result;
//}

//private static string GetSingleArgument(string argument)
//{
//    var result = "\"" + argument + "\"";
//    return result + " ";
//}
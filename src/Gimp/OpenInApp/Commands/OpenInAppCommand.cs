using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
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
                var menuCommandID = new CommandID(PackageGuids.guidOpenInVsCmdSet, PackageIds.CmdIdOpenInAppItemNode);
                var menuItem = new MenuCommand(OpenApp, menuCommandID);
                commandService.AddCommand(menuItem);

                menuCommandID = new CommandID(PackageGuids.guidOpenInVsCmdSet, PackageIds.CmdIdOpenInAppCodeWin);
                menuItem = new MenuCommand(OpenApp, menuCommandID);
                commandService.AddCommand(menuItem);

                //gregt next 2 to be tested
                menuCommandID = new CommandID(PackageGuids.guidOpenInVsCmdSet, PackageIds.CmdIdOpenInAppFolderNode);
                menuItem = new MenuCommand(OpenApp, menuCommandID);
                commandService.AddCommand(menuItem);

                menuCommandID = new CommandID(PackageGuids.guidOpenInVsCmdSet, PackageIds.CmdIdOpenInAppProjNode);
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

                var path = ProjectHelpers.GetSelectedPath(dte);//////////////////, _options.OpenSolutionProjectAsRegularFile);

                if (!string.IsNullOrEmpty(path))
                {                   
                    OpenApp(dte);
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

        private void OpenApp(DTE2 dte)
        {
            EnsurePathExist();

            var actualArtefactsToBeOpened = GetArtefactsToBeOpened(dte);

            var arguments = " ";

            foreach (var actualArtefactToBeOpened in actualArtefactsToBeOpened)
            {
                arguments += GetSingleArgument(actualArtefactToBeOpened);
            }

            arguments = arguments.TrimEnd(' ');

            var start = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = $"\"{_options.PathToExe}\"",
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            };

            using (System.Diagnostics.Process.Start(start))
            {
            }
        }

        private static IList<string> GetArtefactsToBeOpened(DTE2 dte)
        {
            var result = new List<string>();

            foreach (SelectedItem selectedItem in dte.SelectedItems)
            {
                var itemName = selectedItem.ProjectItem.FileNames[0];
                result.Add(itemName);
            }

            return result;
        }

        private static string GetSingleArgument(string argument)
        {
            var result = "\"" + argument + "\"";
            return result + " ";
        }

        private void EnsurePathExist()
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

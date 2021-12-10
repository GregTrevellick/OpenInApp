using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Forms;
using Microsoft;

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
                var menuItem = new MenuCommand(OpenFolderInVs, menuCommandID);
                commandService.AddCommand(menuItem);

                //menuCommandID = new CommandID(PackageGuids.guidOpenInVsCmdSet, PackageIds.CmdIdOpenInAppCodeWin);
                //menuItem = new MenuCommand(OpenFolderInVs, menuCommandID);
                //commandService.AddCommand(menuItem);

                //menuCommandID = new CommandID(PackageGuids.guidOpenInVsCmdSet, PackageIds.CmdIdOpenInAppFolderNode);
                //menuItem = new MenuCommand(OpenFolderInVs, menuCommandID);
                //commandService.AddCommand(menuItem);

                //menuCommandID = new CommandID(PackageGuids.guidOpenInVsCmdSet, PackageIds.CmdIdOpenInAppProjNode);
                //menuItem = new MenuCommand(OpenFolderInVs, menuCommandID);
                //commandService.AddCommand(menuItem);
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

        private void OpenFolderInVs(object sender, EventArgs e)
        {
            try
            {
                var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
                Assumes.Present(dte);

                string path = ProjectHelpers.GetSelectedPath(dte, _options.OpenSolutionProjectAsRegularFile);

                if (!string.IsNullOrEmpty(path))
                {
                    int line = 0;

                    if (dte.ActiveDocument?.Selection is TextSelection selection)
                    {
                        line = selection.ActivePoint.Line;
                    }

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

            var actualArtefactsToBeOpened = ArtefactsToOpenHelper.GetArtefactsToBeOpened(dte);

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

        private static string GetSingleArgument(string argument)
        {
            var result = "\"" + argument + "\"";
            return result + " ";
        }

        private void EnsurePathExist()
        {
            if (File.Exists(_options.PathToExe))
                return;

            if (!string.IsNullOrEmpty(AppDetect.OnDisc()))
            {
                SaveOptions(_options, AppDetect.OnDisc());
            }
            else
            {
                var box = MessageBox.Show(
                    "I can't find Paint.Net executable. Would you like to help me find it?", 
                    Vsix.Name,
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (box == DialogResult.No)
                    return;

                var dialog = new OpenFileDialog
                {
                    DefaultExt = ".exe",
                    FileName = "Code.exe",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    CheckFileExists = true
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

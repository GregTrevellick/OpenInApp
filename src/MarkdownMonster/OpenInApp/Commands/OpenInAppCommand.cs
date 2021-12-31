using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using OpenInMarkdownMonster;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenInApp
{
    internal sealed class OpenInAppCommand
    {
        private readonly Package _package;
        private readonly Options _options;
        private readonly OpenCmd _openCmd;

        private OpenInAppCommand(Package package, Options options)
        {
            _package = package;
            _openCmd = new OpenCmd();
            _options = options;

            var commandService = (OleMenuCommandService)ServiceProvider.GetService(typeof(IMenuCommandService));

            if (commandService != null)
            {
                var menuCommandID = new CommandID(PackageGuids.guidOpenInAppCmdSet, PackageIds.OpenInApp);
                var menuItem = new MenuCommand(OpenApp, menuCommandID);
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
            _openCmd.OpenApplicationExe(actualArtefactsToBeOpened, _options.PathToExe);
        }

        private void EnsurePathToExeExist()
        {
            if (!File.Exists(_options.PathToExe))
            {
                var pathToExeOnDisc = AppDetect.PathToExeOnDisc(MyConstants.ExeNameIncFolderWithinProgramFiles, MyConstants.ExeName);

                if (!string.IsNullOrEmpty(pathToExeOnDisc))
                {
                    SaveOptions(_options, pathToExeOnDisc);
                }
                else
                {
                    var dialogFileName = _openCmd.LocateItManually(MyConstants.ExeName, Vsix.Name);

                    if (dialogFileName != null)
                    {
                        SaveOptions(_options, dialogFileName);
                    }
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
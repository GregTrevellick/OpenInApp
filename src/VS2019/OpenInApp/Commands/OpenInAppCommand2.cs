//using EnvDTE;
//using EnvDTE80;
//using Microsoft;
//using Microsoft.VisualStudio.Shell;
//using OpenInVS2019;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenInApp
{
    internal sealed partial class OpenInAppCommand
    {
        private void OpenApplicationExe(IList<string> actualArtefactsToBeOpened)
        {
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

            using (Process.Start(start))
            {
            }
        }

        private void LocateItManually()
        {
            var box = MessageBox.Show(
                $"Cannot locate {MyConstants.ExeName} executable. Locate it manually?",
                Vsix.Name,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (box != DialogResult.No)
            {
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
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenInApp
{
    public class OpenCmd
    {
        public void OpenApplicationExe(IList<string> actualArtefactsToBeOpened, string optionsPathToExe)
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
                FileName = $"\"{optionsPathToExe}\"",
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            using (Process.Start(start))
            {
            }
        }

        public string LocateItManually()
        {
            string dialogFileName = null;

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

                var dialogResult = dialog.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    dialogFileName = dialog.FileName;
                }
            }

            return dialogFileName;
        }
    }
}
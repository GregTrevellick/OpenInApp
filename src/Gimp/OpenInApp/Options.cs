using Microsoft.VisualStudio.Shell;
////using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace OpenInApp
{
    public class Options : DialogPage
    {
        //[Category("General")]
        //[DisplayName("Command line arguments")]
        //[Description("Command line arguments to pass to executable")]
        //public string CommandLineArguments { get; set; }

        [Category("General")]
        [DisplayName("Path to executable")]
        [Description("Specify the path to executable.")]
        public string PathToExe { get; set; } //= Environment.ExpandEnvironmentVariables(@"%localappdata%\Programs\Microsoft VS Code\Code.exe");

        //[Category("General")]
        //[DisplayName("Open solution/project as regular file")]
        //[Description("When true, opens solutions/projects as regular files and does not load folder path into VS Code.")]
        //public bool OpenSolutionProjectAsRegularFile { get; set; }

        protected override void OnApply(PageApplyEventArgs e)
        {
            if (!File.Exists(PathToExe))
            {
                e.ApplyBehavior = ApplyKind.Cancel;
                MessageBox.Show($"The file \"{PathToExe}\" doesn't exist.", Vsix.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            base.OnApply(e);
        }
    }
}

using Microsoft.VisualStudio.Shell;
using OpenInApp;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace OpenInAndroidStudio
{
    public class Options : DialogPage
    {
        [Category("General")]
        [DisplayName("Path to executable")]
        [Description("Specify the path to executable.")]
        public string PathToExe { get; set; } 

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
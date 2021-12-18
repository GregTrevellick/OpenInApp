using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenInApp
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(Options), Vsix.Name, Vsix.Name, 0, 0, true)]
    [Guid(PackageGuids.guidOpenInGimpPackageString)]
    public sealed class VSPackage : AsyncPackage
    {
        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();

            var options = (Options)GetDialogPage(typeof(Options));

            Logger.Initialize(this, Vsix.Name);
            OpenInAppCommand.Initialize(this, options);
        }
    }
}

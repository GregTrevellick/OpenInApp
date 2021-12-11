using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

public static class Logger
{
    private static string _name;
    private static IVsOutputWindowPane pane;
    private static IServiceProvider _provider;

    public static void Initialize(Package provider, string name)
    {
        _provider = provider;
        _name = name;
    }

    public static void Log(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        try
        {
            if (EnsurePane())
            {
                pane.OutputString(DateTime.Now.ToString() + ": " + message + Environment.NewLine);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }

    public static void Log(Exception ex)
    {
        if (ex != null)
        {
            Log(ex.ToString());
        }
    }

    private static bool EnsurePane()
    {
        if (pane == null)
        {
            Guid guid = Guid.NewGuid();
            IVsOutputWindow output = (IVsOutputWindow)_provider.GetService(typeof(SVsOutputWindow));
            Assumes.Present(output);

            output.CreatePane(ref guid, _name, 1, 1);
            output.GetPane(ref guid, out pane);
        }

        return pane != null;
    }
}
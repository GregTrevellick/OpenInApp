// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var start = new ProcessStartInfo()
{
    Arguments = "\"C:\\Users\\gtrev\\Source\\Repos\\dummy1\\dummy1\\NewFolder\\Class1 - Copy (2).cs\"",//THIS WORKS
    //Arguments = "C:\\Users\\gtrev\\Source\\Repos\\dummy1\\dummy1\\NewFolder\\Class1 - Copy (2).cs",//THIS NOT WORKS
    CreateNoWindow = true,
    //FileName = "\"D:\\Program Files (x86)\\Notepad++\\notepad++.exe\"",
    FileName = "\"C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\Common7\\IDE\\devenv.exe\"",
    //FileName = "\"D:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Common7\\IDE\\devenv.exe\"",
    UseShellExecute = false,//UseShellExecute = true,
    WindowStyle = ProcessWindowStyle.Hidden
};

using (System.Diagnostics.Process.Start(start))
{
}

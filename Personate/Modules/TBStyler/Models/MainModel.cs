using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TBStyler.Settings;

namespace TBStyler.WPF;

class MainModel
{
    private SettingsService settingsService;
    private Process consoleApp;

    public MainModel(SettingsService settingsService)
    {
        this.settingsService = settingsService;
        StartConsoleApp();
    }

    public void StartConsoleApp()
    {
        consoleApp = Process.GetProcessesByName("TBStyler").FirstOrDefault();

        if (consoleApp == null)
        {
            consoleApp = new Process();
            consoleApp.StartInfo = GetDefaultStartInfo();
            consoleApp.Start();
            consoleApp.PriorityClass = ProcessPriorityClass.Idle;
        }

        WriteLineToConsole("-start");
    }

    private ProcessStartInfo GetDefaultStartInfo()
    {
        var DefaultStartInfo = new ProcessStartInfo()
        {
            FileName = GetPathToCore(),
            //Arguments = settingsService.PathToLast,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = !Debugger.IsAttached,
        };
        return DefaultStartInfo;
    }

    private string GetPathToCore()
    {
        string currentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        string solutionPath = Directory.GetParent(currentDirectory).Parent.Parent.Parent.FullName;
        string secondProjectPath = Path.Combine(solutionPath, "TBStyler", "bin", "Debug", "net8.0-windows", "TBStyler.exe");
        if (File.Exists(secondProjectPath)) return secondProjectPath;
        else return @"C:\Users\Egxr41k\Desktop\TaskbarStyler\TBStyler\bin\Debug\net8.0-windows\TBStyler.exe";
    }

    public void Reset()
    {

    }

    public void Reload()
    {

    }

    public void Stop()
    {
        WriteLineToConsole("-stop");
        consoleApp.Kill();
    }

    public void Restart()
    {
        WriteLineToConsole("-stop");
        consoleApp.Kill();

        StartConsoleApp();
    }

    public void Apply()
    {
        WriteLineToConsole("-update");
    }

    public void WriteLineToConsole(string data)
    {
        if (consoleApp != null && !consoleApp.HasExited)
        {
            consoleApp.StandardInput.WriteLine(data);
        }
    }
}

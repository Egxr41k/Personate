using System.Diagnostics;
using System.Security.Cryptography.Pkcs;
using TBStyler.Win32.Types;
using Personate.Settings;

namespace TBStyler;

public class Setuper
{
    // public NotifyIcon noty = new NotifyIcon() { };
    private SettingsService settingsService;
    private Centerer taskbarCenter;
    private Styler taskbarStyle;
    public Setuper(SettingsService settingsService)
    {
        this.settingsService = settingsService;

        taskbarCenter = new Centerer(settingsService.Settings.Taskbar);
        taskbarStyle = new Styler(settingsService.Settings.Taskbar);

        //PerformanceOptimizer.KillProcessByName("TBStyler");

        Win32.Api.CalculateRightPosition();

        WaitForShell_TrayWnd();

        //PerformanceOptimizer.ClearMemory();

        taskbarStyle.ResetStyles();

        //if (Settings.ShowTrayIcon == 1) TrayIconSetup();
    }

    public void Start()
    {
        taskbarCenter.StartIfEnable();
        taskbarStyle.StartIfEnable();
    }

    public void Stop()
    {
        taskbarCenter.RestorePosition();
        taskbarStyle.ResetStyles();
    }

    private void WaitForShell_TrayWnd()
    {
        IntPtr handle;
        do
        {
            Console.WriteLine("Waiting for Shell_TrayWnd");
            handle = default;
            Task.Delay(250);
            IntPtr Shell_TrayWnd = Win32.Intertop.FindWindowByClass("Shell_TrayWnd", 0);
            IntPtr TrayNotifyWnd = Win32.Intertop.FindWindowEx(Shell_TrayWnd, 0, "TrayNotifyWnd", null);
            IntPtr ReBarWindow32 = Win32.Intertop.FindWindowEx(Shell_TrayWnd, 0, "ReBarWindow32", null);
            IntPtr MSTaskSwWClass = Win32.Intertop.FindWindowEx(ReBarWindow32, 0, "MSTaskSwWClass", null);
            IntPtr MSTaskListWClass = Win32.Intertop.FindWindowEx(MSTaskSwWClass, 0, "MSTaskListWClass", null);
            handle = MSTaskListWClass;
        }
        while (handle == 0);
    }
}

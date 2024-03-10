using System.Diagnostics;
using System.Security.Cryptography.Pkcs;
using TBStyler.Win32.Types;
using TBStyler.Settings;

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

        taskbarCenter = new Centerer(settingsService.Settings);
        taskbarStyle = new Styler(settingsService.Settings);

        PerformanceOptimizer.KillProcessByName("TBStyler");

        Win32.Api.CalculateRightPosition();

        WaitForShell_TrayWnd();

        PerformanceOptimizer.ClearMemory();

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

    }

    private void WaitForShell_TrayWnd()
    {
        IntPtr handle;
        do
        {
            Console.WriteLine("Waiting for Shell_TrayWnd");
            handle = default;
            Task.Delay(250);
            nint Shell_TrayWnd = Win32.Intertop.FindWindowByClass("Shell_TrayWnd", 0);
            nint TrayNotifyWnd = Win32.Intertop.FindWindowEx(Shell_TrayWnd, 0, "TrayNotifyWnd", null);
            nint ReBarWindow32 = Win32.Intertop.FindWindowEx(Shell_TrayWnd, 0, "ReBarWindow32", null);
            nint MSTaskSwWClass = Win32.Intertop.FindWindowEx(ReBarWindow32, 0, "MSTaskSwWClass", null);
            nint MSTaskListWClass = Win32.Intertop.FindWindowEx(MSTaskSwWClass, 0, "MSTaskListWClass", null);
            handle = MSTaskListWClass;
        }
        while (handle == 0);
    }
}

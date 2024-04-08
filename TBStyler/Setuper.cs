using Personate.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TBStyler;

internal class Setuper
{
    public static NotifyIcon noty = new NotifyIcon();
    public Setuper(TaskbarSettingsDTO Settings)
    {
        try
        {
            bool stopgiven = false;

            // Kill every other running instance of TaskbarX
            try
            {
                foreach (Process prog in Process.GetProcessesByName("TaskbarX"))
                {
                    if (prog.Id != Process.GetCurrentProcess().Id)
                    {
                        prog.Kill();
                    }
                }
            }
            catch { }

            // If animation speed is lower than 1 then make it 1. Otherwise, it will give an error.
            if (Settings.AnimationSpeed <= 1)
            {
                Settings.AnimationSpeed = 1;
            }

            // Makes the animations run smoother
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.PriorityClass = ProcessPriorityClass.Idle;

            // Prevent wrong position calculations
            Win32.Intertop.SetProcessDpiAwareness(Win32.Types.ProcessDpiAwareness.Process_Per_Monitor_DPI_Aware);

            // Wait for Shell_TrayWnd
            IntPtr Handle;
            do
            {
                Console.WriteLine("Waiting for Shell_TrayWnd");
                Handle = IntPtr.Zero;
                Thread.Sleep(250);
                IntPtr Shell_TrayWnd = Win32.Intertop.FindWindowByClass("Shell_TrayWnd", IntPtr.Zero);
                IntPtr TrayNotifyWnd = Win32.Intertop.FindWindowEx(Shell_TrayWnd, IntPtr.Zero, "TrayNotifyWnd", null);
                IntPtr ReBarWindow32 = Win32.Intertop.FindWindowEx(Shell_TrayWnd, IntPtr.Zero, "ReBarWindow32", null);
                IntPtr MSTaskSwWClass = Win32.Intertop.FindWindowEx(ReBarWindow32, IntPtr.Zero, "MSTaskSwWClass", null);
                IntPtr MSTaskListWClass = Win32.Intertop.FindWindowEx(MSTaskSwWClass, IntPtr.Zero, "MSTaskListWClass", null);
                Handle = MSTaskListWClass;
            } while (Handle == IntPtr.Zero);

            IntPtr Win11Taskbar = Win32.Intertop.FindWindowEx(Win32.Intertop.FindWindowByClass("Shell_TrayWnd", IntPtr.Zero), IntPtr.Zero, "Windows.UI.Composition.DesktopWindowContentBridge", "");
            if (Win11Taskbar != IntPtr.Zero)
            {
                // Windows 11 Taskbar present
                Settings.DontCenterTaskbar = 1;
            }

            if (stopgiven)
            {
                noty.Visible = false;
                TaskbarCenter.RevertToZero();
                ResetTaskbarStyle();
                return;
            }

            if (Settings.ShowTrayIcon == 1)
            {
                TrayIconBuster.TrayIconBuster.RemovePhantomIcons();
            }

            // Just empty startup memory before starting
            ClearMemory();

            // Reset the taskbar style...
            ResetTaskbarStyle();

            if (Settings.ShowTrayIcon == 1)
            {
                noty.Text = "TaskbarX (L = Restart) (M = Config) (R = Stop)";
                noty.Icon = new(@"C:\Users\Egxr41k\Desktop\Personate\TBStyler\Personate.ico");
                noty.Visible = true;
            }

            noty.MouseClick += new MouseEventHandler(MnuRef_Click);

            // Start the TaskbarCenterer
            if (Settings.DontCenterTaskbar != 1)
            {
                TaskbarCenter taskbarCenter = new TaskbarCenter(Settings);
                Thread t1 = new Thread(taskbarCenter.TaskbarCenterer);
                t1.Start();
            }

            // Start the TaskbarStyler if enabled
            if (Settings.TaskbarStyle == 1 || Settings.TaskbarStyle == 2 || Settings.TaskbarStyle == 3 || Settings.TaskbarStyle == 4 || Settings.TaskbarStyle == 5)
            {
                TaskbarStyle taskbarStyle = new TaskbarStyle(Settings);
                Thread t2 = new Thread(taskbarStyle.TaskbarStyler);
                t2.Start();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void Toaster(string message)
    {
        noty.BalloonTipTitle = "TaskbarX";
        noty.BalloonTipText = message;
        noty.Visible = true;
        noty.ShowBalloonTip(3000);
    }

    public static void MnuRef_Click(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            noty.Visible = false;
            Application.Restart();
        }
        else if (e.Button == MouseButtons.Right)
        {
            noty.Visible = false;
            TaskbarCenter.RevertToZero();
            ResetTaskbarStyle();
            Environment.Exit(0);
        }
        else if (e.Button == MouseButtons.Middle)
        {
            if (System.AppDomain.CurrentDomain.BaseDirectory.Contains("40210ChrisAndriessen"))
            {
                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                        Arguments = " /c start shell:AppsFolder\\40210ChrisAndriessen.FalconX_y1dazs5f5wq00!TaskbarXGUI"
                    };
                    Process.Start(processInfo);
                }
                catch { }
            }
            else
            {
                try
                {
                    Process.Start("TaskbarX Configurator.exe");
                }
                catch { }
            }
        }
    }


    #region Commands

    [DllImport("user32")]
    public static extern int EnumWindows(CallBack Adress, int y);

    public delegate bool CallBack(IntPtr hwnd, int lParam);

    public static System.Collections.ObjectModel.Collection<IntPtr> ActiveWindows = new System.Collections.ObjectModel.Collection<IntPtr>();

    public static System.Collections.ObjectModel.Collection<IntPtr> GetActiveWindows()
    {
        windowHandles.Clear();
        EnumWindows(Enumerator, 0);

        bool maintaskbarfound = false;
        bool sectaskbarfound = false;

        foreach (IntPtr Taskbar in windowHandles)
        {
            StringBuilder sClassName = new StringBuilder("", 256);
            Win32.Intertop.GetClassName(Taskbar, sClassName, 256);
            if (sClassName.ToString() == "Shell_TrayWnd")
            {
                maintaskbarfound = true;
            }
            if (sClassName.ToString() == "Shell_SecondaryTrayWnd")
            {
                sectaskbarfound = true;
            }
            Console.WriteLine("=" + maintaskbarfound);
        }

        if (!maintaskbarfound)
        {
            try
            {
                windowHandles.Add(Win32.Intertop.FindWindow("Shell_TrayWnd", null));
            }
            catch { }
        }

        if (!sectaskbarfound)
        {
            if (Screen.AllScreens.Length >= 2)
            {
                try
                {
                    windowHandles.Add(Win32.Intertop.FindWindow("Shell_SecondaryTrayWnd", null));
                }
                catch { }
            }
        }

        return ActiveWindows;
    }

    public static bool Enumerator(IntPtr hwnd, int lParam)
    {
        StringBuilder sClassName = new StringBuilder("", 256);
        Win32.Intertop.GetClassName(hwnd, sClassName, 256);
        if (sClassName.ToString() == "Shell_TrayWnd" || sClassName.ToString() == "Shell_SecondaryTrayWnd")
        {
            windowHandles.Add(hwnd);
        }
        return true;
    }

    public static ArrayList windowHandles = new ArrayList();

    public static void ResetTaskbarStyle()
    {
        GetActiveWindows();

        ArrayList trays = new ArrayList();
        foreach (IntPtr trayWnd in windowHandles)
        {
            trays.Add(trayWnd);
        }

        foreach (IntPtr tray in trays)
        {
            IntPtr trayptr = tray;

            Win32.Intertop.SendMessage(trayptr, Win32.Intertop.WM_THEMECHANGED, true, 0);
            Win32.Intertop.SendMessage(trayptr, Win32.Intertop.WM_DWMCOLORIZATIONCOLORCHANGED, true, 0);
            Win32.Intertop.SendMessage(trayptr, Win32.Intertop.WM_DWMCOMPOSITIONCHANGED, true, 0);

            Win32.Types.Rect tt = new Win32.Types.Rect();
            Win32.Intertop.GetClientRect(trayptr, ref tt);

            Win32.Intertop.SetWindowRgn(trayptr, Win32.Intertop.CreateRectRgn(tt.Left, tt.Top, tt.Right, tt.Bottom), true);
        }
    }

    public static void RestartExplorer()
    {
        foreach (Process MyProcess in Process.GetProcessesByName("explorer"))
        {
            MyProcess.Kill();
        }
    }

    public static int ClearMemory()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        return Win32.Intertop.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
    }

    #endregion
}

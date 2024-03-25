using Microsoft.VisualBasic.ApplicationServices;
using Personate.Settings;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace TBStyler;

internal class Program
{
    public static NotifyIcon noty = new NotifyIcon();
    public static void Main(string[] args)
    {
        try
        {
            // Set default settings
            Settings.TaskbarStyle = 0;
            Settings.PrimaryTaskbarOffset = 0;
            Settings.SecondaryTaskbarOffset = 0;
            Settings.CenterPrimaryOnly = 0;
            Settings.CenterSecondaryOnly = 0;
            Settings.AnimationStyle = "cubiceaseinout";
            Settings.AnimationSpeed = 300;
            Settings.LoopRefreshRate = 400;
            Settings.CenterInBetween = 0;
            Settings.DontCenterTaskbar = 0;
            Settings.FixToolbarsOnTrayChange = 1;
            Settings.OnBatteryAnimationStyle = "cubiceaseinout";
            Settings.OnBatteryLoopRefreshRate = 400;
            Settings.RevertZeroBeyondTray = 1;
            Settings.TaskbarRounding = 0;
            Settings.TaskbarSegments = 0;

            bool stopgiven = false;

            // Read the arguments for the settings
            string[] arguments = Environment.GetCommandLineArgs();
            foreach (string argument in arguments)
            {
                string[] val = argument.Split('=');
                if (argument.Contains("-stop"))
                {
                    stopgiven = true;
                }
                if (argument.Contains("-showstartmenu"))
                {
                    Win32.Api.ShowStartMenu();
                    return;
                }
                if (argument.Contains("-console="))
                {
                    Win32.Intertop.AllocConsole();
                    Settings.ConsoleEnabled = 1;
                }
                if (argument.Contains("-tbs="))
                {
                    Settings.TaskbarStyle = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-color="))
                {
                    string colorval = val[1];
                    string[] colorsep = colorval.Split(';');

                    Settings.TaskbarStyleRed = Convert.ToInt32(colorsep[0]);
                    Settings.TaskbarStyleGreen = Convert.ToInt32(colorsep[1]);
                    Settings.TaskbarStyleBlue = Convert.ToInt32(colorsep[2]);
                    Settings.TaskbarStyleAlpha = Convert.ToInt32(colorsep[3]);
                }
                if (argument.Contains("-ptbo="))
                {
                    Settings.PrimaryTaskbarOffset = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-stbo="))
                {
                    Settings.SecondaryTaskbarOffset = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-cpo="))
                {
                    Settings.CenterPrimaryOnly = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-cso="))
                {
                    Settings.CenterSecondaryOnly = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-as="))
                {
                    Settings.AnimationStyle = val[1];
                }
                if (argument.Contains("-asp="))
                {
                    Settings.AnimationSpeed = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-lr="))
                {
                    Settings.LoopRefreshRate = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-cib="))
                {
                    Settings.CenterInBetween = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-obas="))
                {
                    Settings.OnBatteryAnimationStyle = val[1];
                }
                if (argument.Contains("-oblr="))
                {
                    Settings.OnBatteryLoopRefreshRate = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-ftotc="))
                {
                    Settings.FixToolbarsOnTrayChange = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-rzbt="))
                {
                    Settings.RevertZeroBeyondTray = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-sr="))
                {
                    Settings.SkipResolution = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-sr2="))
                {
                    Settings.SkipResolution2 = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-sr3="))
                {
                    Settings.SkipResolution3 = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-dtbsowm="))
                {
                    Settings.DefaultTaskbarStyleOnWinMax = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-cfsa="))
                {
                    Settings.CheckFullscreenApp = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-dct="))
                {
                    Settings.DontCenterTaskbar = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-hps="))
                {
                    Settings.HidePrimaryStartButton = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-hss="))
                {
                    Settings.HideSecondaryStartButton = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-hpt="))
                {
                    Settings.HidePrimaryNotifyWnd = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-hst="))
                {
                    Settings.HideSecondaryNotifyWnd = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-sti="))
                {
                    // Settings.ShowTrayIcon = Convert.ToInt32(val[1]);
                    Settings.ShowTrayIcon = 0; // Assuming it's commented out
                }
                if (argument.Contains("-tbsom="))
                {
                    Settings.TaskbarStyleOnMax = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-stsb="))
                {
                    Settings.StickyStartButton = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-tpop="))
                {
                    Settings.TotalPrimaryOpacity = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-tsop="))
                {
                    Settings.TotalSecondaryOpacity = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-tbr="))
                {
                    Settings.TaskbarRounding = Convert.ToInt32(val[1]);
                }
                if (argument.Contains("-tbsg="))
                {
                    Settings.TaskbarSegments = Convert.ToInt32(val[1]);
                }
            }


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
                Thread t1 = new Thread(TaskbarCenter.TaskbarCenterer);
                t1.Start();
            }

            // Start the TaskbarStyler if enabled
            if (Settings.TaskbarStyle == 1 || Settings.TaskbarStyle == 2 || Settings.TaskbarStyle == 3 || Settings.TaskbarStyle == 4 || Settings.TaskbarStyle == 5)
            {
                Thread t2 = new Thread(TaskbarStyle.TaskbarStyler);
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

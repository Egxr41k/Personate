using Microsoft.Win32;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using TBStyler.Win32.Types;

namespace TBStyler;

public class TaskbarCenter
{
    #region Values

    public static bool ScreensChanged;
    public static int TaskbarCount;
    public static ArrayList windowHandles = new ArrayList();
    public static bool trayfixed;
    public static IntPtr setposhwnd;
    public static int setpospos;
    public static string setposori;
    public static string initposcalc;
    public static bool initposcalcready;
    public static bool isanimating;
    public static UserPreferenceChangedEventHandler UserPref = new UserPreferenceChangedEventHandler(HandlePrefChange);

    #endregion

    public static void TaskbarCenterer()
    {
        RevertToZero();

        SystemEvents.DisplaySettingsChanged += DPChange;
        SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;

        Thread t1 = new Thread(Looper);
        t1.Start();

        if (Settings.FixToolbarsOnTrayChange == 1)
        {
            Thread t2 = new Thread(TrayLoopFix);
            t2.Start();
        }
    }

    #region Commands

    [DllImport("user32.dll")]
    public static extern int EnumWindows(CallBack Adress, int y);

    public delegate bool CallBack(IntPtr hwnd, int lParam);

    public static List<IntPtr> ActiveWindows = new List<IntPtr>();

    public static List<IntPtr> GetActiveWindows()
    {
        windowHandles.Clear();
        EnumWindows(Enumerator, 0);

        bool maintaskbarfound = false;
        bool sectaskbarfound = false;

        foreach (var Taskbar in windowHandles)
        {
            StringBuilder sClassName = new StringBuilder("", 256);
            Win32.Intertop.GetClassName((IntPtr)Taskbar, sClassName, 256);
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

    public struct RectangleX
    {
        public int left;
        public int top;
        public int width;
        public int height;
    }

    public static RectangleX GetLocation(Accessibility.IAccessible acc, int idChild)
    {
        RectangleX rect = new RectangleX();
        if (acc != null)
        {
            acc.accLocation(out rect.left, out rect.top, out rect.width, out rect.height, idChild);
        }
        return rect;
    }

    public static void SetPos()
    {
        if (setposori == "H")
        {
            do
            {
                Win32.Intertop.SetWindowPos(setposhwnd, IntPtr.Zero, setpospos, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                if (isanimating == true)
                {
                    break;
                }
            } while (trayfixed == true);
        }
        else
        {
            do
            {
                Win32.Intertop.SetWindowPos(setposhwnd, IntPtr.Zero, 0, setpospos, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                if (isanimating == true)
                {
                    break;
                }
            } while (trayfixed == true);
        }
    }

    public static void Animate(IntPtr hwnd, int oldpos, string orient, EasingDelegate easing, int valueToReach, int duration, bool isPrimary, int width)
    {
        try
        {
            Thread t1 = new Thread(() => TaskbarAnimate.Animate(hwnd, oldpos, orient, easing, valueToReach, duration, isPrimary, width));
            t1.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine("@Animation Call | " + ex.Message);
        }
    }

    public static bool revertcycle;

    public static void RevertToZero()
    {
        //Put all taskbars back to default position
        GetActiveWindows();

        foreach (Process prog in Process.GetProcesses())
        {
            if (prog.ProcessName == "AcrylicPanel")
            {
                prog.Kill();
            }
        }

        ArrayList Taskbars = new ArrayList();

        foreach (var Taskbar in windowHandles)
        {
            StringBuilder sClassName = new StringBuilder("", 256);
            Win32.Intertop.GetClassName((IntPtr)Taskbar, sClassName, 256);

            IntPtr MSTaskListWClass  = 0;

            if (sClassName.ToString() == "Shell_TrayWnd")
            {
                IntPtr ReBarWindow32 = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, (IntPtr)0, "ReBarWindow32", null);
                IntPtr MStart = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, (IntPtr)0, "Start", null);
                Win32.Intertop.ShowWindow(MStart, Win32.Types.ShowWindowCommands.Show);

                IntPtr MTray = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, (IntPtr)0, "TrayNotifyWnd", null);
                Win32.Intertop.SetWindowLong(MTray, (Win32.Types.WindowStyles)Win32.Intertop.GWL_STYLE, 0x56000000);
                Win32.Intertop.SetWindowLong(MTray, (Win32.Types.WindowStyles)Win32.Intertop.GWL_EXSTYLE, 0x2000);
                Win32.Intertop.SendMessage(MTray, 11, true, 0);
                Win32.Intertop.ShowWindow(MTray, Win32.Types.ShowWindowCommands.Show);

                IntPtr MSTaskSwWClass = Win32.Intertop.FindWindowEx(ReBarWindow32, (IntPtr)0, "MSTaskSwWClass", null);
                MSTaskListWClass = Win32.Intertop.FindWindowEx(MSTaskSwWClass, (IntPtr)0, "MSTaskListWClass", null);
            }

            if (sClassName.ToString() == "Shell_SecondaryTrayWnd")
            {
                IntPtr WorkerW = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, (IntPtr)0, "WorkerW", null);
                IntPtr SStart = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, (IntPtr)0, "Start", null);
                Win32.Intertop.ShowWindow(SStart, Win32.Types.ShowWindowCommands.Show);
                IntPtr STray = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, (IntPtr)0, "ClockButton", null);
                Win32.Intertop.ShowWindow(STray, Win32.Types.ShowWindowCommands.Show);
                MSTaskListWClass = Win32.Intertop.FindWindowEx(WorkerW, (IntPtr)0, "MSTaskListWClass", null);
            }

            Taskbars.Add(MSTaskListWClass);
        }

        foreach (var TaskList in Taskbars)
        {
            Win32.Intertop.SendMessage(Win32.Intertop.GetParent(Win32.Intertop.GetParent((IntPtr)TaskList)), 11, true, 0);
            Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, 0, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
        }
    }

    #endregion

    #region Events

    public static void HandlePrefChange(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
    {
        if (e.Category == Microsoft.Win32.UserPreferenceCategory.General)
        {
            Console.WriteLine();
            Thread.Sleep(1000);
            //Wait for Shell_TrayWnd
            IntPtr Handle;
            do
            {
                Console.WriteLine("Waiting for Shell_TrayWnd");
                Thread.Sleep(250);
                Handle = Win32.Intertop.FindWindowByClass("Shell_TrayWnd", (IntPtr)0);
            } while (Handle == IntPtr.Zero);

            Application.Restart();
        }
    }

    public static void DPChange(object sender, EventArgs e)
    {
        Console.WriteLine();
        Thread.Sleep(1000);
        //Wait for Shell_TrayWnd
        IntPtr Handle;
        do
        {
            Console.WriteLine("Waiting for Shell_TrayWnd");
            Thread.Sleep(250);
            Handle = Win32.Intertop.FindWindowByClass("Shell_TrayWnd", (IntPtr)0);
        } while (Handle == IntPtr.Zero);

        Application.Restart();
    }

    public static void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
    {
        Console.WriteLine();
        Thread.Sleep(1000);
        //Wait for Shell_TrayWnd
        IntPtr Handle;
        do
        {
            Console.WriteLine("Waiting for Shell_TrayWnd");
            Thread.Sleep(250);
            Handle = Win32.Intertop.FindWindowByClass("Shell_TrayWnd", (IntPtr)0);
        } while (Handle == IntPtr.Zero);

        Application.Restart();
    }

    #endregion

    #region Looper

    public static void Looper()
    {
        try
        {
            // This loop will check if the taskbar changes and requires a move
            GetActiveWindows();

            ArrayList Taskbars = new ArrayList();

            // Put all Taskbars into an ArrayList based on each TrayWnd in the TrayWnds ArrayList
            foreach (var Taskbar in windowHandles)
            {
                StringBuilder sClassName = new StringBuilder("", 256);
                Win32.Intertop.GetClassName((IntPtr)Taskbar, sClassName, 256);
                IntPtr MSTaskListWClass = IntPtr.Zero;

                Console.WriteLine(sClassName.ToString());

                if (sClassName.ToString() == "Shell_TrayWnd")
                {
                    IntPtr ReBarWindow32 = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, IntPtr.Zero, "ReBarWindow32", null);

                    if (Settings.TotalPrimaryOpacity != null)
                    {
                        Win32.Intertop.SetWindowLong((IntPtr)Taskbar, (WindowStyles)Win32.Intertop.GWL_EXSTYLE, 0x80000);
                        Win32.Intertop.SetLayeredWindowAttributes((IntPtr)Taskbar, 0, (byte)(255 / 100 * Convert.ToByte(Settings.TotalPrimaryOpacity)), 0x2);
                    }

                    if (Settings.HidePrimaryStartButton == 1)
                    {
                        IntPtr MStart = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, IntPtr.Zero, "Start", null);
                        Win32.Intertop.ShowWindow(MStart, ShowWindowCommands.Hide);
                        Win32.Intertop.SetLayeredWindowAttributes(MStart, 0, 0, 0x2);
                    }

                    if (Settings.HidePrimaryNotifyWnd == 1)
                    {
                        IntPtr MTray = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, IntPtr.Zero, "TrayNotifyWnd", null);
                        Win32.Intertop.ShowWindow(MTray, ShowWindowCommands.Hide);
                        Win32.Intertop.SetWindowLong(MTray, (WindowStyles)Win32.Intertop.GWL_STYLE, 0x7E000000);
                        Win32.Intertop.SetWindowLong(MTray, (WindowStyles)Win32.Intertop.GWL_EXSTYLE, 0x80000);
                        Win32.Intertop.SendMessage(MTray, 11, false, 0);
                        Win32.Intertop.SetLayeredWindowAttributes(MTray, 0, 0, 0x2);
                    }

                    IntPtr MSTaskSwWClass = Win32.Intertop.FindWindowEx(ReBarWindow32, IntPtr.Zero, "MSTaskSwWClass", null);
                    MSTaskListWClass = Win32.Intertop.FindWindowEx(MSTaskSwWClass, IntPtr.Zero, "MSTaskListWClass", null);
                }

                if (sClassName.ToString() == "Shell_SecondaryTrayWnd")
                {
                    IntPtr WorkerW = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, IntPtr.Zero, "WorkerW", null);

                    if (Settings.TotalSecondaryOpacity != null)
                    {
                        Win32.Intertop.SetWindowLong((IntPtr)Taskbar, (WindowStyles)Win32.Intertop.GWL_EXSTYLE, 0x80000);
                        Win32.Intertop.SetLayeredWindowAttributes((IntPtr)Taskbar, 0, (byte)(255 / 100 * Convert.ToByte(Settings.TotalSecondaryOpacity)), 0x2);
                    }

                    if (Settings.HideSecondaryStartButton == 1)
                    {
                        IntPtr SStart = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, IntPtr.Zero, "Start", null);
                        Win32.Intertop.ShowWindow(SStart, ShowWindowCommands.Hide);
                        Win32.Intertop.SetLayeredWindowAttributes(SStart, 0, 0, 0x2);
                    }

                    if (Settings.HideSecondaryNotifyWnd == 1)
                    {
                        IntPtr STray = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, IntPtr.Zero, "ClockButton", null);
                        Win32.Intertop.ShowWindow(STray, ShowWindowCommands.Hide);
                        Win32.Intertop.SetLayeredWindowAttributes(STray, 0, 0, 0x2);
                    }

                    MSTaskListWClass = Win32.Intertop.FindWindowEx(WorkerW, IntPtr.Zero, "MSTaskListWClass", null);
                }

                if (MSTaskListWClass == IntPtr.Zero)
                {
                    MessageBox.Show("TaskbarX: Could not find the handle of the taskbar. Your current version or build of Windows may not be supported.");
                    Environment.Exit(0);
                }

                Taskbars.Add(MSTaskListWClass);
            }

            List<Accessibility.IAccessible> TaskObjects = new List<Accessibility.IAccessible>();
            foreach (var TaskList in Taskbars)
            {
                Accessibility.IAccessible accessiblex = MSAA.Api.GetAccessibleFromHandle((IntPtr)TaskList);
                TaskObjects.Add(accessiblex);
            }

            // Start the endless loop
            while (true)
            {
                try
                {
                    string results = null;
                    string oldresults = null;

                    if (Settings.SkipResolution != 0)
                    {
                        if (Screen.PrimaryScreen.Bounds.Width == Settings.SkipResolution)
                        {
                            RevertToZero();
                            break;
                        }
                    }

                    if (Settings.SkipResolution2 != 0)
                    {
                        if (Screen.PrimaryScreen.Bounds.Width == Settings.SkipResolution2)
                        {
                            RevertToZero();
                            break;
                        }
                    }

                    if (Settings.SkipResolution3 != 0)
                    {
                        if (Screen.PrimaryScreen.Bounds.Width == Settings.SkipResolution3)
                        {
                            RevertToZero();
                            break;
                        }
                    }

                    if (Settings.CheckFullscreenApp == 1)
                    {
                        IntPtr activewindow = Win32.Intertop.GetForegroundWindow();
                        Screen curmonx = Screen.FromHandle(activewindow);
                        Rect activewindowsize = new Rect();
                        Win32.Intertop.GetWindowRect(activewindow, ref activewindowsize);

                        if (activewindowsize.Top == curmonx.Bounds.Top && activewindowsize.Bottom == curmonx.Bounds.Bottom && activewindowsize.Left == curmonx.Bounds.Left && activewindowsize.Right == curmonx.Bounds.Right)
                        {
                            Console.WriteLine("Fullscreen App detected " + activewindowsize.Bottom + "," + activewindowsize.Top + "," + activewindowsize.Left + "," + activewindowsize.Right);

                            Settings.Pause = true;
                            do
                            {
                                Thread.Sleep(500);
                                activewindow = Win32.Intertop.GetForegroundWindow();
                                Win32.Intertop.GetWindowRect(activewindow, ref activewindowsize);
                                Thread.Sleep(500);

                            } while (activewindowsize.Top == curmonx.Bounds.Top && activewindowsize.Bottom == curmonx.Bounds.Bottom && activewindowsize.Left == curmonx.Bounds.Left && activewindowsize.Right == curmonx.Bounds.Right);
                            Console.WriteLine("Fullscreen App deactivated");

                            Settings.Pause = false;
                        }
                    }

                    // Go through each taskbar and result in a unique string containing the current state
                    foreach (var TaskList in TaskObjects)
                    {
                        Accessibility.IAccessible[] children = MSAA.Api.GetAccessibleChildren((Accessibility.IAccessible)TaskList);

                        RectangleX TaskListPos = GetLocation(TaskList, 0);

                        int tH = TaskListPos.height;
                        int tW = TaskListPos.width;

                        RectangleX LastChildPos = new RectangleX();

                        foreach (Accessibility.IAccessible childx in children)
                        {
                            if ((int)childx.accRole[0] == 22) // 0x16 = toolbar
                            {
                                LastChildPos = GetLocation(childx, MSAA.Api.GetAccessibleChildren(childx).Length);
                                break;
                            }
                        }

                        int cL = LastChildPos.left;
                        int cT = LastChildPos.top;
                        int cW = LastChildPos.width;
                        int cH = LastChildPos.height;

                        try
                        {
                            int testiferror = cL;
                        }
                        catch (Exception)
                        {
                            // Current taskbar is empty, go to next taskbar.
                            continue;
                        }

                        string Orientation;
                        int TaskbarCount;
                        int TrayWndSize;

                        // Get current taskbar orientation (H = Horizontal | V = Vertical)
                        if (tH >= tW)
                        {
                            Orientation = "V";
                        }
                        else
                        {
                            Orientation = "H";
                        }

                        // Get the end position of the last icon in the taskbar
                        if (Orientation == "H")
                        {
                            TaskbarCount = cL + cW;
                        }
                        else
                        {
                            TaskbarCount = cT + cH;
                        }

                        // Gets the width of the whole taskbar's placeholder
                        TrayWndSize = Orientation == "H" ? tW : tH;

                        // Put the results into a string ready to be matched for differences with last loop
                        results += Orientation + TaskbarCount + TrayWndSize;

                        initposcalcready = true;
                    }

                    if (results != oldresults)
                    {
                        // Something has changed; we can now calculate the new position for each taskbar

                        initposcalcready = false;
                        initposcalc = results;

                        // Start the PositionCalculator
                        Thread t3 = new Thread(InitPositionCalculator);
                        t3.Start();
                    }

                    // Save current results for next loop
                    oldresults = results;

                    int loopRefreshRate = SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline ? Settings.OnBatteryLoopRefreshRate : Settings.LoopRefreshRate;
                    Thread.Sleep(loopRefreshRate);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("@Looper1 | " + ex.Message);

                    // Lost taskbar handles; restart application
                    if (ex.ToString().Contains("NullReference") || ex.ToString().Contains("Missing method"))
                    {
                        IntPtr Handle;
                        do
                        {
                            Handle = IntPtr.Zero;
                            System.Threading.Thread.Sleep(250);
                            Handle = Win32.Intertop.FindWindowByClass("Shell_TrayWnd", IntPtr.Zero);
                        } while (Handle == IntPtr.Zero);
                        System.Threading.Thread.Sleep(1000);
                        Application.Restart();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("@Looper2 | " + ex.Message);
        }
    }

    #endregion

    #region TrayLoopFix

    public static void TrayLoopFix()
    {
        try
        {
            IntPtr shellTrayWnd = Win32.Intertop.FindWindowByClass("Shell_TrayWnd", IntPtr.Zero);
            IntPtr trayNotifyWnd = Win32.Intertop.FindWindowEx(shellTrayWnd, IntPtr.Zero, "TrayNotifyWnd", null);
            IntPtr reBarWindow32 = Win32.Intertop.FindWindowEx(shellTrayWnd, IntPtr.Zero, "ReBarWindow32", null);
            IntPtr mSTaskSwWClass = Win32.Intertop.FindWindowEx(reBarWindow32, IntPtr.Zero, "MSTaskSwWClass", null);
            IntPtr mSTaskListWClass = Win32.Intertop.FindWindowEx(mSTaskSwWClass, IntPtr.Zero, "MSTaskListWClass", null);

            Accessibility.IAccessible accessible = MSAA.Api.GetAccessibleFromHandle(mSTaskListWClass);
            Accessibility.IAccessible accessible2 = MSAA.Api.GetAccessibleFromHandle(trayNotifyWnd);
            Accessibility.IAccessible accessible3 = MSAA.Api.GetAccessibleFromHandle(mSTaskSwWClass);

            uint SWP_NOSIZE = 1;
            uint SWP_ASYNCWINDOWPOS = 16384;
            uint SWP_NOACTIVATE = 16;
            uint SWP_NOSENDCHANGING = 1024;
            uint SWP_NOZORDER = 4;

            do
            {
                RectangleX rebarPos = GetLocation(accessible3, 0);
                RectangleX trayNotifyPos = GetLocation(accessible2, 0);
                RectangleX taskListPos = GetLocation(accessible, 0);

                Win32.Intertop.SendMessage(reBarWindow32, 11, false, 0);
                Win32.Intertop.SendMessage(Win32.Intertop.GetParent(shellTrayWnd), 11, false, 0);

                int trayNotifyWidth = 0;
                int oldTrayNotifyWidth = 0;
                string trayOrientation;

                // If the TrayNotifyWnd updates then refresh the taskbar
                if (taskListPos.height >= taskListPos.width)
                {
                    trayOrientation = "V";
                }
                else
                {
                    trayOrientation = "H";
                }

                trayNotifyWidth = trayNotifyPos.width;

                if (trayNotifyWidth != oldTrayNotifyWidth)
                {
                    if (oldTrayNotifyWidth != 0)
                    {
                        if (taskListPos.left != 0)
                        {
                            if (trayNotifyPos.left == 3)
                            {
                                break;
                            }

                            int pos = Math.Abs((taskListPos.left - rebarPos.left));

                            trayfixed = false;

                            setposhwnd = mSTaskListWClass;
                            setpospos = pos;
                            setposori = trayOrientation;

                            Thread t1 = new Thread(SetPos);
                            t1.Start();

                            Thread.Sleep(5);
                            Win32.Intertop.SendMessage(reBarWindow32, 11, true, 0);
                            Thread.Sleep(5);
                            Win32.Intertop.SendMessage(reBarWindow32, 11, false, 0);
                            Thread.Sleep(5);
                            trayfixed = true;
                        }
                    }
                }

                oldTrayNotifyWidth = trayNotifyWidth;

                Thread.Sleep(400);

            } while (true);
        }
        catch (Exception ex)
        {
            Console.WriteLine("@TrayLoopFix | " + ex.Message);
        }
    }

    #endregion

    #region PositionCalculator

    public static void InitPositionCalculator()
    {
        string mm;
        string mm2;

        mm = initposcalc;

        do
        {
            Thread.Sleep(10);
        } while (!initposcalcready);

        mm2 = initposcalc;

        if (mm == mm2)
        {
            // Start the PositionCalculator
            Thread t3 = new Thread(PositionCalculator);
            t3.Start();
        }
    }

    public static void PositionCalculator()
    {
        try
        {
            // Calculate the new positions and pass them through to the animator
            ArrayList Taskbars = new ArrayList();

            // Put all Taskbars into an ArrayList based on each TrayWnd in the TrayWnds ArrayList
            foreach (var Taskbar in windowHandles)
            {
                StringBuilder sClassName = new StringBuilder("", 256);
                Win32.Intertop.GetClassName((IntPtr)Taskbar, sClassName, 256);

                IntPtr MSTaskListWClass = IntPtr.Zero;

                if (sClassName.ToString() == "Shell_TrayWnd")
                {
                    IntPtr ReBarWindow32 = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, IntPtr.Zero, "ReBarWindow32", null);
                    IntPtr MSTaskSwWClass = Win32.Intertop.FindWindowEx(ReBarWindow32, IntPtr.Zero, "MSTaskSwWClass", null);
                    MSTaskListWClass = Win32.Intertop.FindWindowEx(MSTaskSwWClass, IntPtr.Zero, "MSTaskListWClass", null);
                }

                if (sClassName.ToString() == "Shell_SecondaryTrayWnd")
                {
                    IntPtr WorkerW = Win32.Intertop.FindWindowEx((IntPtr)Taskbar, IntPtr.Zero, "WorkerW", null);
                    MSTaskListWClass = Win32.Intertop.FindWindowEx(WorkerW, IntPtr.Zero, "MSTaskListWClass", null);
                }

                Win32.Intertop.SetWindowLong((IntPtr)Taskbar, (WindowStyles)Win32.Intertop.GWL_EXSTYLE, 0x80);

                if (MSTaskListWClass == IntPtr.Zero)
                {
                    Console.WriteLine("TaskbarX: Could not find the handle of the taskbar. Restarting...");
                    Thread.Sleep(1000);
                    Application.Restart();
                }

                Taskbars.Add(MSTaskListWClass);
            }

            // Calculate Position for every taskbar and trigger the animator
            foreach (var TaskList in Taskbars)
            {
                StringBuilder sClassName = new StringBuilder("", 256);
                Win32.Intertop.GetClassName((IntPtr)TaskList, sClassName, 256);

                RectangleX LastChildPos = new RectangleX();
                RectangleX TaskListPos = new RectangleX();

                Accessibility.IAccessible accessible = MSAA.Api.GetAccessibleFromHandle((IntPtr)TaskList);
                Accessibility.IAccessible[] children = MSAA.Api.GetAccessibleChildren(accessible);

                TaskListPos = GetLocation(accessible, 0);

                foreach (Accessibility.IAccessible childx in children)
                {
                    if (Convert.ToInt32(childx.accRole[0]) == 22) // 0x16 = toolbar
                    {
                        LastChildPos = GetLocation(childx, MSAA.Api.GetAccessibleChildren(childx).Length);
                        break;
                    }
                }

                IntPtr RebarHandle = Win32.Intertop.GetParent((IntPtr)TaskList);
                Accessibility.IAccessible accessible3 = MSAA.Api.GetAccessibleFromHandle(RebarHandle);

                StringBuilder RebarClassName = new StringBuilder("", 256);
                Win32.Intertop.GetClassName(RebarHandle, RebarClassName, 256);

                string Orientation;
                int TaskbarWidth;
                int TrayWndLeft;
                int TrayWndWidth;
                int RebarWndLeft;
                int TaskbarLeft;
                int NewPosition;
                int curleft;
                int curleft2;

                RectangleX TrayNotifyPos = new RectangleX();
                RectangleX NewsAndInterestsPos = new RectangleX();
                IntPtr NewsAndInterestsHandle = IntPtr.Zero;

                IntPtr TrayWndHandle = Win32.Intertop.GetParent(Win32.Intertop.GetParent((IntPtr)TaskList));

                StringBuilder TrayWndClassName = new StringBuilder("", 256);
                Win32.Intertop.GetClassName(TrayWndHandle, TrayWndClassName, 256);

                // Check if TrayWnd = wrong. if it is, correct it (This will be the primary taskbar which should be Shell_TrayWnd)
                if (TrayWndClassName.ToString() == "ReBarWindow32")
                {
                    Win32.Intertop.SendMessage(TrayWndHandle, 11, false, 0);
                    TrayWndHandle = Win32.Intertop.GetParent(Win32.Intertop.GetParent(Win32.Intertop.GetParent((IntPtr)TaskList)));

                    IntPtr TrayNotify = Win32.Intertop.FindWindowEx(TrayWndHandle, IntPtr.Zero, "TrayNotifyWnd", null);
                    Accessibility.IAccessible accessible4 = MSAA.Api.GetAccessibleFromHandle(TrayNotify);
                    TrayNotifyPos = GetLocation(accessible4, 0);

                    IntPtr NewsAndInterests = Win32.Intertop.FindWindowEx(TrayWndHandle, IntPtr.Zero, "DynamicContent1", null);
                    if (!Convert.ToInt32(NewsAndInterests.ToString()).Equals(Convert.ToInt32("0")))
                    {
                        NewsAndInterestsHandle = NewsAndInterests;
                        Accessibility.IAccessible accessible5 = MSAA.Api.GetAccessibleFromHandle(NewsAndInterests);
                        NewsAndInterestsPos = GetLocation(accessible5, 0);
                    }

                    Win32.Intertop.SendMessage(Win32.Intertop.GetParent(TrayWndHandle), 11, false, 0);
                }

                Win32.Intertop.GetClassName(TrayWndHandle, TrayWndClassName, 256);
                Accessibility.IAccessible accessible2 = MSAA.Api.GetAccessibleFromHandle(TrayWndHandle);

                RectangleX TrayWndPos = GetLocation(accessible2, 0);
                RectangleX RebarPos = GetLocation(accessible3, 0);

                // If the taskbar is still moving then wait until it's not (This will prevent unneeded calculations that trigger the animator)
                do
                {
                    curleft = TaskListPos.left;
                    TaskListPos = GetLocation(accessible, 0);
                    System.Threading.Thread.Sleep(30);
                    curleft2 = TaskListPos.left;
                } while (curleft != curleft2);

                // Get current taskbar orientation (H = Horizontal | V = Vertical)
                if (TaskListPos.height >= TaskListPos.width)
                {
                    Orientation = "V";
                }
                else
                {
                    Orientation = "H";
                }

                // Calculate the exact width of the total icons
                try
                {
                    if (Orientation == "H")
                    {
                        TaskbarWidth = Convert.ToInt32((LastChildPos.left - TaskListPos.left));
                    }
                    else
                    {
                        TaskbarWidth = Convert.ToInt32((LastChildPos.top - TaskListPos.top));
                    }
                }
                catch
                {
                    TaskbarWidth = 0;
                    // Taskbar is empty just skip
                }

                // Get info needed to calculate the position
                if (Orientation == "H")
                {
                    TrayWndLeft = Math.Abs(Convert.ToInt32(TrayWndPos.left));
                    TrayWndWidth = Math.Abs(Convert.ToInt32(TrayWndPos.width));
                    RebarWndLeft = Math.Abs(Convert.ToInt32(RebarPos.left));
                    TaskbarLeft = Math.Abs(Convert.ToInt32(RebarWndLeft - TrayWndLeft));
                }
                else
                {
                    TrayWndLeft = Math.Abs(Convert.ToInt32(TrayWndPos.top));
                    TrayWndWidth = Math.Abs(Convert.ToInt32(TrayWndPos.height));
                    RebarWndLeft = Math.Abs(Convert.ToInt32(RebarPos.top));
                    TaskbarLeft = Math.Abs(Convert.ToInt32(RebarWndLeft - TrayWndLeft));
                }

                Console.WriteLine("!" + NewsAndInterestsPos.width);

                // Calculate new position
                if (TrayWndClassName.ToString() == "Shell_TrayWnd")
                {
                    if (Settings.CenterInBetween == 1)
                    {
                        if (Orientation == "H")
                        {
                            int offset = (TrayNotifyPos.width / 2 - (TaskbarLeft / 2)) + NewsAndInterestsPos.width / 2;
                            NewPosition = Math.Abs(TrayWndLeft - offset);
                        }
                        else
                        {
                            NewPosition = Math.Abs(TrayWndLeft + (TaskbarWidth / 2) - (TrayWndWidth / 2));
                        }
                    }
                    else
                    {
                        if (Settings.CenterPrimaryOnly == 1)
                        {
                            NewPosition = Math.Abs(TrayWndLeft + (TaskbarWidth / 2) - (TrayWndWidth / 2));
                        }
                        else
                        {
                            NewPosition = TrayWndLeft;
                        }
                    }
                }
                else
                {
                    if (Settings.CenterSecondaryOnly == 1)
                    {
                        NewPosition = Math.Abs(TrayWndLeft + (TaskbarWidth / 2) - (TrayWndWidth / 2));
                    }
                    else
                    {
                        NewPosition = TrayWndLeft;
                    }
                }


                if (Settings.TaskbarSegments == 1)
                {
                    if (Orientation == "H")
                    {
                        Rect ttseg = new Rect();
                        Win32.Intertop.GetClientRect((nint)TaskList, ref ttseg);

                        Rect trayseg = new Rect();
                        Win32.Intertop.GetClientRect(
                            Win32.Intertop.FindWindowEx(
                                TrayWndHandle, 0,
                                "TrayNotifyWnd",
                                null),
                            ref trayseg);

                        Rect clockseg = new Rect();
                        Win32.Intertop.GetClientRect(
                            Win32.Intertop.FindWindowEx(
                                TrayWndHandle,
                                0, "ClockButton",
                                null),
                            ref clockseg);


                        Rect startseg = new Rect();
                        Win32.Intertop.GetClientRect(
                            Win32.Intertop.FindWindowEx(
                                TrayWndHandle,
                                0,
                                "Start",
                                null),
                            ref startseg);

                        //Rect searchseg = new Rect();
                        //Win32.Intertop.GetWindowRect(
                        //    Win32.Intertop.FindWindowEx(
                        //        TrayWndHandle, 
                        //        0, 
                        //        "TrayButton", 
                        //        "Type here to search"), 
                        //    ref searchseg);

                        //Rect cortanaseg = new Rect();
                        //Win32.Intertop.GetWindowRect(
                        //    Win32.Intertop.FindWindowEx(
                        //        TrayWndHandle, 
                        //        0, 
                        //        "TrayButton", 
                        //        "Talk to Cortana"), 
                        //    ref cortanaseg);

                        //Rect taskviewseg = new Rect();
                        //Win32.Intertop.GetWindowRect(
                        //    Win32.Intertop.FindWindowEx(
                        //        TrayWndHandle, 
                        //        0, 
                        //        "TrayButton", 
                        //        "Task View"), 
                        //    ref taskviewseg);

                        //if (settings.TaskbarRounding != 0)
                        //{
                        //    Win32.Intertop.SetWindowRgn(
                        //        TrayWndHandle,
                        //        Win32.Intertop.CreateRoundRectRgn(
                        //            TaskbarLeft + newPosition + 4,
                        //            ttseg.Top,
                        //            TaskbarLeft + newPosition + TaskbarWidth - 2,
                        //            ttseg.Bottom + 1,
                        //            settings.TaskbarRounding,
                        //            settings.TaskbarRounding), 
                        //        true);
                        //}
                        //else
                        //{

                        //}

                        IntPtr Tasklist_rgn = Win32.Intertop.CreateRoundRectRgn(
                            TaskbarLeft + NewPosition + 4,
                            ttseg.Top,
                            TaskbarLeft + NewPosition + TaskbarWidth - 2,
                            ttseg.Bottom + 1,
                            Settings.TaskbarRounding,
                            Settings.TaskbarRounding);

                        IntPtr NotifyTray_rgn = Win32.Intertop.CreateRoundRectRgn(
                            TrayNotifyPos.left,
                            0,
                            TrayNotifyPos.left + TrayNotifyPos.width,
                            TrayNotifyPos.top + TrayNotifyPos.height,
                            Settings.TaskbarRounding,
                            Settings.TaskbarRounding);

                        IntPtr Start_rgn = Win32.Intertop.CreateRoundRectRgn(
                            startseg.Left,
                            0,
                            startseg.Right,
                            startseg.Bottom,
                            Settings.TaskbarRounding,
                            Settings.TaskbarRounding);

                        //IntPtr Search_rgn = Win32.Intertop.CreateRectRgn(
                        //    searchseg.Left, 
                        //    0, 
                        //    searchseg.Right, 
                        //    searchseg.Bottom);

                        //IntPtr Cortana_rgn = Win32.Intertop.CreateRectRgn(
                        //    cortanaseg.Left, 
                        //    0, 
                        //    cortanaseg.Right, 
                        //    cortanaseg.Bottom);

                        //IntPtr TaskView_rgn = Win32.Intertop.CreateRectRgn(
                        //    taskviewseg.Left, 
                        //    0, 
                        //    taskviewseg.Right, 
                        //    taskviewseg.Bottom);

                        IntPtr Clock_rgn = Win32.Intertop.CreateRoundRectRgn(
                            clockseg.Left,
                            0,
                            clockseg.Right,
                            clockseg.Bottom,
                            Settings.TaskbarRounding,
                            Settings.TaskbarRounding);


                        IntPtr Totalreg = Win32.Intertop.CreateRoundRectRgn(
                            0,
                            0,
                            0,
                            0,
                            0,
                            0);

                        Win32.Intertop.CombineRgn(
                            Totalreg,
                            Tasklist_rgn,
                            NotifyTray_rgn, 2);

                        if (TrayWndClassName.ToString() == "Shell_TrayWnd")
                        {
                            Win32.Intertop.CombineRgn(
                                Totalreg,
                                Totalreg,
                                Start_rgn,
                                2);
                        }

                        //Win32.Intertop.CombineRgn(
                        //    Totalreg, 
                        //    Totalreg, 
                        //    Search_rgn, 
                        //    2);

                        //Win32.Intertop.CombineRgn(
                        //    Totalreg, 
                        //    Totalreg, 
                        //    Cortana_rgn, 
                        //    2);

                        //Win32.Intertop.CombineRgn(
                        //    Totalreg, 
                        //    Totalreg, 
                        //    TaskView_rgn, 
                        //    2);

                        //Win32.Intertop.CombineRgn(
                        //    Totalreg, 
                        //    Totalreg, 
                        //    Clock_rgn, 
                        //    2);

                        Win32.Intertop.SetWindowRgn(
                            TrayWndHandle,
                            Totalreg,
                            true);

                        //Win32.Intertop.SetWindowRgn(
                        //    TrayWndHandle,
                        //    Win32.Intertop.CreateRectRgn(
                        //        TaskbarLeft + newPosition + 4,
                        //        ttseg.Top,
                        //        TaskbarLeft + newPosition + TaskbarWidth - 2,
                        //        ttseg.Bottom + 1),
                        //    true);
                        //}
                    }
                    else
                    {
                        Rect ttseg = new Rect();
                        Win32.Intertop.GetClientRect((nint)TaskList, ref ttseg);
                        Rect trayseg = new Rect();
                        Win32.Intertop.GetClientRect(
                            Win32.Intertop.FindWindowEx(
                                TrayWndHandle,
                                0,
                                "TrayNotifyWnd",
                                null),
                            ref trayseg);

                        Rect clockseg = new Rect();
                        Win32.Intertop.GetClientRect(
                            Win32.Intertop.FindWindowEx(
                                TrayWndHandle,
                                0,
                                "ClockButton",
                                null),
                            ref clockseg);


                        Rect startseg = new Rect();
                        Win32.Intertop.GetClientRect(
                            Win32.Intertop.FindWindowEx(
                                TrayWndHandle,
                                0,
                                "Start",
                                null),
                            ref startseg);

                        //Rect searchseg = new Rect();
                        //Win32.Intertop.GetWindowRect(
                        //    Win32.Intertop.FindWindowEx(
                        //        TrayWndHandle, 
                        //        0, 
                        //        "TrayButton", 
                        //        "Type here to search"), 
                        //    ref searchseg);

                        //Rect cortanaseg = new Rect();
                        //Win32.Intertop.GetWindowRect(
                        //    Win32.Intertop.FindWindowEx(
                        //        TrayWndHandle, 
                        //        0, 
                        //        "TrayButton", 
                        //        "Talk to Cortana"), 
                        //    ref cortanaseg);

                        //Rect taskviewseg = new Rect();
                        //Win32.Intertop.GetWindowRect(
                        //    Win32.Intertop.FindWindowEx(
                        //        TrayWndHandle, 
                        //        0, 
                        //        "TrayButton", 
                        //        "Task View"), 
                        //    ref taskviewseg);

                        //if (settings.TaskbarRounding != 0)
                        //{
                        //    Win32.Intertop.SetWindowRgn(TrayWndHandle,
                        //        Win32.Intertop.CreateRoundRectRgn(
                        //            TaskbarLeft + newPosition + 4,
                        //            ttseg.Top, TaskbarLeft + newPosition + TaskbarWidth - 2,
                        //            ttseg.Bottom + 1,
                        //            settings.TaskbarRounding,
                        //            settings.TaskbarRounding),
                        //        true);
                        //}
                        //else
                        //{

                        IntPtr Tasklist_rgn = Win32.Intertop.CreateRoundRectRgn(
                            ttseg.Left,
                            TaskbarLeft + NewPosition + 4,
                            ttseg.Right,
                            TaskbarLeft + NewPosition + TaskbarWidth - 2,
                            Settings.TaskbarRounding,
                            Settings.TaskbarRounding);

                        //IntPtr NotifyTray_rgn = Win32.Intertop.CreateRoundRectRgn(
                        //    trayseg.Left, 
                        //    trayseg.Top, 
                        //    trayseg.Right, 
                        //    trayseg.Bottom, 
                        //    settings.TaskbarRounding, 
                        //    settings.TaskbarRounding);

                        //IntPtr Start_rgn = Win32.Intertop.CreateRoundRectRgn(
                        //    startseg.Left, 
                        //    0, 
                        //    startseg.Right, 
                        //    startseg.Bottom, 
                        //    settings.TaskbarRounding, 
                        //    settings.TaskbarRounding);

                        //IntPtr Search_rgn = Win32.Intertop.CreateRectRgn(
                        //    searchseg.Left, 
                        //    0, 
                        //    searchseg.Right, 
                        //    searchseg.Bottom);

                        //IntPtr Cortana_rgn = Win32.Intertop.CreateRectRgn(
                        //    cortanaseg.Left, 
                        //    0, 
                        //    cortanaseg.Right, 
                        //    cortanaseg.Bottom);

                        //IntPtr TaskView_rgn = Win32.Intertop.CreateRectRgn(
                        //    taskviewseg.Left, 
                        //    0, 
                        //    taskviewseg.Right, 
                        //    taskviewseg.Bottom);

                        //IntPtr Clock_rgn = Win32.Intertop.CreateRoundRectRgn(
                        //    clockseg.Left, 
                        //    0, 
                        //    clockseg.Right, 
                        //    clockseg.Bottom, 
                        //    settings.TaskbarRounding, 
                        //    settings.TaskbarRounding);


                        IntPtr Totalreg = Win32.Intertop.CreateRoundRectRgn(
                            0,
                            0,
                            0,
                            0,
                            0,
                            0);

                        Win32.Intertop.CombineRgn(
                            Totalreg,
                            Tasklist_rgn,
                            Tasklist_rgn,
                            2);

                        //Win32.Intertop.CombineRgn(
                        //    Totalreg, 
                        //    Totalreg, 
                        //    Start_rgn, 
                        //    2);

                        //Win32.Intertop.CombineRgn(
                        //    Totalreg, 
                        //    Totalreg, 
                        //    Search_rgn, 
                        //    2);

                        //Win32.Intertop.CombineRgn(
                        //    Totalreg, 
                        //    Totalreg, 
                        //    Cortana_rgn, 
                        //    2);

                        //Win32.Intertop.CombineRgn(
                        //    Totalreg, 
                        //    Totalreg, 
                        //    TaskView_rgn, 
                        //    2);

                        //Win32.Intertop.CombineRgn(
                        //    Totalreg, 
                        //    Totalreg, 
                        //    Clock_rgn, 
                        //    2);

                        Win32.Intertop.SetWindowRgn(
                            TrayWndHandle,
                            Totalreg,
                            true);

                        //Win32.Intertop.SetWindowRgn(
                        //    TrayWndHandle,
                        //    Win32.Intertop.CreateRectRgn(
                        //        TaskbarLeft + newPosition + 4,
                        //        ttseg.Top,
                        //        TaskbarLeft + newPosition + TaskbarWidth - 2,
                        //        ttseg.Bottom + 1),
                        //    true);
                        //}
                    }
                }
                else
                {
                    if (Settings.TaskbarRounding != 0)
                    {
                        Win32.Intertop.SetWindowRgn(
                            TrayWndHandle,
                            Win32.Intertop.CreateRoundRectRgn(
                                0,
                                0,
                                TrayWndPos.width,
                                TrayWndPos.height,
                                Settings.TaskbarRounding,
                                Settings.TaskbarRounding), true);
                    }
                }



                //IntPtr sb = Win32.Intertop.FindWindowEx(
                //    TrayWndHandle,
                //    0,
                //    "Start",
                //    null);

                //var sbrect = new Rect();
                //Win32.Intertop.GetClientRect(sb, ref sbrect);

                //Win32.Intertop.SetWindowPos(
                //    sb,
                //    IntPtr.Zero,
                //    TaskbarLeft + newPosition + 4 + (sbrect.Left - sbrect.Right),
                //    0,
                //    0,
                //    0,
                //    Win32.Intertop.SWP_NOSIZE |
                //    Win32.Intertop.SWP_ASYNCWINDOWPOS |
                //    Win32.Intertop.SWP_NOACTIVATE |
                //    Win32.Intertop.SWP_NOZORDER |
                //    Win32.Intertop.SWP_NOSENDCHANGING);

                //Win32.Intertop.SendMessage(
                //    TrayWndHandle,
                //    Win32.Intertop.WM_DWMCOMPOSITIONCHANGED,
                //    true,
                //    0);

                if (Settings.TaskbarSegments == 1)
                {
                    if (Settings.TaskbarStyle != 0)
                    {
                        //Win32.Intertop.SendMessage(
                        //    TrayWndHandle,
                        //    Win32.Intertop.WM_THEMECHANGED,
                        //    true,
                        //    0);

                        //Win32.Intertop.SendMessage(
                        //    TrayWndHandle,
                        //    Win32.Intertop.WM_DWMCOLORIZATIONCOLORCHANGED,
                        //    true,
                        //    0);

                        Win32.Intertop.SendMessage(
                            TrayWndHandle,
                            Win32.Intertop.WM_DWMCOMPOSITIONCHANGED,
                            true,
                            0);
                    }
                }

                //Win32.SetWindowPos(
                //  TrayWndHandle,
                //  Win32.HWND_TOPMOST,
                //  0,
                //  0,
                //  0,
                //  0,
                //  Win32.TOPMOST_FLAGS)

                if (SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline)
                {
                    if (Settings.CenterPrimaryOnly == 1)
                    {
                        if (TrayWndClassName.ToString() == "Shell_TrayWnd")
                        {
                            if (Orientation == "H")
                            {
                                if (Settings.OnBatteryAnimationStyle == "none")
                                {
                                    Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, NewPosition, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                                }
                                DaAnimator(Settings.OnBatteryAnimationStyle, (IntPtr)TaskList, TaskListPos.left, RebarPos.left, "H", NewPosition, true, TaskbarWidth);
                            }
                            else
                            {
                                if (Settings.OnBatteryAnimationStyle == "none")
                                {
                                    Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, 0, NewPosition, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                                }
                                else
                                {
                                    DaAnimator(Settings.OnBatteryAnimationStyle, (IntPtr)TaskList, TaskListPos.top, RebarPos.top, "V", NewPosition, true, TaskbarWidth);
                                }
                            }
                        }
                    }
                    else if (Settings.CenterSecondaryOnly == 1)
                    {
                        if (TrayWndClassName.ToString() == "Shell_SecondaryTrayWnd")
                        {
                            if (Orientation == "H")
                            {
                                if (Settings.OnBatteryAnimationStyle == "none")
                                {
                                    Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, NewPosition, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                                }
                                else
                                {
                                    DaAnimator(Settings.OnBatteryAnimationStyle, (IntPtr)TaskList, TaskListPos.left, RebarPos.left, "H", NewPosition, false, TaskbarWidth);
                                }
                            }
                            else
                            {
                                if (Settings.OnBatteryAnimationStyle == "none")
                                {
                                    Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, 0, NewPosition, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                                }
                                else
                                {
                                    DaAnimator(Settings.OnBatteryAnimationStyle, (IntPtr)TaskList, TaskListPos.top, RebarPos.top, "V", NewPosition, false, TaskbarWidth);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Orientation == "H")
                        {
                            if (Settings.OnBatteryAnimationStyle == "none")
                            {
                                Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, NewPosition, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                            }
                            else
                            {
                                DaAnimator(Settings.OnBatteryAnimationStyle, (IntPtr)TaskList, TaskListPos.left, RebarPos.left, "H", NewPosition, false, TaskbarWidth);
                            }
                        }
                        else
                        {
                            if (Settings.OnBatteryAnimationStyle == "none")
                            {
                                Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, 0, NewPosition, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                            }
                            else
                            {
                                DaAnimator(Settings.OnBatteryAnimationStyle, (IntPtr)TaskList, TaskListPos.top, RebarPos.top, "V", NewPosition, false, TaskbarWidth);
                            }
                        }
                    }
                }
                else
                {
                    if (Settings.CenterPrimaryOnly == 1)
                    {
                        if (TrayWndClassName.ToString() == "Shell_TrayWnd")
                        {
                            if (Orientation == "H")
                            {
                                if (Settings.AnimationStyle == "none")
                                {
                                    Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, NewPosition, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                                }
                                else
                                {
                                    DaAnimator(Settings.AnimationStyle, (IntPtr)TaskList, TaskListPos.left, RebarPos.left, "H", NewPosition, true, TaskbarWidth);
                                }
                            }
                            else
                            {
                                if (Settings.AnimationStyle == "none")
                                {
                                    Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, 0, NewPosition, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                                }
                                else
                                {
                                    DaAnimator(Settings.AnimationStyle, (IntPtr)TaskList, TaskListPos.top, RebarPos.top, "V", NewPosition, true, TaskbarWidth);
                                }
                            }
                        }
                    }
                    else if (Settings.CenterSecondaryOnly == 1)
                    {
                        if (TrayWndClassName.ToString() == "Shell_SecondaryTrayWnd")
                        {
                            if (Orientation == "H")
                            {
                                if (Settings.AnimationStyle == "none")
                                {
                                    Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, NewPosition, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                                }
                                else
                                {
                                    DaAnimator(Settings.AnimationStyle, (IntPtr)TaskList, TaskListPos.left, RebarPos.left, "H", NewPosition, false, TaskbarWidth);
                                }
                            }
                            else
                            {
                                if (Settings.AnimationStyle == "none")
                                {
                                    Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, 0, NewPosition, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                                }
                                else
                                {
                                    DaAnimator(Settings.AnimationStyle, (IntPtr)TaskList, TaskListPos.top, RebarPos.top, "V", NewPosition, false, TaskbarWidth);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Orientation == "H")
                        {
                            if (Settings.AnimationStyle == "none")
                            {
                                Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, NewPosition, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                            }
                            else
                            {
                                DaAnimator(Settings.AnimationStyle, (IntPtr)TaskList, TaskListPos.left, RebarPos.left, "H", NewPosition, false, TaskbarWidth);
                            }
                        }
                        else
                        {
                            if (Settings.AnimationStyle == "none")
                            {
                                Win32.Intertop.SetWindowPos((IntPtr)TaskList, IntPtr.Zero, 0, NewPosition, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                            }
                            else
                            {
                                DaAnimator(Settings.AnimationStyle, (IntPtr)TaskList, TaskListPos.top, RebarPos.top, "V", NewPosition, false, TaskbarWidth);
                            }
                        }
                    }
                } 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("@Calculator | " + ex.Message);
        }
    }


    private static void DaAnimator(string animationStyle, IntPtr taskList, int taskListc, int rebarc, string orient, int position, bool isprimary, int width)
    {
        if (animationStyle == "linear")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.Linear, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "expoeaseout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.ExpoEaseOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "expoeasein")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.ExpoEaseIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "expoeaseinout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.ExpoEaseInOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "expoeaseoutin")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.ExpoEaseOutIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "circeaseout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.CircEaseOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "circeasein")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.CircEaseIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "circeaseinout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.CircEaseInOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "circeaseoutin")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.CircEaseOutIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quadeaseout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuadEaseOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quadeasein")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuadEaseIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quadeaseinout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuadEaseInOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quadeaseoutin")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuadEaseOutIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "sineeaseout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.SineEaseOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "sineeasein")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.SineEaseIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "sineeaseinout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.SineEaseInOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "sineeaseoutin")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.SineEaseOutIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "cubiceaseout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.CubicEaseOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "cubiceasein")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.CubicEaseIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "cubiceaseinout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.CubicEaseInOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "cubiceaseoutin")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.CubicEaseOutIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quarteaseout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuartEaseOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quarteasein")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuartEaseIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quarteaseinout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuartEaseInOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quarteaseoutin")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuartEaseOutIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quinteaseout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuintEaseOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quinteasein")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuintEaseIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "quinteaseinout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuintEaseInOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if(animationStyle == "quinteaseoutin")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.QuintEaseOutIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "elasticeaseout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.ElasticEaseOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "elasticeasein")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.ElasticEaseIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "elasticeaseinout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.ElasticEaseInOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "elasticeaseoutin")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.ElasticEaseOutIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "bounceeaseout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.BounceEaseOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "bounceeasein")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.BounceEaseIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "bounceeaseinout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.BounceEaseInOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "bounceeaseoutin")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.BounceEaseOutIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "backeaseout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.BackEaseOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "backeasein")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.BackEaseIn, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "backeaseinout")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.BackEaseInOut, position, Settings.AnimationSpeed, isprimary, width);
        }
        else if (animationStyle == "backeaseoutin")
        {
            Animate(taskList, taskListc - rebarc, orient, Easings.BackEaseOutIn, position, Settings.AnimationSpeed, isprimary, width);
        }

    }

    #endregion
}

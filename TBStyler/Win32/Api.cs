using System.Text;
using TBStyler.Win32.Types;

namespace TBStyler.Win32;

public class Api
{
    public static AccentState GetAccentState(int style)
    {
        return style switch
        {
            1 => AccentState.ACCENT_ENABLE_TRANSPARENT,
            2 => AccentState.ACCENT_ENABLE_BLURBEHIND,
            3 => AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND,
            4 => AccentState.ACCENT_ENABLE_TRANSPARENTGRADIENT,
            5 => AccentState.ACCENT_ENABLE_GRADIENT,
            _ => AccentState.ACCENT_NORMAL,
        };
    }
    public static void CalculateRightPosition()
    {
        Intertop.SetProcessDpiAwareness(ProcessDpiAwareness.Process_Per_Monitor_DPI_Aware);
    }
    public static bool CheckIsWin11()
    {
        IntPtr childAfter =
            Intertop.FindWindowByClass(
                "Shell_TrayWnd",
                0);

        IntPtr Win11Taskbar =
            Intertop.FindWindowEx(
                childAfter,
                0,
                "Windows.UI.Composition.DesktopWindowContentBridge",
                null);

        return Win11Taskbar != 0;
    }

    public static List<nint> GetTaskbars(bool setLong = false)
    {
        List<IntPtr> windowHandles = GetActiveWindows();
        List<IntPtr> taskbars = [];

        foreach (IntPtr taskbar in windowHandles)
        {
            string sClassName = GetClassName(taskbar);
            IntPtr mSTaskListWClass = 0;


            if (sClassName == "Shell_TrayWnd")
            {
                IntPtr ReBarWindow32 = Intertop.FindWindowEx(
                    taskbar,
                    0,
                    "ReBarWindow32",
                    null);

                IntPtr MSTaskSwWClass = Intertop.FindWindowEx(
                    ReBarWindow32,
                    0,
                    "MSTaskSwWClass",
                    null);

                mSTaskListWClass = Intertop.FindWindowEx(
                    MSTaskSwWClass,
                    0,
                    "MSTaskListWClass",
                    null);
            }

            if (sClassName == "Shell_SecondaryTrayWnd")
            {
                IntPtr WorkerW = Intertop.FindWindowEx(
                    taskbar,
                    0,
                    "WorkerW",
                    null);

                mSTaskListWClass = Intertop.FindWindowEx(
                    WorkerW,
                    0,
                    "MSTaskListWClass",
                    null);
            }

            if (setLong) SetHwndlong(taskbar);

            taskbars.Add(mSTaskListWClass);
        }

        return taskbars;
    }

    private static void SetHwndlong(nint taskbar)
    {
        Intertop.SetWindowLong(
                        taskbar,
                        (WindowStyles)Win32.Intertop.GWL_EXSTYLE,
                        0x80);
    }

    public static List<IntPtr> GetActiveWindows()
    {
        List<IntPtr> windowHandles = [];

        Intertop.EnumWindows(delegate (IntPtr hwnd, IntPtr lParam)
        {
            string sClassName = GetClassName(hwnd);
            if (sClassName == "Shell_TrayWnd" || 
                sClassName == "Shell_SecondaryTrayWnd")
            {
                windowHandles.Add(hwnd);
            }
            return true;
        }, 0);

        bool maintaskbarfound = false;
        bool sectaskbarfound = false;

        foreach (IntPtr taskbar in windowHandles)
        {
            string sClassName = GetClassName(taskbar);

            if (sClassName == "Shell_TrayWnd")
            {
                maintaskbarfound = true;
            }
            if (sClassName == "Shell_SecondaryTrayWnd")
            {
                sectaskbarfound = true;
            }

            Console.WriteLine("=" + maintaskbarfound);
        }

        try
        {
            string lpClassName = null;
            if (!maintaskbarfound)
            {
                lpClassName = "Shell_TrayWnd";
            }
            if (!sectaskbarfound && Screen.AllScreens.Length >= 2)
            {
                lpClassName = "Shell_SecondaryTrayWnd";
            }
            if (lpClassName != null)
            {
                nint handle = Intertop.FindWindow(lpClassName, null);
                windowHandles.Add(handle);
            }
        }
        catch { }
        return windowHandles;
    }

    public static string GetClassName(IntPtr hwnd)
    {
        StringBuilder lpClassName = new StringBuilder("", 256);
        Intertop.GetClassName(hwnd, lpClassName, 256);
        return lpClassName.ToString();
    }

    public static void SetDeafaultStyle(IntPtr trayptr)
    {
        Intertop.SendMessage(
                        trayptr,
                        Intertop.WM_THEMECHANGED,
                        true,
                        0);
        Intertop.SendMessage(
            trayptr,
            Intertop.WM_DWMCOLORIZATIONCOLORCHANGED,
            true,
            0);
        Intertop.SendMessage(
            trayptr,
            Intertop.WM_DWMCOMPOSITIONCHANGED,
            true,
            0);

        Rect tt = new Rect();
        Intertop.GetClientRect(
            trayptr,
            ref tt);

        Intertop.SetWindowRgn(
            trayptr,
            Intertop.CreateRectRgn(
                tt.Left,
                tt.Top,
                tt.Right,
                tt.Bottom),
            true);
    }

    public static void SetDefaultPosition(IntPtr taskbar)
    {
        Intertop.GetParent(taskbar);
        Intertop.SendMessage(
            Intertop.GetParent(
                Intertop.GetParent(taskbar)),
                11,
                true,
                0);

        SetHwndPosition(
            taskbar,
            0,
            0);
    }

    public static void SetHwndPosition(nint hwnd, int x, int y)
    {
        Intertop.SetWindowPos(
            hwnd,
            nint.Zero,
            x,
            y,
            0,
            0,
            Intertop.SWP_NOSIZE |
            Intertop.SWP_ASYNCWINDOWPOS |
            Intertop.SWP_NOACTIVATE |
            Intertop.SWP_NOZORDER |
            Intertop.SWP_NOSENDCHANGING);
    }
}

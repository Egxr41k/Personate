using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

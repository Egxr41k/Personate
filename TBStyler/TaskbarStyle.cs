using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TBStyler;

public class TaskbarStyle
{
    public delegate bool CallBack(IntPtr hwnd, int lParam);

    [DllImport("user32")]
    public static extern int EnumWindows(CallBack cb, int lParam);

    public static System.Collections.ObjectModel.Collection<IntPtr> ActiveWindows = new System.Collections.ObjectModel.Collection<IntPtr>();

    public static System.Collections.ObjectModel.Collection<IntPtr> GetActiveWindows()
    {
        windowHandles.Clear();
        EnumWindows(new CallBack(Enumerator), 0);

        bool maintaskbarfound = false;
        bool sectaskbarfound = false;

        foreach (IntPtr taskbar in windowHandles)
        {
            StringBuilder sClassName = new StringBuilder("", 256);
            Win32.Intertop.GetClassName(taskbar, sClassName, 256);
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
                windowHandles.Add(Win32.Intertop.FindWindow("Shell_TrayWnd", ""));
            }
            catch
            {
            }
        }

        if (!sectaskbarfound)
        {
            if (Screen.AllScreens.Length >= 2)
            {
                try
                {
                    windowHandles.Add(Win32.Intertop.FindWindow("Shell_SecondaryTrayWnd", ""));
                }
                catch
                {
                }
            }
        }

        return ActiveWindows;
    }

    public static System.Collections.ArrayList windowHandles = new System.Collections.ArrayList();
    public static System.Collections.ArrayList maximizedwindows = new System.Collections.ArrayList();
    public static System.Collections.ArrayList trays = new System.Collections.ArrayList();
    public static System.Collections.ArrayList traysbackup = new System.Collections.ArrayList();
    public static System.Collections.ArrayList normalwindows = new System.Collections.ArrayList();
    public static System.Collections.ArrayList resetted = new System.Collections.ArrayList();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowPlacement(IntPtr hWnd, ref Win32.Types.WindowPlacement lpwndpl);

    [DllImport("user32.dll")]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("dwmapi.dll")]
    public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out int pvAttribute, int cbAttribute);

    public static bool IsPhanthom(IntPtr hWnd)
    {
        int CloakedVal;
        int hRes = DwmGetWindowAttribute(hWnd, (int)Win32.Types.DwmWindowAttribute.Cloaked, out CloakedVal, Marshal.SizeOf(typeof(int)));
        if (hRes != 0)
        {
            CloakedVal = 0;
        }
        return (CloakedVal != 0);
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

    public static bool Enumerator2(IntPtr hwnd, int lParam)
    {
        try
        {
            int intRet;
            Win32.Types.WindowPlacement wpTemp = new Win32.Types.WindowPlacement();
            wpTemp.Length = Marshal.SizeOf(wpTemp);
            intRet = Convert.ToInt32(Win32.Intertop.GetWindowPlacement(hwnd, ref wpTemp));
            int style = Win32.Intertop.GetWindowLong(hwnd, Win32.Intertop.GWL_STYLE);

            if (!IsPhanthom(hwnd)) //Fix phanthom windows
            {
                if ((style & (int)Win32.Types.WindowStyles.WS_VISIBLE) == (int)Win32.Types.WindowStyles.WS_VISIBLE)
                {
                    if (wpTemp.showCmd == 3)
                    {
                        maximizedwindows.Remove(hwnd);
                        maximizedwindows.Add(hwnd);
                    }
                    else
                    {
                        normalwindows.Remove(hwnd);
                        normalwindows.Add(hwnd);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
        return true;
    }

    public static void Tbsm()
    {
        do
        {
            int windowsold;
            int windowsnew;
            windowsold = maximizedwindows.Count;

            maximizedwindows.Clear();
            System.Threading.Thread.Sleep(250);
            EnumWindows(Enumerator2, 0);

            windowsnew = maximizedwindows.Count;

            if (windowsnew != windowsold)
            {
                foreach (IntPtr tray in traysbackup)
                {
                    foreach (IntPtr normalwindow in normalwindows)
                    {
                        Screen curmonx = Screen.FromHandle(normalwindow);
                        Screen curmontbx = Screen.FromHandle(tray);
                        if (curmonx.DeviceName == curmontbx.DeviceName)
                        {
                            trays.Remove(tray);
                            trays.Add(tray);
                        }
                    }
                }

                foreach (IntPtr tray in traysbackup)
                {
                    foreach (IntPtr maxedwindow in maximizedwindows)
                    {
                        Screen curmonx = Screen.FromHandle(maxedwindow);
                        Screen curmontbx = Screen.FromHandle(tray);
                        if (curmonx.DeviceName == curmontbx.DeviceName)
                        {
                            trays.Remove(tray);
                            Win32.Intertop.PostMessage(tray, 0x31E, (IntPtr)0x1, (IntPtr)0x0);
                        }
                    }
                }
            }
        } while (true);
    }

    public static void TaskbarStyler()
    {
        try
        {
            GetActiveWindows();

            Win32.Types.AccentPolicy accent = new Win32.Types.AccentPolicy();
            int accentStructSize = Marshal.SizeOf(accent);

            // Select accent based on settings
            if (Settings.TaskbarStyle == 1)
            {
                accent.AccentState = Win32.Types.AccentState.ACCENT_ENABLE_TRANSPARENT;
            }

            if (Settings.TaskbarStyle == 2)
            {
                accent.AccentState = Win32.Types.AccentState.ACCENT_ENABLE_BLURBEHIND;
            }

            if (Settings.TaskbarStyle == 3)
            {
                accent.AccentState = Win32.Types.AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND;
            }

            if (Settings.TaskbarStyle == 4)
            {
                accent.AccentState = Win32.Types.AccentState.ACCENT_ENABLE_TRANSPARENTGRADIENT;
            }

            if (Settings.TaskbarStyle == 5)
            {
                accent.AccentState = Win32.Types.AccentState.ACCENT_ENABLE_GRADIENT;
            }

            accent.AccentFlags = 2; // enable colorize
            accent.GradientColor = BitConverter.ToInt32(new byte[] { (byte)Settings.TaskbarStyleRed, (byte)Settings.TaskbarStyleGreen, (byte)Settings.TaskbarStyleBlue, (byte)(Settings.TaskbarStyleAlpha * 2.55) }, 0);

            // Save accent data
            IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            Win32.Types.WindowCompositionAttributeData data = new Win32.Types.WindowCompositionAttributeData
            {
                Attribute = Win32.Intertop.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            // Put all TrayWnds into an ArrayList
            foreach (IntPtr trayWnd in windowHandles)
            {
                trays.Add(trayWnd);
                traysbackup.Add(trayWnd);
            }

            if (Settings.DefaultTaskbarStyleOnWinMax == 1)
            {
                Thread t2 = new Thread(new ThreadStart(Tbsm));
                t2.Start();
            }

            // Set taskbar style for all TrayWnds each 14 milliseconds
            foreach (IntPtr tray in trays)
            {
                IntPtr trayptr = (IntPtr)Convert.ToInt32(tray.ToString());
                Win32.Intertop.SetWindowCompositionAttribute(trayptr, ref data);
            }

            do
            {
                try
                {
                    foreach (IntPtr tray in trays)
                    {
                        Win32.Intertop.SetWindowCompositionAttribute(tray, ref data);
                    }
                    System.Threading.Thread.Sleep(10);
                }
                catch
                {
                }
            } while (true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static int childLeft;
    public static int childTop;
    public static int childWidth;
    public static int childHeight;

    public static int GetLocation(Accessibility.IAccessible acc, int idChild)
    {
        acc.accLocation(out childLeft, out childTop, out childWidth, out childHeight, idChild);
        return 0;
    }
}

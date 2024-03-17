using System.Collections;
using System.Runtime.InteropServices;
using Personate.Settings;
using TBStyler.Win32.Types;


namespace TBStyler;

public class Styler
{
    private ArrayList maximizedwindows { get; set; } = [];
    private ArrayList trays { get; set; } = [];
    private ArrayList traysbackup { get; set; } = [];
    private ArrayList normalwindows { get; set; } = [];

    private TaskbarSettingsDTO settings { get; set; }
    public Styler(TaskbarSettingsDTO settings)
    {
        this.settings = settings;
    }

    public void ResetStyles()
    {
        List<IntPtr> windowHandles = Win32.Api.GetActiveWindows();

        foreach (IntPtr trayptr in windowHandles)
        {
            Win32.Api.SetDeafaultStyle(trayptr);
        }
    }

    public void StartIfEnable()
    {
        if (CheckIsEnable())
        {
            Task.Run(() =>
            {
                SetStyles();
            }, Program.Cancellation.Token);
        } // else ResetStyles();
    }

    private bool CheckIsEnable()
    {
        return settings.TaskbarStyle >= 1 && settings.TaskbarStyle <= 5;
    }

    private void SetStyles()
    {
        try
        {
            List<nint> windowHandles = Win32.Api.GetActiveWindows();

            Win32.Types.AccentPolicy accent = new Win32.Types.AccentPolicy();
            int accentStructSize = Marshal.SizeOf(accent);

            // Select accent based on settings
            accent.AccentState = Win32.Api.GetAccentState(settings.TaskbarStyle);

            accent.AccentFlags = 2; // enable colorize
            accent.GradientColor = GetColor();

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

            if (settings.DefaultTaskbarStyleOnWinMax == 1)
            {
                Task.Run(() =>
                {
                    Tbsm();
                }, Program.Cancellation.Token);
            }

            // Set taskbar style for all TrayWnds each 14 milliseconds
            foreach (IntPtr tray in trays)
            {
                IntPtr trayptr = (IntPtr)Convert.ToInt32(tray.ToString());
                Win32.Intertop.SetWindowCompositionAttribute(trayptr, ref data);
            }


            Task.Run(() =>
            {
                Looper(data);
            }, Program.Cancellation.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private int GetColor()
    {
        return BitConverter.ToInt32([
            (byte)settings.TaskbarStyleRed,
            (byte)settings.TaskbarStyleGreen,
            (byte)settings.TaskbarStyleBlue,
            (byte)(settings.TaskbarStyleAlpha * 2.55)
        ], 0);
    }

    private void Looper(Win32.Types.WindowCompositionAttributeData data)
    {
        while (Program.IsntCancel)
        {
            foreach (IntPtr tray in trays)
            {
                Win32.Intertop.SetWindowCompositionAttribute(tray, ref data);
            }
            Task.Delay(10);
        }
    }

    private void Tbsm()
    {
        while (Program.IsntCancel)
        {
            int windowsold;
            int windowsnew;
            windowsold = maximizedwindows.Count;

            maximizedwindows.Clear();
            Task.Delay(250);
            Win32.Intertop.EnumWindows(Enumerator2, 0);

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
                            Win32.Intertop.PostMessage(
                                tray, 
                                0x31E,
                                0x1,
                                0x0);
                        }
                    }
                }
            }
        }
    }

    private bool Enumerator2(IntPtr hwnd, IntPtr lParam)
    {
        int intRet;
        Win32.Types.WindowPlacement wpTemp = new Win32.Types.WindowPlacement();
        wpTemp.Length = Marshal.SizeOf(wpTemp);
        intRet = Convert.ToInt32(Win32.Intertop.GetWindowPlacement(hwnd, ref wpTemp));
        int style = Win32.Intertop.GetWindowLong(hwnd, Win32.Intertop.GWL_STYLE);

        if (!IsPhanthom(hwnd))
        {
            FixPhanthomWindows(hwnd, wpTemp, style);
        }

        return true;
    }

    private bool IsPhanthom(IntPtr hWnd)
    {
        int CloakedVal = 0;
        int hRes = Win32.Intertop.DwmGetWindowAttribute(
            hWnd,
            Win32.Types.DwmWindowAttribute.Cloaked,
            ref CloakedVal,
            Marshal.SizeOf(typeof(int)));

        if (hRes != 0)
        {
            CloakedVal = 0;
        }
        return (CloakedVal != 0);
    }

    private void FixPhanthomWindows(nint hwnd, Win32.Types.WindowPlacement wpTemp, int style)
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
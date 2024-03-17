using System.Collections;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Win32;
using Accessibility;
using TBStyler.Win32;
using TBStyler.Win32.Types;
using TBStyler.MSAA;
using Personate.Settings;

namespace TBStyler;

public class Centerer
{
    private UserPreferenceChangedEventHandler UserPref;
    private Position position { get; set; }
    private TrayLoopFixer trayLoopFixer { get; set; }
    private TaskbarSettingsDTO settings { get; set; }

    public Centerer(TaskbarSettingsDTO settings)
    {
        this.settings = settings;
        position = new Position(settings);
        trayLoopFixer = new TrayLoopFixer();

        UserPref = new UserPreferenceChangedEventHandler(PrefChange);
        SystemEvents.DisplaySettingsChanged += DPChange;
        SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
    }

    public void StartIfEnable()
    {
        if (CheckIsEnable())
        {
            Task.Run(() =>
            {
                SetCenter();
            }, Program.Cancellation.Token);
        } else position.Restore();
    }

    private bool CheckIsEnable()
    {
        return settings.DontCenterTaskbar != 1;
    }

    public void RestorePosition()
    {
        position.Restore();
    }

    private void SetCenter()
    {
        position.Restore();


        // Start the Looper
        Task.Run(() =>
        {
            Looper();
        }, Program.Cancellation.Token);

        // Start the TrayLoopFix
        if (settings.FixToolbarsOnTrayChange == 1)
        {
            Task.Run(() =>
            {
                trayLoopFixer.Fix();
            }, Program.Cancellation.Token);
        }
    }

    #region Events

    private void PrefChange(object sender, UserPreferenceChangedEventArgs e)
    {
        if (e.Category == UserPreferenceCategory.General)
        {
            WaitingForShellTrayWnd();
        }
    }

    private void DPChange(object? sender, EventArgs e)
    {
        WaitingForShellTrayWnd();
    }

    private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
    {
        WaitingForShellTrayWnd();
    }

    #endregion

    private void Looper()
    {
        // This loop will check if the taskbar changes and requires a move
        try
        {
            List<IntPtr> taskbars = Win32.Api.GetTaskbars();
            ConfigureTaskbars();

            foreach (IntPtr taskbar in taskbars)
            {
                HandleZero(taskbar);
            }

            var taskObjects = new List<Accessibility.IAccessible>();
            foreach (var taskList in taskbars)
            {
                var accessiblex = MSAA.Api.GetAccessibleFromHandle(taskList);
                taskObjects.Add(accessiblex);
            }


            // Start the endless loop
            string oldresults = "";
            string results = "";
            bool initposcalcready;

            while (Program.IsntCancel)
            {
                try
                {
                    if (Screen.PrimaryScreen != null &&
                        Screen.PrimaryScreen.Bounds.Width != 0 &&
                        (Screen.PrimaryScreen.Bounds.Width ==
                        settings.SkipResolution ||
                        Screen.PrimaryScreen.Bounds.Width ==
                        settings.SkipResolution2 ||
                        Screen.PrimaryScreen.Bounds.Width ==
                        settings.SkipResolution3
                        ))
                    {
                        position.Restore();
                    }

                    if (settings.CheckFullscreenApp == 1)
                    {
                        var activeWindow = Win32.Intertop.GetForegroundWindow();
                        var curMonX = Screen.FromHandle(activeWindow);
                        var activeWindowSize = new Rect();
                        Win32.Intertop.GetWindowRect(activeWindow, ref activeWindowSize);

                        if (activeWindowSize.Top == curMonX.Bounds.Top &&
                            activeWindowSize.Bottom == curMonX.Bounds.Bottom &&
                            activeWindowSize.Left == curMonX.Bounds.Left &&
                            activeWindowSize.Right == curMonX.Bounds.Right)
                        {
                            Console.WriteLine(
                                "Fullscreen App detected " +
                                activeWindowSize.Bottom + "," +
                                activeWindowSize.Top + "," +
                                activeWindowSize.Left + "," +
                                activeWindowSize.Right);

                            settings.Pause = true;
                            do
                            {
                                Task.Delay(500);
                                activeWindow = Win32.Intertop.GetForegroundWindow();
                                Win32.Intertop.GetWindowRect(activeWindow, ref activeWindowSize);
                                Task.Delay(500);

                            } while (activeWindowSize.Top == curMonX.Bounds.Top &&
                                     activeWindowSize.Bottom == curMonX.Bounds.Bottom &&
                                     activeWindowSize.Left == curMonX.Bounds.Left &&
                                     activeWindowSize.Right == curMonX.Bounds.Right);

                            Console.WriteLine("Fullscreen App deactivated");

                            settings.Pause = false;
                        }
                    }

                    foreach (var taskList in taskObjects)
                    {
                        IAccessible[] children = MSAA.Api.GetAccessibleChildren(taskList);
                        RectangleX taskListPos = MSAA.Api.GetLocation(taskList, 0);
                        int tH = taskListPos.height;
                        int tW = taskListPos.width;

                        var lastChildPos = new MSAA.RectangleX();
                        foreach (var childx in children)
                        {
                            IntPtr toolbar = 0x16;
                            if ((int)childx.accRole[0] == (int)toolbar)
                            {
                                IAccessible[] childrensx = MSAA.Api.GetAccessibleChildren(childx);
                                lastChildPos = MSAA.Api.GetLocation(
                                    childx,
                                    childrensx.Length);
                                break;
                            }
                        }

                        int cL = lastChildPos.left;
                        int cT = lastChildPos.top;
                        int cW = lastChildPos.width;
                        int cH = lastChildPos.height;

                        try
                        {
                            int testIfError = cL;
                        }
                        catch (Exception)
                        {
                            // Current taskbar is empty go to the next taskbar.
                            continue;
                        }

                        int taskbarCount;
                        int trayWndSize;

                        // Get the current taskbar orientation (H = Horizontal | V = Vertical)
                        settings.HorizontalOrientation = !(tH >= tW);


                        // Get the end position of the last icon in the taskbar
                        taskbarCount = settings.HorizontalOrientation ? cL + cW : cT + cH;

                        // Get the width of the whole taskbar's placeholder
                        trayWndSize = settings.HorizontalOrientation ? tW : tH;

                        // Put the results into a string ready to be matched for differences with the last loop
                        results = (settings.HorizontalOrientation ? "H" : "V") + taskbarCount + trayWndSize;

                        initposcalcready = true;
                    }

                    if (results != oldresults)
                    {
                        // Something has changed; we can now calculate the new position for each taskbar
                        initposcalcready = false;

                        // Start the PositionCalculator
                        Task.Run(() =>
                        {
                            while (!initposcalcready) Task.Delay(10);
                            bool noChanges = oldresults == results;

                            if (noChanges)
                            {
                                Task.Run(() =>
                                {
                                    position.Calculate();
                                }, Program.Cancellation.Token);
                            }
                        }, Program.Cancellation.Token);
                    }

                    // Save current results for the next loop
                    oldresults = results;

                    if (SystemInformation.PowerStatus.PowerLineStatus ==
                        PowerLineStatus.Offline)
                    {
                        Task.Delay(settings.OnBatteryLoopRefreshRate);
                    }
                    else
                    {
                        Task.Delay(settings.LoopRefreshRate);
                    }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("@Looper1 | " + ex.Message);
                    WaitingForShellTrayWnd();
                }
                catch (MissingMethodException ex)
                {
                    Console.WriteLine("@Looper1 | " + ex.Message);
                    WaitingForShellTrayWnd();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("@Looper2 | " + ex.Message);
        }
    }

    private void ConfigureTaskbars()
    {
        Win32.Api.GetActiveWindows().ForEach(taskbar =>
        {
            ConfigurePrimary(taskbar);
            ConfigureSecondary(taskbar);
        });
    }

    private static void HandleZero(nint mSTaskListWClass)
    {
        if (mSTaskListWClass == 0)
        {
            Console.WriteLine("TaskbarX: " +
                "Could not find the handle of the taskbar. " +
                "Your current version or build of Windows may not be supported.");
            Environment.Exit(0);
        }
    }

    private void ConfigureSecondary(nint taskbar)
    {
        if (settings.TotalSecondaryOpacity != 0)
        {
            Win32.Intertop.SetWindowLong(
                taskbar,
                (WindowStyles)Win32.Intertop.GWL_EXSTYLE,
                0x80000);

            Win32.Intertop.SetLayeredWindowAttributes(
                taskbar,
                0,
                (byte)(255 / 100 * (byte)settings.TotalSecondaryOpacity),
                0x2);
        }

        if (settings.HideSecondaryStartButton == 1)
        {
            var sStart = Win32.Intertop.FindWindowEx(
                taskbar,
                0,
                "Start",
                null);

            Win32.Intertop.ShowWindow(
                sStart,
                ShowWindowCommands.Hide);

            Win32.Intertop.SetLayeredWindowAttributes(
                sStart,
                0,
                0,
                0x2);
        }

        if (settings.HideSecondaryNotifyWnd == 1)
        {
            var sTray = Win32.Intertop.FindWindowEx(
                taskbar,
                0,
                "ClockButton",
                null);

            Win32.Intertop.ShowWindow(
                sTray,
                ShowWindowCommands.Hide);

            Win32.Intertop.SetLayeredWindowAttributes(
                sTray,
                0,
                0,
                0x2);
        }
    }

    private void ConfigurePrimary(nint taskbar)
    {
        if (settings.TotalPrimaryOpacity != 0)
        {
            Win32.Intertop.SetWindowLong(
                taskbar,
                (WindowStyles)Win32.Intertop.GWL_EXSTYLE,
                0x80000);

            Win32.Intertop.SetLayeredWindowAttributes(
                taskbar,
                0,
                Convert.ToByte(255 / 100 * settings.TotalPrimaryOpacity),
                0x2);
        }

        if (settings.HidePrimaryStartButton == 1)
        {
            var mStart = Win32.Intertop.FindWindowEx(
                taskbar,
                0,
                "Start",
                null);

            Win32.Intertop.ShowWindow(
                mStart,
                ShowWindowCommands.Hide);

            Win32.Intertop.SetLayeredWindowAttributes(
                mStart,
                0,
                0,
                0x2);
        }

        if (settings.HidePrimaryNotifyWnd == 1)
        {
            var mTray = Win32.Intertop.FindWindowEx(
                taskbar,
                0,
                "TrayNotifyWnd",
                null);

            Win32.Intertop.ShowWindow(
                mTray,
                ShowWindowCommands.Hide);

            Win32.Intertop.SetWindowLong(
                mTray,
                (WindowStyles)Win32.Intertop.GWL_STYLE,
                0x7E000000);

            Win32.Intertop.SetWindowLong(
                mTray,
                (WindowStyles)Win32.Intertop.GWL_EXSTYLE,
                0x80000);

            Win32.Intertop.SendMessage(
                mTray,
                11,
                false,
                0);

            Win32.Intertop.SetLayeredWindowAttributes(
                mTray,
                0,
                0,
                0x2);
        }
    }

    private void WaitingForShellTrayWnd()
    {
        Console.WriteLine();
        Task.Delay(1000);

        IntPtr Handle = 0;
        while(Handle == 0)
        {
            Console.WriteLine("Waiting for Shell_TrayWnd");
            Task.Delay(250);
            Handle = Win32.Intertop.FindWindowByClass("Shell_TrayWnd", 0);
        };

        Application.Restart();
    }
}
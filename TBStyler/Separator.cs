using Personate.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBStyler.MSAA;
using TBStyler.Win32.Types;

namespace TBStyler;

internal class Separator
{
    private TaskbarSettingsDTO settings { get; set; }
    public Separator(TaskbarSettingsDTO settings)
    {
        this.settings = settings;
    }

    public void SeparateToSegments(
        nint TaskList, 
        int TaskbarWidth, 
        int TaskbarLeft, 
        int newPosition, 
        RectangleX TrayNotifyPos, 
        nint TrayWndHandle, 
        string TrayWndClassName, 
        RectangleX TrayWndPos)
    {
        if (settings.TaskbarSegments == 1)
        {
            if (settings.HorizontalOrientation)
            {
                Rect ttseg = new Rect();
                Win32.Intertop.GetClientRect(TaskList, ref ttseg);

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
                    TaskbarLeft + newPosition + 4,
                    ttseg.Top,
                    TaskbarLeft + newPosition + TaskbarWidth - 2,
                    ttseg.Bottom + 1,
                    settings.TaskbarRounding,
                    settings.TaskbarRounding);

                IntPtr NotifyTray_rgn = Win32.Intertop.CreateRoundRectRgn(
                    TrayNotifyPos.left,
                    0,
                    TrayNotifyPos.left + TrayNotifyPos.width,
                    TrayNotifyPos.top + TrayNotifyPos.height,
                    settings.TaskbarRounding,
                    settings.TaskbarRounding);

                IntPtr Start_rgn = Win32.Intertop.CreateRoundRectRgn(
                    startseg.Left,
                    0,
                    startseg.Right,
                    startseg.Bottom,
                    settings.TaskbarRounding,
                    settings.TaskbarRounding);

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
                    settings.TaskbarRounding,
                    settings.TaskbarRounding);


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

                if (TrayWndClassName == "Shell_TrayWnd")
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
                Win32.Intertop.GetClientRect(TaskList, ref ttseg);
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
                    TaskbarLeft + newPosition + 4,
                    ttseg.Right,
                    TaskbarLeft + newPosition + TaskbarWidth - 2,
                    settings.TaskbarRounding,
                    settings.TaskbarRounding);

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
            if (settings.TaskbarRounding != 0)
            {
                Win32.Intertop.SetWindowRgn(
                    TrayWndHandle,
                    Win32.Intertop.CreateRoundRectRgn(
                        0,
                        0,
                        TrayWndPos.width,
                        TrayWndPos.height,
                        settings.TaskbarRounding,
                        settings.TaskbarRounding), true);
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

        if (settings.TaskbarSegments == 1)
        {
            if (settings.TaskbarStyle != 0)
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
    }
}

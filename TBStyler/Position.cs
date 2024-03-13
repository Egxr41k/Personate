using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TBStyler.MSAA;
using TBStyler.Win32;
using TBStyler.Win32.Types;
using Accessibility;
using Personate.Settings;

namespace TBStyler;

internal class Position
{
    private TaskbarSettingsDTO settings { get; set; }
    private Animator animator { get; set; }

    public Position(TaskbarSettingsDTO settings)
    {
        this.settings = settings;
        animator = new Animator(settings);
    }

    public void Calculate()
    {
        // Calculate the new positions and pass them through to the animator

        List<IntPtr> windowHandles = Win32.Api.GetActiveWindows();
        List<IntPtr> taskbars = [];
        // Put all Taskbars into an ArrayList based on each TrayWnd in the TrayWnds ArrayList
        foreach (IntPtr taskbar in windowHandles)
        {
            string sClassName = Win32.Api.GetClassName(taskbar);
            IntPtr mSTaskListWClass = 0;


            if (sClassName == "Shell_TrayWnd")
            {
                IntPtr ReBarWindow32 = Win32.Intertop.FindWindowEx(
                    taskbar, 
                    0, 
                    "ReBarWindow32",
                    null);

                IntPtr MSTaskSwWClass = Win32.Intertop.FindWindowEx(
                    ReBarWindow32, 
                    0, 
                    "MSTaskSwWClass",
                    null);

                mSTaskListWClass = Win32.Intertop.FindWindowEx(
                    MSTaskSwWClass, 
                    0, 
                    "MSTaskListWClass",
                    null);
            }

            if (sClassName == "Shell_SecondaryTrayWnd")
            {
                IntPtr WorkerW = Win32.Intertop.FindWindowEx(
                    taskbar, 
                    0, 
                    "WorkerW",
                    null);

                mSTaskListWClass = Win32.Intertop.FindWindowEx(
                    WorkerW, 
                    0, 
                    "MSTaskListWClass",
                    null);
            }

            Win32.Intertop.SetWindowLong(
                taskbar, 
                (WindowStyles)Win32.Intertop.GWL_EXSTYLE, 
                0x80);

            if (mSTaskListWClass == default)
            {
                Console.WriteLine("TaskbarX: " +
                    "Could not find the handle of the taskbar. " +
                    "Restarting...");

                Task.Delay(1000);
                Application.Restart(); 
            }

            taskbars.Add(mSTaskListWClass);
        }

        // Calculate Position for every taskbar and trigger the animator
        foreach (var TaskList in taskbars)
        {
            string sClassName = Win32.Api.GetClassName(TaskList);

            RectangleX LastChildPos = new RectangleX();
            RectangleX TaskListPos = new RectangleX();

            IAccessible accessible = MSAA.Api.GetAccessibleFromHandle(TaskList);
            IAccessible[] children = MSAA.Api.GetAccessibleChildren(accessible);

            TaskListPos = MSAA.Api.GetLocation(accessible, 0);

            foreach (IAccessible childx in children)
            {
                if (Convert.ToInt32(childx.accRole[0]) == Convert.ToInt32(22))
                {
                    IAccessible[] childrenx = MSAA.Api.GetAccessibleChildren(childx);
                    LastChildPos = MSAA.Api.GetLocation(
                        childx,
                        childrenx.Length);
                    break;
                }
            }

            var RebarHandle = Win32.Intertop.GetParent(TaskList);
            IAccessible accessible3 = MSAA.Api.GetAccessibleFromHandle(RebarHandle);

            string RebarClassName = Win32.Api.GetClassName(RebarHandle);

            int TaskbarWidth;
            int TrayWndLeft;
            int TrayWndWidth;
            int RebarWndLeft;
            int TaskbarLeft;
            int newPosition;
            int oldPosition;
            int curleft;
            int curleft2;

            RectangleX TrayNotifyPos = new RectangleX();
            RectangleX NewsAndInterestsPos = new RectangleX();
            IntPtr NewsAndInterestsHandle;

            IntPtr TaskListParent = Win32.Intertop.GetParent(TaskList);
            IntPtr TrayWndHandle = Win32.Intertop.GetParent(TaskListParent);




            string TrayWndClassName = Win32.Api.GetClassName(TrayWndHandle);

            // Check if TrayWnd = wrong. if it is, correct it
            // (This will be the primary taskbar which should be Shell_TrayWnd)
            if (TrayWndClassName == "ReBarWindow32")
            {
                Win32.Intertop.SendMessage(
                    TrayWndHandle,
                    11,
                    false,
                    0);

                TaskListParent = Win32.Intertop.GetParent(TaskList);
                IntPtr TaskListGrandParent = Win32.Intertop.GetParent(TaskListParent);
                TrayWndHandle = Win32.Intertop.GetParent(TaskListGrandParent);

                IntPtr TrayNotify = Win32.Intertop.FindWindowEx(
                    TrayWndHandle,
                    0,
                    "TrayNotifyWnd",
                    null);

                IAccessible accessible4 = MSAA.Api.GetAccessibleFromHandle(TrayNotify);
                TrayNotifyPos = MSAA.Api.GetLocation(accessible4, 0);



                IntPtr NewsAndInterests = Win32.Intertop.FindWindowEx(
                    TrayWndHandle,
                    0, 
                    "DynamicContent1",
                    null);

                if (NewsAndInterests != 0)
                {
                    NewsAndInterestsHandle = NewsAndInterests;
                    IAccessible accessible5 = MSAA.Api.GetAccessibleFromHandle(NewsAndInterests);
                    NewsAndInterestsPos = MSAA.Api.GetLocation(accessible5, 0);
                }

                IntPtr TrayWndParentHandle = Win32.Intertop.GetParent(TrayWndHandle);
                Win32.Intertop.SendMessage(
                    TrayWndParentHandle,
                    11,
                    false,
                    0);
            }



            TrayWndClassName = Win32.Api.GetClassName(TrayWndHandle);
            IAccessible accessible2 = MSAA.Api.GetAccessibleFromHandle(TrayWndHandle);

            RectangleX TrayWndPos = MSAA.Api.GetLocation(accessible2, 0);
            RectangleX RebarPos = MSAA.Api.GetLocation(accessible3, 0);

            // If the taskbar is still moving then wait until it's not
            // (This will prevent unneeded calculations that trigger the animator)
            do
            {
                curleft = TaskListPos.left;
                TaskListPos = MSAA.Api.GetLocation(accessible, 0);
                // ' TaskListcL = childLeft2
                Task.Delay(30);
                curleft2 = TaskListPos.left;
            }
            while (curleft != curleft2);

            // Get current taskbar orientation (H = Horizontal | V = Vertical)
            settings.HorizontalOrientation = !(TaskListPos.height >= TaskListPos.width);

            // Calculate the exact width of the total icons
            TaskbarWidth = CalculateWidth(LastChildPos, TaskListPos);
            
            bool isCenterPrimary =
                    settings.CenterPrimaryOnly == 1 &&
                    TrayWndClassName == "Shell_TrayWnd";

            oldPosition = CalculateOldPosition(TaskListPos, RebarPos);


            // Get info needed to calculate the position
            if (settings.HorizontalOrientation)
            {
                TrayWndLeft =  Math.Abs(TrayWndPos.left);
                TrayWndWidth = Math.Abs(TrayWndPos.width);
                RebarWndLeft = Math.Abs(RebarPos.left);
                TaskbarLeft =  Math.Abs(RebarWndLeft - TrayWndLeft);
            }
            else
            {
                TrayWndLeft =  Math.Abs(TrayWndPos.top);
                TrayWndWidth = Math.Abs(TrayWndPos.height);
                RebarWndLeft = Math.Abs(RebarPos.top);
                TaskbarLeft =  Math.Abs(RebarWndLeft - TrayWndLeft);
            }

            Console.WriteLine("!" + NewsAndInterestsPos.width);

            // Calculate new position
            if (TrayWndClassName.ToString() == "Shell_TrayWnd")
            {
                if (settings.CenterInBetween == 1)
                {
                    if (settings.HorizontalOrientation)
                    {
                        var offset = (TrayNotifyPos.width / (double)2 - (TaskbarLeft / 2)) + NewsAndInterestsPos.width / (double)2;
                        newPosition = Math.Abs(Convert.ToInt32((TrayWndWidth / (double)2 - (TaskbarWidth / (double)2) - TaskbarLeft - offset))) + settings.PrimaryTaskbarOffset;
                    }
                    else
                    {
                        var offset = (TrayNotifyPos.height / (double)2 - (TaskbarLeft / 2)) + NewsAndInterestsPos.height / (double)2;
                        newPosition = Math.Abs(Convert.ToInt32((TrayWndWidth / (double)2 - (TaskbarWidth / (double)2) - TaskbarLeft - offset))) + settings.PrimaryTaskbarOffset;
                    }
                }
                else newPosition = Math.Abs(Convert.ToInt32(Convert.ToInt32((TrayWndWidth / (double)2) - (TaskbarWidth / (double)2) - TaskbarLeft))) + settings.PrimaryTaskbarOffset;
            }
            else newPosition = Math.Abs(Convert.ToInt32(Convert.ToInt32((TrayWndWidth / (double)2) - (TaskbarWidth / (double)2) - TaskbarLeft))) + settings.SecondaryTaskbarOffset;



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

            animator.StartIfEnable(TaskList, TaskbarWidth, oldPosition, newPosition);
        }
    }

    private int CalculateWidth(RectangleX LastChildPos, RectangleX TaskListPos)
    {
        if (settings.HorizontalOrientation)
            return LastChildPos.left - TaskListPos.left;
        else return LastChildPos.top - TaskListPos.top;
    }

    private int CalculateOldPosition(RectangleX taskListPos, RectangleX rebarPos)
    {
        //bool isCenterPrimary
        int leftOffset = taskListPos.left - rebarPos.left;
        int topOffset = taskListPos.top - rebarPos.top;

        int primaryOffset = settings.HorizontalOrientation ? leftOffset : topOffset;
        //int secondaryOffset = settings.IsHorizontalOrientation ? topOffset : leftOffset;

        //int offset = isCenterPrimary ? primaryOffset : secondaryOffset;

        return primaryOffset;
    }
    
    public void Restore()
    {
        PerformanceOptimizer.KillProcessByName("AcrylicPanel");
        // Put all taskbars back to default position
        List<nint> windowHandles = Win32.Api.GetActiveWindows();
        List<IntPtr> taskbars = [];

        foreach (IntPtr taskbar in windowHandles)
        {
            string sClassName = Win32.Api.GetClassName(taskbar);

            IntPtr MSTaskListWClass = 0;

            if (sClassName == "Shell_TrayWnd")
            {
                IntPtr ReBarWindow32 = Win32.Intertop.FindWindowEx(
                    taskbar, 
                    0, 
                    "ReBarWindow32",
                    null);

                IntPtr MStart = Win32.Intertop.FindWindowEx(
                    taskbar, 
                    0, 
                    "Start",
                    null);

                Win32.Intertop.ShowWindow(
                    MStart, 
                    ShowWindowCommands.Show);

                IntPtr MTray = Win32.Intertop.FindWindowEx(
                    taskbar, 
                    0, 
                    "TrayNotifyWnd",
                    null);

                Win32.Intertop.SetWindowLong(
                    MTray, 
                    (WindowStyles)Win32.Intertop.GWL_STYLE, 
                    0x56000000);

                Win32.Intertop.SetWindowLong(
                    MTray, 
                    (WindowStyles)Win32.Intertop.GWL_EXSTYLE, 
                    0x2000);

                Win32.Intertop.SendMessage(
                    MTray, 
                    11, 
                    true, 
                    0);

                Win32.Intertop.ShowWindow(
                    MTray, 
                    ShowWindowCommands.Show);

                IntPtr MSTaskSwWClass = Win32.Intertop.FindWindowEx(
                    ReBarWindow32, 
                    0, 
                    "MSTaskSwWClass",
                    null);

                MSTaskListWClass = Win32.Intertop.FindWindowEx(
                    MSTaskSwWClass, 
                    0, 
                    "MSTaskListWClass",
                    null);
            }

            if (sClassName == "Shell_SecondaryTrayWnd")
            {
                IntPtr WorkerW = Win32.Intertop.FindWindowEx(
                    taskbar, 
                    0, 
                    "WorkerW",
                    null);

                IntPtr SStart = Win32.Intertop.FindWindowEx(
                    taskbar, 
                    0, 
                    "Start",
                    null);

                Win32.Intertop.ShowWindow(
                    SStart, 
                    ShowWindowCommands.Show);

                IntPtr STray = Win32.Intertop.FindWindowEx(
                    taskbar, 
                    0, 
                    "ClockButton",
                    null);

                Win32.Intertop.ShowWindow(
                    STray, 
                    ShowWindowCommands.Show);

                MSTaskListWClass = Win32.Intertop.FindWindowEx(
                    WorkerW, 
                    0, 
                    "MSTaskListWClass",
                    null);
            }

            taskbars.Add(MSTaskListWClass);
        }

        foreach (IntPtr taskList in taskbars)
        {
            Win32.Intertop.GetParent(taskList);
            Win32.Intertop.SendMessage(
                Win32.Intertop.GetParent(
                    Win32.Intertop.GetParent(taskList)), 
                11, 
                true, 
                0);

            Win32.Api.SetHwndPosition(
                taskList, 
                0, 
                0);
        }
    }
}

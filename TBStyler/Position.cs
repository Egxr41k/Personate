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
    private Separator separator { get; set; }

    public Position(TaskbarSettingsDTO settings)
    {
        this.settings = settings;
        animator = new Animator(settings);
        separator = new Separator(settings);
    }

    public void Calculate()
    {
        // Calculate the new positions and pass them through to the animator
        List<IntPtr> taskbars = Win32.Api.GetTaskbars(true);

        // Calculate Position for every taskbar and trigger the animator
        foreach (IntPtr TaskList in taskbars)
        {
            HandleDefault(TaskList);

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

            RectangleX TrayNotifyPos = new RectangleX();
            RectangleX NewsAndInterestsPos = new RectangleX();
            IntPtr NewsAndInterestsHandle = 0;

            IntPtr TaskListParent = Win32.Intertop.GetParent(TaskList);
            IntPtr TrayWndHandle = Win32.Intertop.GetParent(TaskListParent);

            string TrayWndClassName = Win32.Api.GetClassName(TrayWndHandle);

            // Check if TrayWnd = wrong. if it is, correct it
            // (This will be the primary taskbar which should be Shell_TrayWnd)
            if (TrayWndClassName == "ReBarWindow32")
            {
                CorrectTaskbar(TaskList, out TrayNotifyPos, ref NewsAndInterestsPos, ref NewsAndInterestsHandle, out TaskListParent, ref TrayWndHandle);
            }

            TrayWndClassName = Win32.Api.GetClassName(TrayWndHandle);
            IAccessible accessible2 = MSAA.Api.GetAccessibleFromHandle(TrayWndHandle);

            RectangleX TrayWndPos = MSAA.Api.GetLocation(accessible2, 0);
            RectangleX RebarPos = MSAA.Api.GetLocation(accessible3, 0);

            // If the taskbar is still moving then wait until it's not
            // (This will prevent unneeded calculations that trigger the animator)

            int curleft;
            int curleft2;
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
            int TaskbarWidth = CalculateWidth(LastChildPos, TaskListPos);

            bool isCenterPrimary =
                    settings.CenterPrimaryOnly == 1 &&
                    TrayWndClassName == "Shell_TrayWnd";

            int oldPosition = CalculateOldPosition(TaskListPos, RebarPos);

            int TrayWndLeft = settings.HorizontalOrientation ? Math.Abs(TrayWndPos.left) : Math.Abs(TrayWndPos.top);
            int TrayWndWidth = settings.HorizontalOrientation ? Math.Abs(TrayWndPos.width) : Math.Abs(TrayWndPos.height);
            int RebarWndLeft = settings.HorizontalOrientation ? Math.Abs(RebarPos.left) : Math.Abs(RebarPos.top);
            int TaskbarLeft = Math.Abs(RebarWndLeft - TrayWndLeft);
            int newPosition = CalculateNewPosition(TaskbarWidth, TrayWndWidth, TaskbarLeft, TrayNotifyPos, NewsAndInterestsPos, TrayWndClassName);


            //Console.WriteLine("!" + NewsAndInterestsPos.width);
            separator.SeparateToSegments(TaskList, TaskbarWidth, TaskbarLeft, newPosition, TrayNotifyPos, TrayWndHandle, TrayWndClassName, TrayWndPos);

            animator.StartIfEnable(TaskList, TaskbarWidth, oldPosition, newPosition);
        }
    }

    private static void CorrectTaskbar(nint TaskList, out RectangleX TrayNotifyPos, ref RectangleX NewsAndInterestsPos, ref nint NewsAndInterestsHandle, out nint TaskListParent, ref nint TrayWndHandle)
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

    private int CalculateNewPosition(int TaskbarWidth, int TrayWndWidth, int TaskbarLeft, RectangleX TrayNotifyPos, RectangleX NewsAndInterestsPos, string TrayWndClassName)
    {
        int newPosition;
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
        return newPosition;
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

        List<IntPtr> taskbars = Win32.Api.GetTaskbars();

        foreach (IntPtr taskbar in taskbars)
        {
            string sClassName = Win32.Api.GetClassName(taskbar);

            if (sClassName == "Shell_TrayWnd")
            {
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
            }

            if (sClassName == "Shell_SecondaryTrayWnd")
            {

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
            }

            Win32.Api.SetDefaultPosition(taskbar);
        }
    }

    private static void HandleDefault(IntPtr taskbar)
    {
        if (taskbar == default)
        {
            Console.WriteLine("TaskbarX: " +
                "Could not find the handle of the taskbar. " +
                "Restarting...");

            Task.Delay(1000);
            Application.Restart();
        }
    }
}

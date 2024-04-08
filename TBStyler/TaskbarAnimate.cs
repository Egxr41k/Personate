using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personate.Settings;


namespace TBStyler;

public class TaskbarAnimate
{
    public static ArrayList current = new ArrayList();
    private TaskbarSettingsDTO Settings { get; set; }

    public TaskbarAnimate(TaskbarSettingsDTO settings)
    {
        Settings = settings;
    }

    public void Animate(IntPtr hwnd, int oldpos, string orient, EasingDelegate easing, int valueToReach, int duration, bool isPrimary, int width)
    {
        try
        {
            if (Math.Abs(valueToReach - oldpos) == 0)
            {
                // The difference is 0 so there is no need to trigger the animator.
                return;
            }

            if (Settings.RevertZeroBeyondTray == 1)
            {
                // Prevent moving beyond Tray area.
                Win32.Types.Rect TrayPos2 = new Win32.Types.Rect();
                Win32.Intertop.GetWindowRect(Win32.Intertop.GetParent(hwnd), ref TrayPos2);
                int rightposition = valueToReach + width;

                if (orient == "H")
                {
                    if (rightposition >= TrayPos2.Right - TrayPos2.Left)
                    {
                        Win32.Intertop.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                        return;
                    }
                }
                else
                {
                    if (rightposition >= TrayPos2.Bottom - TrayPos2.Top)
                    {
                        Win32.Intertop.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                        return;
                    }
                }
            }

            if (valueToReach == oldpos || Math.Abs(valueToReach - oldpos) <= 10)
            {
                // Prevent Wiggling (if the new position has a difference of 10 or lower then there is no reason to move)
                return;
            }

            foreach (var tt in current)
            {
                if ((IntPtr)tt == hwnd)
                {
                    // If hwnd is already getting animated then hwnd is in this arraylist and exit the animator because it's unneeded.
                    return;
                }
            }

            current.Add(hwnd);

            Stopwatch sw = new Stopwatch();
            int originalValue = oldpos;
            int elapsed = 0;
            int minValue = originalValue <= valueToReach ? originalValue : valueToReach;
            int maxValue = Math.Abs(valueToReach - originalValue);
            bool increasing = originalValue < valueToReach;

            elapsed = 0;
            sw.Start();

            if (isPrimary)
            {
                TaskbarCenter.isanimating = true;
            }

            while (!(elapsed >= duration))
            {
                elapsed = (int)sw.ElapsedMilliseconds;

                int newValue = (int)(easing(elapsed, minValue, maxValue, duration));

                if (!increasing)
                {
                    newValue = (originalValue + valueToReach) - newValue;
                }

                if (orient == "H")
                {
                    Win32.Intertop.SetWindowPos(hwnd, IntPtr.Zero, newValue, 0, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                }
                else
                {
                    Win32.Intertop.SetWindowPos(hwnd, IntPtr.Zero, 0, newValue, 0, 0, Win32.Intertop.SWP_NOSIZE | Win32.Intertop.SWP_ASYNCWINDOWPOS | Win32.Intertop.SWP_NOACTIVATE | Win32.Intertop.SWP_NOZORDER | Win32.Intertop.SWP_NOSENDCHANGING);
                }
            }

            if (isPrimary)
            {
                TaskbarCenter.isanimating = false;
            }

            sw.Stop();
            current.Remove(hwnd);

            Setuper.ClearMemory();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

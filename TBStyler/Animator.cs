using System;
using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using TBStyler.MSAA;
using TBStyler.Win32;
using TBStyler.Win32.Types;
using Personate.Settings;

namespace TBStyler;

public class Animator
{
    private List<IntPtr> current { get; set; }
    private TaskbarSettingsDTO settings { get; set; }

    public Animator(TaskbarSettingsDTO settings)
    {
        this.settings = settings;
        current = [];
    }

    public void StartIfEnable(IntPtr hwnd, int width, int oldposition, int valueToReach)
    {
        if (CheckIsEnable(hwnd, width, oldposition, valueToReach))
        {
            Animate(hwnd, oldposition, valueToReach);
        }
    }

    private bool CheckIsEnable(IntPtr hwnd, int width, int oldposition, int valueToReach)
    {
        bool isEnable = true;

        if (settings.RevertZeroBeyondTray == 1)
        {
            // Prevent moving beyond Tray area.
            int offset = CalculateOffset(hwnd);
            int rightposition = valueToReach + width;
            if (rightposition >= offset)
            {
                Win32.Api.SetHwndPosition(hwnd, 0, 0);
                isEnable = false;
            }
        }
        
        // Prevent Wiggling (if the new position has a difference of 10 or lower then there is no reason to move)
        int distance = Math.Abs(valueToReach - oldposition);
        if (distance <= 10) isEnable = false;
        if (current.Contains(hwnd)) isEnable = false;

        return isEnable;
    }

    private void Animate(IntPtr hwnd, int originalValue, int valueToReach)
    {
        try
        {
            AnimationStyle animationStyle = GetStyle();
            current.Add(hwnd);

            Stopwatch sw = new Stopwatch();
            int elapsed = 0;
            int minValue = originalValue <= valueToReach ? originalValue : valueToReach;
            int maxValue = Math.Abs(valueToReach - originalValue);
            bool increasing = originalValue < valueToReach;

            elapsed = 0;
            sw.Start();
            while (!(elapsed >= settings.AnimationSpeed))
            {
                elapsed = (int)sw.ElapsedMilliseconds;

                int newValue = (int)(animationStyle(elapsed, minValue, maxValue, settings.AnimationSpeed));

                if (originalValue > valueToReach)
                {
                    newValue = (originalValue + valueToReach) - newValue;
                }
                Win32.Api.SetHwndPosition(
                    hwnd,
                    settings.HorizontalOrientation ? newValue : 0,
                    settings.HorizontalOrientation ? 0 : newValue);
            }
            sw.Stop();
            current.Remove(hwnd);
            //PerformanceOptimizer.ClearMemory();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private int CalculateOffset(nint hwnd)
    {
        Rect TrayPos2 = new Rect();
        Win32.Intertop.GetWindowRect(Win32.Intertop.GetParent(hwnd), ref TrayPos2);

        int horizontalOffset = TrayPos2.Right - TrayPos2.Left;
        int verticallOffset = TrayPos2.Bottom - TrayPos2.Top;

        int offset = settings.HorizontalOrientation ?
            horizontalOffset :
            verticallOffset;
        return offset;
    }

    private AnimationStyle GetStyle()
    {
        string styleName = GetStyleName();
        return GetStyleByName(styleName);
    }

    private string GetStyleName()
    {
        bool isOnBattery = SystemInformation.PowerStatus.PowerLineStatus ==
                    PowerLineStatus.Offline;
        bool noAnimationOnBattery =
            settings.OnBatteryAnimationStyle == "none";

        bool noAnimationOnPower =
            settings.AnimationStyle == "none";

        bool noAnimation = isOnBattery ? noAnimationOnBattery : noAnimationOnPower;
        string animationStyle = isOnBattery ? settings.OnBatteryAnimationStyle : settings.AnimationStyle;

        if (noAnimation) return string.Empty;
        else return animationStyle;
    }

    private AnimationStyle GetStyleByName(string styleName)
    {
        return styleName switch
        {
            "linear" => AnimationStyles.Linear,

            "expoeaseout" => AnimationStyles.ExpoEaseOut,
            "expoeasein" => AnimationStyles.ExpoEaseIn,
            "expoeaseinout" => AnimationStyles.ExpoEaseInOut,
            "expoeaseoutin" => AnimationStyles.ExpoEaseOutIn,

            "circeaseout" => AnimationStyles.CircEaseOut,
            "circeasein" => AnimationStyles.CircEaseIn,
            "circeaseinout" => AnimationStyles.CircEaseInOut,
            "circeaseoutin" => AnimationStyles.CircEaseOutIn,

            "quadeaseout" => AnimationStyles.QuadEaseOut,
            "quadeasein" => AnimationStyles.QuadEaseIn,
            "quadeaseinout" => AnimationStyles.QuadEaseInOut,
            "quadeaseoutin" => AnimationStyles.QuadEaseOutIn,

            "sineeaseout" => AnimationStyles.SineEaseOut,
            "sineeasein" => AnimationStyles.SineEaseIn,
            "sineeaseinout" => AnimationStyles.SineEaseInOut,
            "sineeaseoutin" => AnimationStyles.SineEaseOutIn,

            "cubiceaseout" => AnimationStyles.CubicEaseOut,
            "cubiceasein" => AnimationStyles.CubicEaseIn,
            "cubiceaseinout" => AnimationStyles.CubicEaseInOut,
            "cubiceaseoutin" => AnimationStyles.CubicEaseOutIn,

            "quarteaseout" => AnimationStyles.QuartEaseOut,
            "quarteasein" => AnimationStyles.QuartEaseIn,
            "quarteaseinout" => AnimationStyles.QuartEaseInOut,
            "quarteaseoutin" => AnimationStyles.QuartEaseOutIn,

            "quinteaseout" => AnimationStyles.QuintEaseOut,
            "quinteasein" => AnimationStyles.QuintEaseIn,
            "quinteaseinout" => AnimationStyles.QuintEaseInOut,
            "quinteaseoutin" => AnimationStyles.QuintEaseOutIn,

            "elasticeaseout" => AnimationStyles.ElasticEaseOut,
            "elasticeasein" => AnimationStyles.ElasticEaseIn,
            "elasticeaseinout" => AnimationStyles.ElasticEaseInOut,
            "elasticeaseoutin" => AnimationStyles.ElasticEaseOutIn,

            "bounceeaseout" => AnimationStyles.BounceEaseOut,
            "bounceeasein" => AnimationStyles.BounceEaseIn,
            "bounceeaseinout" => AnimationStyles.BounceEaseInOut,
            "bounceeaseoutin" => AnimationStyles.BounceEaseOutIn,

            "backeaseout" => AnimationStyles.BackEaseOut,
            "backeasein" => AnimationStyles.BackEaseIn,
            "backeaseinout" => AnimationStyles.BackEaseInOut,
            "backeaseoutin" => AnimationStyles.BackEaseOutIn,

            _ => AnimationStyles.None,
        };
    }
}

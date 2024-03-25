using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBStyler;

public delegate double EasingDelegate(double currentTime, double minValue, double maxValue, double duration);

internal class Easings
{
    public static double None(double currentTime, double minHeight, double maxHeight, double duration)
    {
        return maxHeight;
    }

    public static double Linear(double currentTime, double minHeight, double maxHeight, double duration)
    {
        return maxHeight * currentTime / duration + minHeight;
    }

    public static double ExpoEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime != duration)
            return maxHeight * (-Math.Pow(2.0, -10.0 * currentTime / duration) + 1.0) + minHeight;
        return minHeight + maxHeight;
    }

    public static double ExpoEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime != 0.0)
            return maxHeight * Math.Pow(2.0, 10.0 * (currentTime / duration - 1.0)) + minHeight;
        return minHeight;
    }

    public static double ExpoEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime == 0.0)
            return minHeight;
        if (currentTime == duration)
            return minHeight + maxHeight;
        double num = currentTime / (duration / 2.0);
        currentTime = num;
        if (num < 1.0)
            return maxHeight / 2.0 * Math.Pow(2.0, 10.0 * (currentTime - 1.0)) + minHeight;
        double num2 = maxHeight / 2.0;
        double x = 2.0;
        double num3 = -10.0;
        double num4 = currentTime - 1.0;
        return num2 * (-Math.Pow(x, num3 * num4) + 2.0) + minHeight;
    }

    public static double ExpoEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.ExpoEaseOut(currentTime * 2.0, minHeight, maxHeight / 2.0, duration);
        return Easings.ExpoEaseIn(currentTime * 2.0 - duration, minHeight + maxHeight / 2.0, maxHeight / 2.0, duration);
    }

    public static double CircEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = 1.0;
        double num2 = currentTime / duration - 1.0;
        currentTime = num2;
        return maxHeight * Math.Sqrt(num - num2 * currentTime) + minHeight;
    }

    public static double CircEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = 1.0;
        double num2 = currentTime / duration;
        currentTime = num2;
        double sqrt = Math.Sqrt(num - num2 * currentTime);
        if (double.IsNaN(sqrt))
            sqrt = 0.0;
        return -maxHeight * (sqrt - 1.0) + minHeight;
    }

    public static double CircEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / (duration / 2.0);
        currentTime = num;
        if (num < 1.0)
            return -maxHeight / 2.0 * (Math.Sqrt(1.0 - currentTime * currentTime) - 1.0) + minHeight;
        double num2 = maxHeight / 2.0;
        double num3 = 1.0;
        double num4 = currentTime - 2.0;
        currentTime = num4;
        return num2 * (Math.Sqrt(num3 - num4 * currentTime) + 1.0) + minHeight;
    }

    public static double CircEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.CircEaseOut(currentTime * 2.0, minHeight, maxHeight / 2.0, duration);
        return Easings.CircEaseIn(currentTime * 2.0 - duration, minHeight + maxHeight / 2.0, maxHeight / 2.0, duration);
    }

    public static double QuadEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = -maxHeight;
        double num2 = currentTime / duration;
        currentTime = num2;
        return num * num2 * (currentTime - 2.0) + minHeight;
    }

    public static double QuadEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration;
        currentTime = num;
        return maxHeight * num * currentTime + minHeight;
    }

    public static double QuadEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / (duration / 2.0);
        currentTime = num;
        if (num < 1.0)
            return maxHeight / 2.0 * currentTime * currentTime + minHeight;
        double num2 = -maxHeight / 2.0;
        double num3 = currentTime - 1.0;
        currentTime = num3;
        return num2 * (num3 * (currentTime - 2.0) - 1.0) + minHeight;
    }

    public static double QuadEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.QuadEaseOut(currentTime * 2.0, minHeight, maxHeight / 2.0, duration);
        return Easings.QuadEaseIn(currentTime * 2.0 - duration, minHeight + maxHeight / 2.0, maxHeight / 2.0, duration);
    }

    public static double SineEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        return maxHeight * Math.Sin(currentTime / duration * 1.5707963267948966) + minHeight;
    }

    public static double SineEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        return -maxHeight * Math.Cos(currentTime / duration * 1.5707963267948966) + maxHeight + minHeight;
    }

    public static double SineEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / (duration / 2.0);
        currentTime = num;
        if (num < 1.0)
            return maxHeight / 2.0 * Math.Sin(3.1415926535897931 * currentTime / 2.0) + minHeight;
        double num2 = -maxHeight / 2.0;
        double num3 = 3.1415926535897931;
        double num4 = currentTime - 1.0;
        return num2 * (Math.Cos(num3 * num4 / 2.0) - 2.0) + minHeight;
    }

    public static double SineEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.SineEaseOut(currentTime * 2.0, minHeight, maxHeight / 2.0, duration);
        return Easings.SineEaseIn(currentTime * 2.0 - duration, minHeight + maxHeight / 2.0, maxHeight / 2.0, duration);
    }

    public static double CubicEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration - 1.0;
        currentTime = num;
        return maxHeight * (num * currentTime * currentTime + 1.0) + minHeight;
    }

    public static double CubicEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration;
        currentTime = num;
        return maxHeight * num * currentTime * currentTime + minHeight;
    }

    public static double CubicEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / (duration / 2.0);
        currentTime = num;
        if (num < 1.0)
            return maxHeight / 2.0 * currentTime * currentTime * currentTime + minHeight;
        double num2 = maxHeight / 2.0;
        double num3 = currentTime - 2.0;
        currentTime = num3;
        return num2 * (num3 * currentTime * currentTime + 2.0) + minHeight;
    }

    public static double CubicEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.CubicEaseOut(currentTime * 2.0, minHeight, maxHeight / 2.0, duration);
        return Easings.CubicEaseIn(currentTime * 2.0 - duration, minHeight + maxHeight / 2.0, maxHeight / 2.0, duration);
    }

    public static double QuartEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = -maxHeight;
        double num2 = currentTime / duration - 1.0;
        currentTime = num2;
        return num * (num2 * currentTime * currentTime * currentTime - 1.0) + minHeight;
    }

    public static double QuartEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration;
        currentTime = num;
        return maxHeight * num * currentTime * currentTime * currentTime + minHeight;
    }

    public static double QuartEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / (duration / 2.0);
        currentTime = num;
        if (num < 1.0)
            return maxHeight / 2.0 * currentTime * currentTime * currentTime * currentTime + minHeight;
        double num2 = -maxHeight / 2.0;
        double num3 = currentTime - 2.0;
        currentTime = num3;
        return num2 * (num3 * currentTime * currentTime * currentTime - 2.0) + minHeight;
    }

    public static double QuartEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.QuartEaseOut(currentTime * 2.0, minHeight, maxHeight / 2.0, duration);
        return Easings.QuartEaseIn(currentTime * 2.0 - duration, minHeight + maxHeight / 2.0, maxHeight / 2.0, duration);
    }

    public static double QuintEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration - 1.0;
        currentTime = num;
        return maxHeight * (num * currentTime * currentTime * currentTime * currentTime + 1.0) + minHeight;
    }

    public static double QuintEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration;
        currentTime = num;
        return maxHeight * num * currentTime * currentTime * currentTime * currentTime + minHeight;
    }

    public static double QuintEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / (duration / 2.0);
        currentTime = num;
        if (num < 1.0)
            return maxHeight / 2.0 * currentTime * currentTime * currentTime * currentTime * currentTime + minHeight;
        double num2 = maxHeight / 2.0;
        double num3 = currentTime - 2.0;
        currentTime = num3;
        return num2 * (num3 * currentTime * currentTime * currentTime * currentTime + 2.0) + minHeight;
    }

    public static double QuintEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.QuintEaseOut(currentTime * 2.0, minHeight, maxHeight / 2.0, duration);
        return Easings.QuintEaseIn(currentTime * 2.0 - duration, minHeight + maxHeight / 2.0, maxHeight / 2.0, duration);
    }

    public static double ElasticEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration;
        currentTime = num;
        if (num == 1.0)
            return minHeight + maxHeight;
        double p = duration * 0.3;
        double s = p / 4.0;
        return maxHeight * Math.Pow(2.0, -10.0 * currentTime) * Math.Sin((currentTime * duration - s) * 6.2831853071795862 / p) + maxHeight + minHeight;
    }

    public static double ElasticEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration;
        currentTime = num;
        if (num == 1.0)
            return minHeight + maxHeight;
        double p = duration * 0.3;
        double s = p / 4.0;
        double x = 2.0;
        double num2 = 10.0;
        double num3 = currentTime - 1.0;
        currentTime = num3;
        return -(maxHeight * Math.Pow(x, num2 * num3) * Math.Sin((currentTime * duration - s) * 6.2831853071795862 / p)) + minHeight;
    }

    public static double ElasticEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / (duration / 2.0);
        currentTime = num;
        if (num == 2.0)
            return minHeight + maxHeight;
        double p = duration * 0.44999999999999996;
        double s = p / 4.0;
        if (currentTime < 1.0)
        {
            double num2 = -0.5;
            double x = 2.0;
            double num3 = 10.0;
            double num4 = currentTime - 1.0;
            currentTime = num4;
            return num2 * (maxHeight * Math.Pow(x, num3 * num4) * Math.Sin((currentTime * duration - s) * 6.2831853071795862 / p)) + minHeight;
        }
        double x2 = 2.0;
        double num5 = -10.0;
        double num6 = currentTime - 1.0;
        currentTime = num6;
        return maxHeight * Math.Pow(x2, num5 * num6) * Math.Sin((currentTime * duration - s) * 6.2831853071795862 / p) * 0.5 + maxHeight + minHeight;
    }

    public static double ElasticEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.ElasticEaseOut(currentTime * 2.0, minHeight, maxHeight / 2.0, duration);
        return Easings.ElasticEaseIn(currentTime * 2.0 - duration, minHeight + maxHeight / 2.0, maxHeight / 2.0, duration);
    }

    public static double BounceEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration;
        currentTime = num;
        if (num < 0.36363636363636365)
            return maxHeight * (7.5625 * currentTime * currentTime) + minHeight;
        if (currentTime < 0.72727272727272729)
        {
            double num2 = 7.5625;
            double num3 = currentTime - 0.54545454545454541;
            currentTime = num3;
            return maxHeight * (num2 * num3 * currentTime + 0.75) + minHeight;
        }
        if (currentTime < 0.90909090909090906)
        {
            double num4 = 7.5625;
            double num5 = currentTime - 0.81818181818181823;
            currentTime = num5;
            return maxHeight * (num4 * num5 * currentTime + 0.9375) + minHeight;
        }
        double num6 = 7.5625;
        double num7 = currentTime - 0.95454545454545459;
        currentTime = num7;
        return maxHeight * (num6 * num7 * currentTime + 0.984375) + minHeight;
    }

    public static double BounceEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        return maxHeight - Easings.BounceEaseOut(duration - currentTime, 0.0, maxHeight, duration) + minHeight;
    }

    public static double BounceEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.BounceEaseIn(currentTime * 2.0, 0.0, maxHeight, duration) * 0.5 + minHeight;
        return Easings.BounceEaseOut(currentTime * 2.0 - duration, 0.0, maxHeight, duration) * 0.5 + maxHeight * 0.5 + minHeight;
    }

    public static double BounceEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.BounceEaseOut(currentTime * 2.0, minHeight, maxHeight / 2.0, duration);
        return Easings.BounceEaseIn(currentTime * 2.0 - duration, minHeight + maxHeight / 2.0, maxHeight / 2.0, duration);
    }

    public static double BackEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration - 1.0;
        currentTime = num;
        return maxHeight * (num * currentTime * (2.70158 * currentTime + 1.70158) + 1.0) + minHeight;
    }

    public static double BackEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double num = currentTime / duration;
        currentTime = num;
        return maxHeight * num * currentTime * (2.70158 * currentTime - 1.70158) + minHeight;
    }

    public static double BackEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
    {
        double s = 1.70158;
        double num = currentTime / (duration / 2.0);
        currentTime = num;
        if (num < 1.0)
        {
            double num2 = maxHeight / 2.0;
            double num3 = currentTime * currentTime;
            double num4 = s * 1.525;
            s = num4;
            return num2 * (num3 * ((num4 + 1.0) * currentTime - s)) + minHeight;
        }
        double num5 = maxHeight / 2.0;
        double num6 = currentTime - 2.0;
        currentTime = num6;
        double num7 = num6 * currentTime;
        double num8 = s * 1.525;
        s = num8;
        return num5 * (num7 * ((num8 + 1.0) * currentTime + s) + 2.0) + minHeight;
    }

    public static double BackEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
    {
        if (currentTime < duration / 2.0)
            return Easings.BackEaseOut(currentTime * 2.0, minHeight, maxHeight / 2.0, duration);
        return Easings.BackEaseIn(currentTime * 2.0 - duration, minHeight + maxHeight / 2.0, maxHeight / 2.0, duration);
    }
}

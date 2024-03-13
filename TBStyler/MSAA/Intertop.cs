using System.Runtime.InteropServices;
using System.Text;
using Accessibility;

namespace TBStyler;

public class Intertop
{
    public const uint STATE_SYSTEM_ALERT_HIGH = 268435456;
    public const uint STATE_SYSTEM_ALERT_LOW = 67108864;
    public const uint STATE_SYSTEM_ALERT_MEDIUM = 134217728;
    public const uint STATE_SYSTEM_ANIMATED = 16384;
    public const uint STATE_SYSTEM_BUSY = 2048;
    public const uint STATE_SYSTEM_CHECKED = 16;
    public const uint STATE_SYSTEM_COLLAPSED = 1024;
    public const uint STATE_SYSTEM_DEFAULT = 256;
    public const uint STATE_SYSTEM_EXPANDED = 512;
    public const uint STATE_SYSTEM_EXTSELECTABLE = 33554432;
    public const uint STATE_SYSTEM_FLOATING = 4096;
    public const uint STATE_SYSTEM_FOCUSABLE = 1048576;
    public const uint STATE_SYSTEM_FOCUSED = 4;
    public const uint STATE_SYSTEM_HASPOPUP = 1073741824;
    public const uint STATE_SYSTEM_HOTTRACKED = 128;
    public const uint STATE_SYSTEM_INVISIBLE = 32768;
    public const uint STATE_SYSTEM_LINKED = 4194304;
    public const uint STATE_SYSTEM_MARQUEED = 8192;
    public const uint STATE_SYSTEM_MIXED = 32;
    public const uint STATE_SYSTEM_MOVEABLE = 262144;
    public const uint STATE_SYSTEM_MULTISELECTABLE = 16777216;
    public const uint STATE_SYSTEM_NORMAL = 0;
    public const uint STATE_SYSTEM_OFFSCREEN = 65536;
    public const uint STATE_SYSTEM_PRESSED = 8;
    public const uint STATE_SYSTEM_READONLY = 64;
    public const uint STATE_SYSTEM_SELECTABLE = 2097152;
    public const uint STATE_SYSTEM_SELECTED = 2;
    public const uint STATE_SYSTEM_SELFVOICING = 524288;
    public const uint STATE_SYSTEM_SIZEABLE = 131072;
    public const uint STATE_SYSTEM_TRAVERSED = 8388608;
    public const uint STATE_SYSTEM_UNAVAILABLE = 1;
    public const uint STATE_SYSTEM_VALID = 536870911;

    [DllImport("oleacc.dll")]
    public static extern uint WindowFromAccessibleObject(
        IAccessible pacc,
        out nint phwnd);

    [DllImport("oleacc.dll")]
    public static extern uint AccessibleChildren(
        IAccessible paccContainer,
        int iChildStart,
        int cChildren,
        [Out] object[] rgvarChildren,
        out int pcObtained);

    [DllImport("oleacc.dll")]
    public static extern uint GetStateText(
        uint dwStateBit, [
        Out] StringBuilder lpszStateBit,
        uint cchStateBitMax);

    [DllImport("oleacc.dll")]
    public static extern int AccessibleObjectFromWindow(
        int Hwnd,
        int dwId,
        ref Guid riid,
        [MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
}

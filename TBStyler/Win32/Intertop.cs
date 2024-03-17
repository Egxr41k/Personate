using System.Runtime.InteropServices;
using TBStyler.Win32.Types;

namespace TBStyler.Win32;

public partial class Intertop
{
    public const Int32 GWL_STYLE = -16;
    public const Int32 GWL_EXSTYLE = -20;
    public const Int32 WS_MAXIMIZE = 16777216;
    public const Int64 WS_POPUP = 2147483648;
    public const Int32 WS_EX_LAYERED = 524288;

    public const UInt32 SWP_NOSIZE = 1;
    public const UInt32 SWP_ASYNCWINDOWPOS = 16384;
    public const UInt32 SWP_NOACTIVATE = 16;
    public const UInt32 SWP_NOSENDCHANGING = 1024;
    public const UInt32 SWP_NOZORDER = 4;
    public const Int64 WM_COMMAND = 0x111;
    public const IntPtr HWND_BROADCAST = 65535;
    public const UInt32 WM_SETTINGCHANGE = 26;
    public const Int32 SMTO_ABORTIFHUNG = 2;

    public const UInt32 TOPMOST_FLAGS = 0x2 | 0x1;
    public const IntPtr HWND_TOPMOST = -1;

    public const Int32 WM_DWMCOLORIZATIONCOLORCHANGED = 0x320;
    public const Int32 WM_DWMCOMPOSITIONCHANGED = 0x31E;
    public const Int32 WM_THEMECHANGED = 0x31A;

    public const Int32 WM_SETREDRAW = 11;

    public const Int32 WCA_ACCENT_POLICY = 19;

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(
        IntPtr hWnd,
        [MarshalAs(UnmanagedType.I4)] ShowWindowCommands nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        Int32 X,
        Int32 Y,
        Int32 cx,
        Int32 cy,
        UInt32 uFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern Int32 GetClassName(
        IntPtr hWnd,
        System.Text.StringBuilder lpClassName,
        Int32 nMaxCount);

    [DllImport("user32.dll")]
    public static extern bool GetWindowPlacement(
        IntPtr hWnd,
        ref WindowPlacement lpwndpl);

    [DllImport("user32")]
    public static extern Int32 EnumWindows(
        EnumWindowProcess enumProc,
        IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern bool EnumChildWindows(
        IntPtr WindowHandle,
        EnumWindowProcess enumProc,
        IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern Int32 SetWindowCompositionAttribute(
        IntPtr hwnd,
        ref WindowCompositionAttributeData data);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr FindWindowEx(
        IntPtr parentHandle,
        IntPtr childAfter,
        string lclassName,
        string windowTitle);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr FindWindow(
        string lpClassName,
        string lpWindowName);

    [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern IntPtr GetParent(
        IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern Int32 GetClientRect(
        IntPtr hWnd,
        ref Rect lpRECT);

    [DllImport("user32.dll")]
    public static extern Int32 SetWindowRgn(
        IntPtr hWnd,
        IntPtr hRgn,
        bool bRedraw);

    [DllImport("user32.dll")]
    public static extern Int32 GetWindowRgn(
        IntPtr hWnd,
        IntPtr hRgn);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateRoundRectRgn(
        Int32 x1,
        Int32 y1,
        Int32 x2,
        Int32 y2,
        Int32 w,
        Int32 h);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateRectRgn(
        Int32 nLeftRect,
        Int32 nTopRect,
        Int32 nRightRect,
        Int32 nBottomRect);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SetParent(
        IntPtr hWndChild,
        IntPtr hWndNewParent);

    [DllImport("user32.dll")]
    public static extern IntPtr MonitorFromWindow(
        IntPtr hwnd,
        UInt32 dwFlags);

    [DllImport("user32.dll")]
    public static extern bool GetMonitorInfo(
        IntPtr hMonitor,
        ref MonitorInfo lpmi);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(
        IntPtr hWnd,
        ref Rect lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern Int32 GetWindowLong(
        IntPtr hWnd,
        Int32 nIndex);

    [DllImport("SHCore.dll", SetLastError = true)]
    public static extern bool SetProcessDpiAwareness(
        ProcessDpiAwareness awareness);

    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr FindWindowByClass(
        string lpClassName,
        IntPtr zero);

    [DllImport("user32.dll")]
    public static extern Int32 SendMessage(IntPtr hWnd,
        Int32 wMsg,
        bool wParam,
        Int32 lParam);

    [DllImport("kernel32.dll")]
    public static extern Int32 SetProcessWorkingSetSize(
        IntPtr hProcess,
        Int32 dwMinimumWorkingSetSize,
        Int32 dwMaximumWorkingSetSize);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    public static extern Int32 SetWindowLong(
        IntPtr hWnd,
        [MarshalAs(UnmanagedType.I4)] WindowStyles nIndex,
        int dwNewLong);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool PostMessage(
        IntPtr hWnd,
        UInt32 Msg,
        IntPtr wParam,
        IntPtr lParam);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool AllocConsole();

    [DllImport("user32.dll", SetLastError = false)]
    public static extern IntPtr GetDesktopWindow();

    [LibraryImport("dwmapi.dll")]
    public static partial Int32 DwmGetWindowAttribute(
        IntPtr hwnd,
        DwmWindowAttribute dwAttribute,
        ref Int32 pvAttribute,
        Int32 cbAttribute);

    [DllImport("dwmapi.dll")]
    public static extern Int32 DwmSetWindowAttribute(
        IntPtr hwnd,
        DwmWindowAttribute dwAttribute,
        ref Rect pvAttribute,
        Int32 cbAttribute);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    public static extern bool SendNotifyMessage(
        IntPtr hWnd,
        UInt32 Msg,
        IntPtr wParam,
        string lParam);


    [DllImport("user32.dll")]
    public static extern bool SetLayeredWindowAttributes(
        IntPtr hwnd,
        UInt32 crKey,
        byte bAlpha,
        UInt32 dwFlags);


    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool RedrawWindow(
        IntPtr hWnd,
        IntPtr lprcUpdate,
        IntPtr hrgnUpdate,
        RedrawWindowFlags flags);

    [DllImport("gdi32.dll")]
    public static extern Int32 CombineRgn(
        IntPtr hrgnDest, 
        IntPtr hrgnSrc1, 
        IntPtr hrgnSrc2,
        Int32 fnCombineMode);

    public delegate bool EnumWindowProcess(IntPtr Handle, IntPtr Parameter);

    [DllImport("user32.dll")]
    public static extern void keybd_event(
        byte bVk,
        byte bScan,
        Int32 dwFlags,
        UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    public static extern IntPtr SetFocus(
        IntPtr hwnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowText(
        IntPtr hwnd,
        System.Text.StringBuilder lpString,
        Int32 cch);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    public static extern IntPtr GetWindowLongPtr(
        HandleRef hWnd,
        [MarshalAs(UnmanagedType.I4)] WindowLongFlags nIndex);
}

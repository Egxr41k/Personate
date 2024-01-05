using System.Runtime.InteropServices;

namespace Personate.General;
internal static class Win32
{
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SystemParametersInfo(
        uint uiAction,
        uint uiParam,
        string pvParam,
        uint fWinIni);

    [DllImport("Setupapi.dll", EntryPoint = "InstallHinfSection", CallingConvention = CallingConvention.StdCall)]
    public static extern void InstallHinfSection(
        [In] nint hwnd,
        [In] nint ModuleHandle,
        [In, MarshalAs(UnmanagedType.LPWStr)] string CmdLineBuffer,
        int nCmdShow);

}

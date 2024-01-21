using System.Diagnostics;
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

    public static bool ExecuteWithCmd(string command)
    {
        // Создание процесса и выполнение команды
        Process process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Verb = "runas",
            UseShellExecute = true,
            Arguments = "/c " + command,
            CreateNoWindow = false,
        };

        // запускаем процесс
        process.Start();

        // Ожидание завершения процесса
        process.WaitForExit();

        return process.ExitCode == 0;
    }
}

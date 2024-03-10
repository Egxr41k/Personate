using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBStyler;

internal static class PerformanceOptimizer
{
    private static Process currentProcess = Process.GetCurrentProcess();
    public static void KillProcessByName(string processName)
    {
        Process? process = Process.GetProcesses()
            .FirstOrDefault(process => 
                process.ProcessName == processName && 
                process.Id != currentProcess.Id);
        process?.Kill();
    }

    public static int ClearMemory()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        return Win32.Intertop.SetProcessWorkingSetSize(currentProcess.Handle, -1, -1);
    }
}

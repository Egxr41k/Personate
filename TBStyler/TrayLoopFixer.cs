using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBStyler;

public class TrayLoopFixer
{
    const uint SWP_NOSIZE = 1;
    const uint SWP_ASYNCWINDOWPOS = 16384;
    const uint SWP_NOACTIVATE = 16;
    const uint SWP_NOSENDCHANGING = 1024;
    const uint SWP_NOZORDER = 4;
    public void Fix()
    {

        IntPtr shellTrayWnd = Win32.Intertop.FindWindowByClass("Shell_TrayWnd", 0);
        IntPtr trayNotifyWnd = Win32.Intertop.FindWindowEx(shellTrayWnd, 0, "TrayNotifyWnd", null);
        IntPtr reBarWindow32 = Win32.Intertop.FindWindowEx(shellTrayWnd, 0, "ReBarWindow32", null);
        IntPtr mSTaskSwWClass = Win32.Intertop.FindWindowEx(reBarWindow32, 0, "MSTaskSwWClass", null);
        IntPtr mSTaskListWClass = Win32.Intertop.FindWindowEx(mSTaskSwWClass, 0, "MSTaskListWClass", null);

        Accessibility.IAccessible accessible = MSAA.Api.GetAccessibleFromHandle(mSTaskListWClass);
        Accessibility.IAccessible accessible2 = MSAA.Api.GetAccessibleFromHandle(trayNotifyWnd);
        Accessibility.IAccessible accessible3 = MSAA.Api.GetAccessibleFromHandle(mSTaskSwWClass);

        do
        {
            MSAA.RectangleX rebarPos = MSAA.Api.GetLocation(accessible3, 0);
            MSAA.RectangleX trayNotifyPos = MSAA.Api.GetLocation(accessible2, 0);
            MSAA.RectangleX taskListPos = MSAA.Api.GetLocation(accessible, 0);

            Win32.Intertop.SendMessage(reBarWindow32, 11, false, 0);
            Win32.Intertop.SendMessage(Win32.Intertop.GetParent(shellTrayWnd), 11, false, 0);

            int oldTrayNotifyWidth = 0;

            // If the TrayNotifyWnd updates then refresh the taskbar
            bool isHorizontalOrientation = !(taskListPos.height >= taskListPos.width);

            int trayNotifyWidth = trayNotifyPos.width;

            if (trayNotifyWidth != 0 &&
                oldTrayNotifyWidth != 0 &&
                taskListPos.left != 0)
            {

                if (trayNotifyPos.left == 3)
                {
                    break;
                }

                int pos = Math.Abs((taskListPos.left - rebarPos.left));

                Task trayFixingTask = Task.Run(() =>
                {
                    Task.Delay(5);
                    Win32.Intertop.SendMessage(reBarWindow32, 11, true, 0);
                    Task.Delay(5);
                    Win32.Intertop.SendMessage(reBarWindow32, 11, false, 0);
                    Task.Delay(5);
                }, Program.Cancellation.Token);


                do
                {
                    Win32.Api.SetHwndPosition(
                        mSTaskListWClass,
                        isHorizontalOrientation ? pos : 0,
                        isHorizontalOrientation ? 0 : pos);

                } while (!trayFixingTask.IsCompleted);
            }

            oldTrayNotifyWidth = trayNotifyWidth;

            Task.Delay(400);

        } while (true);
    }
}

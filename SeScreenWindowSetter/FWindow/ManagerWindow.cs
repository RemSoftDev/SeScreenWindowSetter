using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SeScreenWindowSetter.FWindow
{
    public class ManagerWindow
    {
        //SetWindowPos(handle, 0, 0, 0, 0, 0, ModWindow.SWP_NOZORDER | ModWindow.SWP_NOSIZE | ModWindow.SWP_SHOWWINDOW);
        //Console.WriteLine("Process Name: {0} ", process.ProcessName);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(
                IntPtr hWnd,
                int hWndInsertAfter,
                int x,
                int Y,
                int cx,
                int cy,
                int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        public static Process GetActiveWindowProcess()
        {
            Process res = null;

            Process[] processes = Process.GetProcesses();
            IntPtr activeWindow = GetForegroundWindow();

            foreach (var process in processes)
            {
                IntPtr handle = process.MainWindowHandle;

                if (!string.IsNullOrEmpty(process.MainWindowTitle) &&
                    activeWindow == process.MainWindowHandle)
                {
                    res = process;
                }
            }

            return res;
        }
    }
}

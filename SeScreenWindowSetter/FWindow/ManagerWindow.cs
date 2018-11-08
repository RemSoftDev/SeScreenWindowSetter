using System;
using System.Diagnostics;

namespace SeScreenWindowSetter.FWindow
{
    public class ManagerWindow : Win32Api
    {
        //SetWindowPos(handle, 0, 0, 0, 0, 0, ModWindow.SWP_NOZORDER | ModWindow.SWP_NOSIZE | ModWindow.SWP_SHOWWINDOW);
        //Console.WriteLine("Process Name: {0} ", process.ProcessName);

        public static Process GetActiveWindowProcess()
        {
            Process res = null;

            Process[] processes = Process.GetProcesses();
            IntPtr activeWindow = GetForegroundWindow();

            foreach (var process in processes)
            {
                IntPtr handle = process.MainWindowHandle;

                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {                  
                    Console.WriteLine(process.ProcessName);
                }

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

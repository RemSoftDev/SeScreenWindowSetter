using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SeScreenWindowSetter.FWindow
{
    public class ProcessManager : Win32Api
    {
        public const uint PROCESS_QUERY_INFORMATION = 0x400;
        public const uint PROCESS_VM_READ = 0x010;

        public static string GetProcessName(IntPtr hWnd)
        {
            string processName = string.Empty;

            uint pID;
            GetWindowThreadProcessId(hWnd, out pID);

            IntPtr proc;
            if ((proc = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, (int)pID)) == IntPtr.Zero)
            {
                return null;
            }

            processName = CallQueryFullProcessImageName(proc);

            // UWP apps are wrapped in another app called, if this has focus then try and find the child UWP process
            if (Path.GetFileName(processName).Equals("ApplicationFrameHost.exe"))
            {
                processName = UWP_AppName(hWnd, pID);
            }

            return processName;
        }

        private static string UWP_AppName(IntPtr hWnd, uint pID)
        {
            WINDOWINFO windowinfo = new WINDOWINFO();
            windowinfo.ownerpid = pID;
            windowinfo.childpid = windowinfo.ownerpid;

            IntPtr pWindowinfo = Marshal.AllocHGlobal(Marshal.SizeOf(windowinfo));

            Marshal.StructureToPtr(windowinfo, pWindowinfo, false);

            EnumWindowProc lpEnumFunc = new EnumWindowProc(EnumChildWindowsCallback);
            EnumChildWindows(hWnd, lpEnumFunc, pWindowinfo);

            windowinfo = (WINDOWINFO)Marshal.PtrToStructure(pWindowinfo, typeof(WINDOWINFO));

            IntPtr proc;
            if ((proc = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, (int)windowinfo.childpid)) == IntPtr.Zero)
            {
                return null;
            }

            var processName = CallQueryFullProcessImageName(proc);

            Marshal.FreeHGlobal(pWindowinfo);

            return processName;
        }

        public static string CallQueryFullProcessImageName(IntPtr ptr)
        {
            int capacity = 2000;
            StringBuilder sb = new StringBuilder(capacity);
            QueryFullProcessImageName(ptr, 0, sb, ref capacity);

            return sb.ToString(0, capacity);
        }

        private static bool EnumChildWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            WINDOWINFO info = (WINDOWINFO)Marshal.PtrToStructure(lParam, typeof(WINDOWINFO));

            StringBuilder outText = new StringBuilder(100);
            int a = GetWindowText(hWnd, outText, outText.Capacity);

            uint pID;
            GetWindowThreadProcessId(hWnd, out pID);

            if (pID != info.ownerpid)
            {
                info.childpid = pID;
            }

            Marshal.StructureToPtr(info, lParam, true);

            return true;
        }

        private static bool EnumWindowsProc(IntPtr hWnd, int lParam)
        {
            LDWC.Add(new DesktopWindowsCaption { HWND = hWnd, Title = GetProcessName(hWnd) });
            return true;
        }

        private static List<DesktopWindowsCaption> LDWC;

        public static List<DesktopWindowsCaption> GetDesktopWindowsCaptions()
        {
            LDWC = new List<DesktopWindowsCaption>();
            EnumDelegate enumfunc = new EnumDelegate(EnumWindowsProc);
            IntPtr hDesktop = IntPtr.Zero; // current desktop
            bool success = EnumDesktopWindows(hDesktop, enumfunc, IntPtr.Zero);

            if (success)
            {
                return LDWC;
            }
            else
            {
                string errorMessage = String.Format("EnumDesktopWindows failed with code {0}.", Marshal.GetLastWin32Error());
                throw new Exception(errorMessage);
            }
        }

        public static Func<List<DesktopWindowsCaption>, DesktopWindowsCaption, List<DesktopWindowsCaption>>
            IsProcessWindow = (l, p) =>
             {
                 if (IsWindow(p.HWND) && IsWindowVisible(p.HWND))
                 {
                     Console.WriteLine(p.Title);
                     l.Add(p);
                 }

                 return l;
             };

        public static List<DesktopWindowsCaption> GetAllProcesses() =>
            GetDesktopWindowsCaptions().Aggregate(new List<DesktopWindowsCaption>(), IsProcessWindow);
    }
}

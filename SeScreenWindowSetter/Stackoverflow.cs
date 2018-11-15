using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace App1
{

    internal struct WINDOWINFO
    {
        public uint ownerpid;
        public uint childpid;
    }

    public class UwpUtils
    {
        #region User32
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        // When you don't want the ProcessId, use this overload and pass IntPtr.Zero for the second parameter
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);
        /// <summary>
        /// Delegate for the EnumChildWindows method
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
        /// <returns>True to continue enumerating, false to bail.</returns>
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowProc lpEnumFunc, IntPtr lParam);
        #endregion

        #region Kernel32
        public const UInt32 PROCESS_QUERY_INFORMATION = 0x400;
        public const UInt32 PROCESS_VM_READ = 0x010;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool QueryFullProcessImageName([In]IntPtr hProcess, [In]int dwFlags, [Out]StringBuilder lpExeName, ref int lpdwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)]
            Boolean bInheritHandle,
            Int32 dwProcessId
        );
        #endregion

        public static string GetProcessName(IntPtr hWnd)
        {
            string processName = null;

            //hWnd = GetForegroundWindow();

            //if (hWnd == IntPtr.Zero)
            //{
            //    return null;
            //}

            uint pID;
            GetWindowThreadProcessId(hWnd, out pID);

            IntPtr proc;
            if ((proc = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, (int)pID)) == IntPtr.Zero)
            {
                return null;
            }

            int capacity = 2000;
            StringBuilder sb = new StringBuilder(capacity);
            QueryFullProcessImageName(proc, 0, sb, ref capacity);

            processName = sb.ToString(0, capacity);

            // UWP apps are wrapped in another app called, if this has focus then try and find the child UWP process
            if (Path.GetFileName(processName).Equals("ApplicationFrameHost.exe"))
            {
                processName = UWP_AppName(hWnd, pID);
            }

            return processName;
        }

        #region Get UWP Application Name

        /// <summary>
        /// Find child process for uwp apps, edge, mail, etc.
        /// </summary>
        /// <param name="hWnd">hWnd</param>
        /// <param name="pID">pID</param>
        /// <returns>The application name of the UWP.</returns>
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

            int capacity = 2000;
            StringBuilder sb = new StringBuilder(capacity);
            QueryFullProcessImageName(proc, 0, sb, ref capacity);

            Marshal.FreeHGlobal(pWindowinfo);

            return sb.ToString(0, capacity);
        }

        /// <summary>
        /// Callback for enumerating the child windows.
        /// </summary>
        /// <param name="hWnd">hWnd</param>
        /// <param name="lParam">lParam</param>
        /// <returns>always <c>true</c>.</returns>
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
        #endregion
    }

    public class EnumDesktopWindowsDemo
    {
        private const int MAXTITLE = 255;

        private static ArrayList mTitlesList;
        private static ArrayList mHwndsList;

        private delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
         ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool _EnumDesktopWindows(IntPtr hDesktop,
        EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
         ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int _GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        private static bool EnumWindowsProc(IntPtr hWnd, int lParam)
        {
            string title = GetWindowText(hWnd);
            mTitlesList.Add(title);
            mHwndsList.Add(hWnd);

            return true;
        }

        /// <summary>
        /// Returns the caption of a windows by given HWND identifier.
        /// </summary>
        public static string GetWindowText(IntPtr hWnd)
        {
            StringBuilder title = new StringBuilder(MAXTITLE);
            int titleLength = _GetWindowText(hWnd, title, title.Capacity + 1);
            title.Length = titleLength;

            return title.ToString();
        }

        /// <summary>
        /// Returns the caption of all desktop windows.
        /// </summary>
        public static IntPtr[] GetDesktopWindowsCaptions()
        {
            mTitlesList = new ArrayList();
            mHwndsList = new ArrayList();
            EnumDelegate enumfunc = new EnumDelegate(EnumWindowsProc);
            IntPtr hDesktop = IntPtr.Zero; // current desktop
            bool success = _EnumDesktopWindows(hDesktop, enumfunc, IntPtr.Zero);

            if (success)
            {
                // Copy the result to string array
                string[] titles = new string[mTitlesList.Count];
                mTitlesList.CopyTo(titles);
                IntPtr[] hwnds = new IntPtr[mHwndsList.Count];
                mHwndsList.CopyTo(hwnds);
                return hwnds;
            }
            else
            {
                // Get the last Win32 error code
                int errorCode = Marshal.GetLastWin32Error();

                string errorMessage = String.Format(
                "EnumDesktopWindows failed with code {0}.", errorCode);
                throw new Exception(errorMessage);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        public static List<IntPtr> qqqqq()
        {
            var res = new List<IntPtr>();
            var desktopWindowsCaptions = GetDesktopWindowsCaptions();
            foreach (IntPtr caption in desktopWindowsCaptions)
            {
                uint processID = 0;

                uint threadID = GetWindowThreadProcessId(caption, out processID);



                StringBuilder exePath = new StringBuilder(1024);

                //int exePathLen = GetModuleFileName((IntPtr)processID, exePath, exePath.Capacity);
                //Process p = Process.GetProcessById((int)threadID);
                var dd = UwpUtils.GetProcessName(caption);
                Console.WriteLine("*****************");
                Console.WriteLine(dd);
                Console.WriteLine(caption);
                if (dd == @"C:\Program Files\WindowsApps\Microsoft.WindowsCalculator_10.1809.2731.0_x64__8wekyb3d8bbwe\Calculator.exe"
                    || dd == @"C:\Windows\System32\ApplicationFrameHost.exe")
                {
                    var t = IsWindowVisible(caption);
                    if (t)
                    {
                        res.Add(caption); 
                    }
                }



            }
            return res;
        }
    }
}


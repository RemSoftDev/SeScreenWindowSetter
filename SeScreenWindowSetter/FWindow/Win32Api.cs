using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SeScreenWindowSetter.FWindow
{
    public class Win32Api
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
        protected static extern IntPtr SetWindowPos(
            IntPtr hWnd,
            int hWndInsertAfter,
            int x,
            int Y,
            int cx,
            int cy,
            int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        protected static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);
        [DllImport("user32.dll")]
        protected static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        protected static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool IsWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool QueryFullProcessImageName([In]IntPtr hProcess, [In]int dwFlags, [Out]StringBuilder lpExeName, ref int lpdwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)]
            Boolean bInheritHandle,
            Int32 dwProcessId
        );

        protected delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        protected static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        protected static extern int GetTopWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        protected static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        protected static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        protected delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
         ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern bool EnumDesktopWindows(IntPtr hDesktop,
        EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        protected static extern uint GetWindowModuleFileName(IntPtr hWnd, StringBuilder lpszFileName, uint cchFileNameMax);
    }
}

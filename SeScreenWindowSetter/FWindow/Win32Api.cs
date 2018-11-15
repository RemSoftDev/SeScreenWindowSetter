using System;
using System.Runtime.InteropServices;

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
        protected static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        protected static extern bool IsZoomed(IntPtr hWnd);
    }
}

using System;
using System.Runtime.InteropServices;

namespace SeScreenWindowSetter.FWindow
{
    public class Win32Api
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
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
    }
}

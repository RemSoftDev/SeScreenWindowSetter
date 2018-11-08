using System;
using System.Runtime.InteropServices;

namespace SeScreenWindowSetter.FHotkey
{
    public class Win32Api
    {
        [DllImport("User32.dll")]
        protected static extern bool RegisterHotKey(
            [In] IntPtr hWnd,
            [In] int id,
            [In] uint fsModifiers,
            [In] uint vk);

        [DllImport("User32.dll")]
        protected static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

    }
}

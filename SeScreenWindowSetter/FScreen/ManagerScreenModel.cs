using System;
using System.Runtime.InteropServices;

namespace SeScreenWindowSetter.FScreen
{
    internal class ManagerScreenModel
    {
    }

    public class MonitorInfo
    {
        public bool IsPrimary = false;
        public IntPtr hDesktop;
        public Rectangle Bounds = new Rectangle();
        public MONITORINFOEX mONITORINFOEX = new MONITORINFOEX();
    }
    public struct Rectangle
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
    public class MONITORINFOEX
    {
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
        public RECT rcMonitor = new RECT();
        public RECT rcWork = new RECT();
        public int dwFlags = 0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] szDevice = new char[32];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SeScreenWindowSetter.FScreen
{
    public static class CustomExtensions
    {
        public static Rectangle ToRectang(this RectangleWithProcesses t)
        {
            var res = new Rectangle()
            {
                Height = t.Height,
                Width = t.Width,
                X = t.X,
                Y = t.Y
            };

            return res;
        }
    }

    public class MonitorInfo
    {
        public bool IsPrimary = false;
        public int ScreenNumber;
        public IntPtr hDesktop;
        public Rectangle Bounds;
    }
    public struct Rectangle
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }
    public struct RectangleWithProcesses
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public List<FConfig.Process> Processes;
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

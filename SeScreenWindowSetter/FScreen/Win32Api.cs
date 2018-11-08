using System;
using System.Runtime.InteropServices;

namespace SeScreenWindowSetter.FScreen
{
    public class Win32Api
    {
        [DllImport("user32.dll", SetLastError = true)]
        protected static extern bool SystemParametersInfo(
            int uiAction,
            int uiParam,
            IntPtr pvParam,
            int fWinIni);

        [DllImport("user32")]
        protected static extern bool EnumDisplayMonitors(
            IntPtr hdc,
            IntPtr lpRect,
            MonitorEnumProc callback,
            int dwData);

        protected delegate bool MonitorEnumProc(
            IntPtr hDesktop,
            IntPtr hdc,
            ref RECT pRect,
            int dwData);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        protected static extern bool GetMonitorInfo(IntPtr hmonitor, [In, Out]MONITORINFOEX info);
    }
}

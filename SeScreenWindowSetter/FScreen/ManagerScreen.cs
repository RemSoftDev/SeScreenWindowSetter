using System;
using System.Collections.Generic;

namespace SeScreenWindowSetter.FScreen
{
    public class ManagerScreen : Win32Api
    {
        public static List<MonitorInfo> ActualScreens = new List<MonitorInfo>();

        public static Func<List<MonitorInfo>> Init = () =>
       {
           RefreshActualScreens();
           WorkArea();

           return ActualScreens;
       };

        public static void WorkArea()
        {
            ActualScreens.ForEach(z => WorkAreaMonitor(z));
        }

        public static Action<MonitorInfo> WorkAreaMonitor = item => GetMonitorInfo(item.hDesktop, item.mONITORINFOEX);

        public static void RefreshActualScreens()
        {
            ActualScreens.Clear();

            MonitorEnumProc callback = (IntPtr hDesktop, IntPtr hdc, ref RECT prect, int d) =>
            {
                ActualScreens.Add(new MonitorInfo()
                {
                    hDesktop = hDesktop,

                    Bounds = new Rectangle()
                    {
                        X = prect.left,
                        Y = prect.top,
                        Width = prect.right - prect.left,
                        Height = prect.bottom - prect.top,
                    },

                    IsPrimary = (prect.left == 0) && (prect.top == 0),
                });

                return true;
            };

            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, 0);
        }

    }
}

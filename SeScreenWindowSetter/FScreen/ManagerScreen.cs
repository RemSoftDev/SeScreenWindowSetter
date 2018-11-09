using System;
using System.Collections.Generic;

namespace SeScreenWindowSetter.FScreen
{
    public class ManagerScreen : Win32Api
    {
        public static List<MonitorInfo> ActualScreens = new List<MonitorInfo>();

        public static Func<List<MonitorInfo>> Init = () =>
        {
            RefreshActualScreens(callback);
            WorkArea();

            return ActualScreens;
        };

        private static void WorkArea()
        {
            for (int i = 0; i < ActualScreens.Count; i++)
            {
                WorkAreaMonitor(ActualScreens[i]);
                SetMonitorNumber(ActualScreens[i], i+1);
            }
        }

        private static Action<MonitorInfo> WorkAreaMonitor =
            item =>
        {
            MONITORINFOEX t = new MONITORINFOEX();
            GetMonitorInfo(item.hDesktop, t);

            item.Bounds = new Rectangle()
            {
                X = t.rcWork.left,
                Y = t.rcWork.top,
                Width = t.rcWork.right - t.rcWork.left,
                Height = t.rcWork.bottom - t.rcWork.top,
            };
        };

        private static Action<MonitorInfo, int> SetMonitorNumber =
            (item, n) =>
        {
            if (item.IsPrimary)
            {
                item.ScreenNumber = 0;
            }
            else
            {
                item.ScreenNumber = n;
            }
        };

        private static MonitorEnumProc callback =
            (IntPtr hDesktop, IntPtr hdc, ref RECT prect, int d) =>
        {
            ActualScreens.Add(new MonitorInfo()
            {
                hDesktop = hDesktop,
                IsPrimary = (prect.left == 0) && (prect.top == 0),
            });

            return true;
        };

        private static Action<MonitorEnumProc> RefreshActualScreens =
            (callback) =>
        {
            ActualScreens.Clear();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, 0);
        };
    }
}

using FP.SeScreenWindowSetter;
using SeScreenWindowSetter.FScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SeScreenWindowSetter.FWindow
{
    public class ManagerWindow : Win32Api
    {
        public static Action<IntPtr, Rectangle>
            SetWindowsPositionResolver =
            (h, p) =>
            {
                if (IsIconic(h))
                {
                    SetWindowPlac(h);
                }
                SetWindowsPosition(h, p);
                var vv = Marshal.GetLastWin32Error();
            };

        public static Action<IntPtr, Rectangle>
            SetWindowsPosition =
            (h, p) =>
            {
                SetWindowPos(h, ModWindow.HWND_TOPMOST, p.X, p.Y, p.Height, p.Width, ModWindow.SWP_NOZORDER | ModWindow.SWP_SHOWWINDOW);

            };

        public static Action<IntPtr>
            SetWindowPlac =
            (h) =>
            {
                WINDOWPLACEMENT placement;
                GetWindowPlacement(h, out placement);
                placement.showCmd = Win32ApiModels.SW_SHOWNORMAL;
                SetWindowPlacement(h, ref placement);
            };

        public static Action<List<RectangleWithProcesses[,]>>
            SetWindowsPositionsFromConfig = (r) =>
            {
                Console.WriteLine("****************************************************************");

                var f = GetProcessHandleByName.Curry()(GetAllWindosProcess());

                foreach (var arr in r)
                {
                    for (int i = 0; i < arr.GetLength(0); i++)
                    {
                        for (int j = 0; j < arr.GetLength(1); j++)
                        {
                            foreach (var p in arr[i, j].Processes)
                            {
                                foreach (var item in f(p.ProcessName))
                                {
                                    SetWindowsPositionResolver(item, arr[i, j].ToRectang());
                                }

                            }
                        }
                    }
                }
            };

        public static Func<List<DesktopWindowsCaption>, string, List<IntPtr>>
            GetProcessHandleByName =
            (l, n) =>
            {
                var res = new List<IntPtr>();
                var c = l.Where(z => string.Equals(z.Title, n, StringComparison.InvariantCultureIgnoreCase)).ToList();

                if (c != null)
                {
                    res.AddRange(c.Select(z => z.HWND));
                }

                foreach (var item in c)
                {
                    res.AddRange(GetApplicationFrameHost(l, item));
                }

                return res;
            };

        public static Func<List<DesktopWindowsCaption>, DesktopWindowsCaption, List<IntPtr>>
            GetApplicationFrameHost =
            (l, i) =>
            {
                var res = new List<IntPtr>();

                if (i.Title.Contains("WindowsApps"))
                {
                    var uwpProcess = l[l.IndexOf(i) + 1];
                    if (uwpProcess.Title.Contains("ApplicationFrameHost"))
                    {
                        res.Add(uwpProcess.HWND);
                    } 
                }

                return res;
            };

        public static Func<List<DesktopWindowsCaption>, DesktopWindowsCaption, List<DesktopWindowsCaption>>
            IsWindowProcess = (a, p) =>
        {
            WINDOWPLACEMENT placement;
            GetWindowPlacement(p.HWND, out placement);

            if (placement.showCmd > 0)
            {
                a.Add(p);
            }

            return a;
        };

        public static Func<List<DesktopWindowsCaption>>
            GetAllWindosProcess =
            () =>
            {
                return ProcessManager.GetAllProcesses().Aggregate(new List<DesktopWindowsCaption>(), IsWindowProcess);
            };

        public static Func<DesktopWindowsCaption>
            GetActiveWindowProcess =
            () => GetAllWindosProcess().Where(z => z.HWND == GetForegroundWindow()).FirstOrDefault();
    }
}

using FP.SeScreenWindowSetter;
using SeScreenWindowSetter.FScreen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            };

        public static Action<IntPtr, Rectangle>
            SetWindowsPosition =
            (h, p) => SetWindowPos(h, ModWindow.HWND_TOPMOST, p.X, p.Y, p.Height, p.Width, ModWindow.SWP_NOZORDER | ModWindow.SWP_SHOWWINDOW);

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
                var f = GetProcessHandleByName.Curry()(GetAllWindosProcess());

                foreach (var arr in r)
                {
                    for (int i = 0; i < arr.GetLength(0); i++)
                    {
                        for (int j = 0; j < arr.GetLength(1); j++)
                        {
                            foreach (var p in arr[i, j].Processes)
                            {
                                Console.WriteLine(p.ProcessName);
                                SetWindowsPositionResolver(f(p.ProcessName), arr[i, j].ToRectang());

                            }
                        }
                    }
                }
            };

        public static Func<List<Process>, string, IntPtr>
            GetProcessHandleByName =
            (l, n) =>
            {
                var f = l.Where(z => string.Equals(z.ProcessName, n, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                var res = f?.MainWindowHandle ?? IntPtr.Zero;
                return res;
            };

        public static Func<List<Process>>
            GetAllProcess =
            () => Process.GetProcesses().ToList();

        public static Func<List<Process>, Process, List<Process>>
            IsWindowProcess = (a, p) =>
        {
            WINDOWPLACEMENT placement;
            GetWindowPlacement(p.MainWindowHandle, out placement);

            if (placement.showCmd > 0)
            {
                a.Add(p);
            }

            return a;
        };

        public static Func<List<Process>>
            GetAllWindosProcess =
            () => GetAllProcess().Aggregate(new List<Process>(), IsWindowProcess);

        public static Func<Process>
            GetActiveWindowProcess =
            () => GetAllWindosProcess().Where(z => z.MainWindowHandle == GetForegroundWindow()).FirstOrDefault();
    }
}

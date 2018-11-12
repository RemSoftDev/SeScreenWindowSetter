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
            SetWindowsPosition =
            (h, p) => SetWindowPos(h, 0, p.X, p.Y, p.Height, p.Width, ModWindow.SWP_SHOWWINDOW);

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
                                SetWindowsPosition(f(p.ProcessName), arr[i, j].ToRectang());
                            }
                        }
                    }
                }
            };

        public static Func<List<Process>, string, IntPtr>
            GetProcessHandleByName =
            (l, n) =>
            {
                var f = l.Where(z => string.Equals(z.ProcessName,n, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                return f.MainWindowHandle;
            };

        public static Func<List<Process>>
            GetAllProcess =
            () => Process.GetProcesses().ToList();

        public static Func<List<Process>, Process, List<Process>>
            IsWindowProcess = (a, p) =>
        {
            if (!string.IsNullOrEmpty(p.MainWindowTitle))
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

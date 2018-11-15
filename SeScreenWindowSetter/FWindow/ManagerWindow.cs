using App1;
using FP.SeScreenWindowSetter;
using SeScreenWindowSetter.FScreen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace SeScreenWindowSetter.FWindow
{
    public class ManagerWindow : Win32Api
    {


        private const int SW_MAXIMIZE = 3;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

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
                //ShowWindow(h, SW_MAXIMIZE);
        
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
                var hy = EnumDesktopWindowsDemo.qqqqq();
                var res = new Rectangle()
                {
                    Height = 1000,
                    Width = 1000,
                    X = 0,
                    Y = 0
                };
                foreach (var item in hy)
                {
                    SetWindowsPositionResolver(item, res);

                }
                //foreach (var arr in r)
                //{
                //    for (int i = 0; i < arr.GetLength(0); i++)
                //    {
                //        for (int j = 0; j < arr.GetLength(1); j++)
                //        {
                //            foreach (var p in arr[i, j].Processes)
                //            {
                //                Console.WriteLine(p.ProcessName);
                //                SetWindowsPositionResolver(f(p.ProcessName), arr[i, j].ToRectang());

                //            }
                //        }
                //    }
                //}
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
            () =>
            {
                return Process.GetProcesses().ToList();
                //return EnumDesktopWindowsDemo.GetDesktopWindowsCaptions().ToList();
            };

        public static Func<List<Process>, Process, List<Process>>
            IsWindowProcess = (a, p) =>
        {
            WINDOWPLACEMENT placement;
            GetWindowPlacement(p.MainWindowHandle, out placement);

            if (placement.showCmd > 0)
            {
                a.Add(p);
            }

            if (p.ProcessName == "ApplicationFrameHost")
            {
                var tt = UwpUtils.GetProcessName(p.MainWindowHandle);
            }

            Console.WriteLine("*******");
            Console.WriteLine(p.ProcessName);
            Console.WriteLine(p.MainWindowTitle);
            Console.WriteLine(p.MainWindowHandle.ToInt32());

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

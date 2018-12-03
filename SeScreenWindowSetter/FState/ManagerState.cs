using FP.SeScreenWindowSetter;
using SeScreenWindowSetter.FP;
using SeScreenWindowSetter.FScreen;
using SeScreenWindowSetter.FWindow;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeScreenWindowSetter.FState
{
    public static class ManagerState
    {
        private static Func<StateModel, IEnumerable<StateModel>>
            ProcessScreens = (s) => s.Screens.Select(x => s.With(z => z.Screen = x));

        private static Func<StateModel, IEnumerable<StateModel>>
            ProcessGridTypes = (s) => s.Screen.GridTypes.Select(x => s.With(z => z.GridType = x));

        private static Func<StateModel, IEnumerable<StateModel>>
            ProcessPositions = (s) => s.GridType.Positions.Select(x => s.With(z => z.Position = x));

        private static Func<StateModel, IEnumerable<StateModel>>
            ProcessProcesses = (s) => s.Position.Processes.Select(x => s.With(z => z.Process = x));

        public static Func<StateModel, Maybe<IEnumerable<StateModel>>>
            InitFromConfig = (m) =>
        {
            var r = m.PipeForward(ProcessScreens).ReturnMaybe();

            return r;
        };

        public static Func<IEnumerable<StateModel>, Maybe<IEnumerable<StateModel>>>
          InitFlatStructure = (l) =>
        {
            var r = l.
                    SelectMany(ProcessGridTypes).
                    SelectMany(ProcessPositions).
                    SelectMany(ProcessProcesses).
                    ReturnMaybe();

            return r;
        };

        public static Func<List<MonitorInfo>, IEnumerable<StateModel>, Maybe<IEnumerable<StateModel>>>
            InitFromScreens = (m, s) =>
        {
            var r = m.Where(x => x.IsPrimary == true).First().
                    PipeForward(ProcessMonitorInfo.Curry()(m)).
                    PipeForward(s.Select)
                    ;
            var f = r.ToList();
            return r.ReturnMaybe();
        };

        private static Func<List<MonitorInfo>, MonitorInfo, StateModel, StateModel>
            ProcessMonitorInfo = (m, d, s) =>
        {
            s.MonitorInfo = m.Where(z => z.ScreenNumber == s.Screen.ScreenNumber).
                              DefaultIfEmpty(d).
                              First();
            return s;
        };

        private static Func<List<DesktopWindowsCaption>, StateModel, StateModel>
            LinkHwndAndProcessFromConfig = (w, s) =>
        {
            var hwnd = w.Where(z => z.Title == s.Process.ProcessName).FirstOrDefault()?.HWND;

            if (hwnd != null)
            {
                s.hProcess = (IntPtr)hwnd;
            }

            return s;
        };

        public static Func<List<DesktopWindowsCaption>, IEnumerable<StateModel>, Maybe<IEnumerable<StateModel>>>
            InitFromWindowProcesses = (d, s) =>
        {
            var r = LinkHwndAndProcessFromConfig.Curry()(d).
                    PipeForward(s.Select).
                    ReturnMaybe();

            return r;
        };


        public static Func<IGrouping<string, StateModel>, IEnumerable<StateModel>>
            SetParts = (m) =>
        {
            var r = m.Select(z => z.PipeForward(ScreenGridChek).
                                    PipeForward(SetPartsWidthAndHight));

            return r;
        };

        public static Func<StateModel, StateModel>
            ScreenGridChek = (s) =>
            {
                var type = s.GridType.TypeTitle;
                var dict = s.ScreenGridConverter;

                s.ScreenGridDimension = dict.ContainsKey(type) ? dict[type] : dict.First().Value;
                return s;
            };

        public static Func<StateModel, StateModel>
            SetPartsWidthAndHight = (s) =>
            {
                s.PH = s.LenghtSplitFunctionResolver[s.ScreenGridDimension.Item1];
                s.PW = s.LenghtSplitFunctionResolver[s.ScreenGridDimension.Item2];

                return s;
            };

        public static Func<IEnumerable<StateModel>, Maybe<IEnumerable<StateModel>>>
            InitParts = (s) =>
        {
            var r = s.ToLookup(z => z.GridType.TypeTitle).SelectMany(SetParts);

            return r.ReturnMaybe();
        };

        public static Func<IEnumerable<StateModel>, Maybe<IEnumerable<StateModel>>>
           SetNewCoordinates = (l) =>
        {
            var f = l.ToList();
            var r = l.Select(CalculateCoordinates);
            var f1 = r.ToList();
            return r.ReturnMaybe();
        };

        public static Func<StateModel, StateModel>
            CalculateCoordinates = (s) =>
        {
            var pw = s.PH(s.MonitorInfo.Bounds.Width);
            var ph = s.PW(s.MonitorInfo.Bounds.Height);

            s.X = pw * s.Position.Column;
            s.Y = ph * s.Position.Raw;
            s.Width = ph;
            s.Height = pw;

            return s;
        };
    }
}

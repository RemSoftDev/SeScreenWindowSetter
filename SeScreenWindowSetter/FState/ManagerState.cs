﻿using FP.SeScreenWindowSetter;
using SeScreenWindowSetter.FConfig;
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
        public static Func<Screen, StateModel>
            ProcessScreens = (s) => new StateModel() { Screen = s };

        public static Func<StateModel, IEnumerable<StateModel>>
            ProcessGridTypes = (s) => s.Screen.GridTypes.Select(x => s.With(z => z.GridType = x));

        public static Func<StateModel, IEnumerable<StateModel>>
            ProcessPositions = (s) => s.GridType.Positions.Select(x => s.With(z => z.Position = x));

        public static Func<StateModel, IEnumerable<StateModel>>
            ProcessProcesses = (s) => s.Position.Processes.Select(x => s.With(z => z.Process = x));

        public static Func<ManagerConfigModel, Maybe<IEnumerable<StateModel>>>
            InitFromConfig = (m) =>
        {
            var t = m.Screens.Select(ProcessScreens).ReturnMaybe();
            //SelectMany(ProcessGridTypes).
            //SelectMany(ProcessPositions).
            //SelectMany(ProcessProcesses);

            return t;
        };

        public static Func<List<MonitorInfo>, IEnumerable<StateModel>, Maybe<IEnumerable<StateModel>>>
            InitFromScreens = (m, s) =>
        {
            var t = m.Where(x => x.IsPrimary == true).First().
                    PipeForward(ProcessMonitorInfo.Curry()(m)).
                    PipeForward(s.Select).
                    ReturnMaybe();

            return t;
        };

        public static Func<List<MonitorInfo>, MonitorInfo, StateModel, StateModel>
            ProcessMonitorInfo = (m, d, s) =>
        {
            s.MonitorInfo = m.Where(z => z.ScreenNumber == s.Screen.ScreenNumber).
                              DefaultIfEmpty(d).
                              First();
            return s;
        };

        public static Func<List<DesktopWindowsCaption>, IEnumerable<StateModel>, Maybe<IEnumerable<StateModel>>>
            InitFromWindowProcesses = (p, s) =>
        {


            return s.ReturnMaybe();
        };

    }
}

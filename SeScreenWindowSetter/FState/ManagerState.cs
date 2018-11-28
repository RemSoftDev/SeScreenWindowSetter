using FP.SeScreenWindowSetter;
using SeScreenWindowSetter.FConfig;
using SeScreenWindowSetter.FP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeScreenWindowSetter.FState
{
    public static class ManagerState
    {
        public static Func<ManagerConfigModel, Maybe<StateModel>> Init = (m) =>
        {
            var t = m.Screens.Select(ProcessScreens).SelectMany(ProcessGridTypes).ToList();
            return new Nothing<StateModel>();
        };

        public static Func<Screen, StateModel> ProcessScreens = (s) =>
        {
            return new StateModel() { Screen = s, hDesktop = s.hDesktop };
        };

        public static Func<StateModel, List<StateModel>> ProcessGridTypes = (s) =>
        {
            var res = new List<StateModel>();
            var t = s.Screen.Types.Select(x=>s.With(z => z.Type = x));
            res.AddRange(t);

            return res;
        };
    }
}

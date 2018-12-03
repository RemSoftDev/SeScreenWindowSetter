using FP.SeScreenWindowSetter;
using SeScreenWindowSetter.FConfig;
using SeScreenWindowSetter.FP;
using SeScreenWindowSetter.FScreen;
using System;

namespace SeScreenWindowSetter.FState
{
    public class SetupState
    {
        public static Func<MonitorInfo, GridType, PositionBlockState>
            Init = (mi, c) =>
        {
            var t = new PositionBlockState().
                    PipeForward(InitScreenGridConverter).
                    PipeForward(InitLenghtSplitFunctionResolver);
            t.Config = c;
            t.MonitorInfo = mi;

            return t;
        };

        private static Func<PositionBlockState, PositionBlockState>
        InitScreenGridConverter = s =>
        {
            s.ScreenGridConverter.Add("2_Horisontal", (1, 2));
            s.ScreenGridConverter.Add("2_Vertical", (2, 1));
            s.ScreenGridConverter.Add("4", (2, 2));

            return s;
        };

        private static Func<PositionBlockState, PositionBlockState>
        InitLenghtSplitFunctionResolver = s =>
        {
            s.LenghtSplitFunctionResolver.Add(1, GetLenghtSplit1);
            s.LenghtSplitFunctionResolver.Add(2, GetLenghtSplit2);
            s.LenghtSplitFunctionResolver.Add(3, GetLenghtSplit3);

            return s;
        };

        public static Func<ManagerConfigModel, Maybe<StateModel>>
        InitNew = (m) =>
        {
            var t = new StateModel().
                            PipeForward(InitScreenGridConverter1).
                            PipeForward(InitLenghtSplitFunctionResolver1);

            t.Screens = m.Screens;

            return t.ReturnMaybe();
        };


        private static Func<StateModel, StateModel>
        InitScreenGridConverter1 = s =>
        {
            s.ScreenGridConverter.Add("2_Horisontal", (1, 2));
            s.ScreenGridConverter.Add("2_Vertical", (2, 1));
            s.ScreenGridConverter.Add("4", (2, 2));

            return s;
        };

        private static Func<StateModel, StateModel>
        InitLenghtSplitFunctionResolver1 = s =>
        {
            s.LenghtSplitFunctionResolver.Add(1, GetLenghtSplit1);
            s.LenghtSplitFunctionResolver.Add(2, GetLenghtSplit2);
            s.LenghtSplitFunctionResolver.Add(3, GetLenghtSplit3);

            return s;
        };

        public static Func<int, int>
        GetLenghtSplit1 = (x) => x;

        public static Func<int, int>
        GetLenghtSplit2 = (x) => x / 2;

        public static Func<int, int>
        GetLenghtSplit3 = (x) =>
        {
            var res = (int)Math.Floor((double)x / 3);
            return res;
        };
    }
}

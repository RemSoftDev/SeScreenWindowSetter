using FP.SeScreenWindowSetter;
using System;
using System.Collections.Generic;

namespace SeScreenWindowSetter.FScreen
{
    public class SetupState
    {
        public static Func<PositionBlockState>
            Init =
            () => new PositionBlockState().
                    PipeForward(InitScreenGridConverter).
                    PipeForward(InitLenghtSplitFunctionResolver);

        public static Func<MonitorInfo, FConfig.Type, PositionBlockState>
            Init1 = (mi, c) =>
        {
            var t = Init();
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

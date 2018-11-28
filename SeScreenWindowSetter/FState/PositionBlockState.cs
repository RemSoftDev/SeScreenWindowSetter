using SeScreenWindowSetter.FConfig;
using System;
using System.Collections.Generic;

namespace SeScreenWindowSetter.FState
{
    public class PositionBlockState
    {
        public FScreen.MonitorInfo MonitorInfo;
        public GridType Config;

        public (int, int) ScreenGridDimension;
        public Func<int, int> PW;
        public Func<int, int> PH;

        public FScreen.RectangleWithProcesses[,] ScreenParts;

        public Dictionary<string, (int, int)> ScreenGridConverter = new Dictionary<string, (int, int)>();
        public Dictionary<int, Func<int, int>> LenghtSplitFunctionResolver = new Dictionary<int, Func<int, int>>();
    }
}

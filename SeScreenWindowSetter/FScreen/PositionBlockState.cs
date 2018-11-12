using System;
using System.Collections.Generic;

namespace SeScreenWindowSetter.FScreen
{
    public class PositionBlockState
    {
        public MonitorInfo MonitorInfo;
        public FConfig.Type Config;

        public (int, int) ScreenGridDimension;
        public Func<int, int> PW;
        public Func<int, int> PH;

        public RectangleWithProcesses[,] ScreenParts;

        public Dictionary<string, (int, int)> ScreenGridConverter = new Dictionary<string, (int, int)>();
        public Dictionary<int, Func<int, int>> LenghtSplitFunctionResolver = new Dictionary<int, Func<int, int>>();
    }
}

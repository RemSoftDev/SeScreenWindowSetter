using SeScreenWindowSetter.FConfig;
using SeScreenWindowSetter.FScreen;
using System;
using System.Collections.Generic;

namespace SeScreenWindowSetter.FState
{
    public class StateModel
    {
        public StateModel()
        {

        }
        public StateModel(StateModel s)
        {
            Screen = s.Screen;
            GridType = s.GridType;
            Position = s.Position;
            Process = s.Process;
            MonitorInfo = s.MonitorInfo;
            ScreenGridConverter = s.ScreenGridConverter;
            LenghtSplitFunctionResolver = s.LenghtSplitFunctionResolver;
        }

        public IntPtr hProcess;

        public int X;
        public int Y;
        public int Width;
        public int Height;

        public List<Screen> Screens;
        public Screen Screen;
        public GridType GridType;
        public Position Position;
        public Process Process;

        public MonitorInfo MonitorInfo;

        public (int, int) ScreenGridDimension;
        public Func<int, int> PW;
        public Func<int, int> PH;
        public Dictionary<string, (int, int)> ScreenGridConverter = new Dictionary<string, (int, int)>();
        public Dictionary<int, Func<int, int>> LenghtSplitFunctionResolver = new Dictionary<int, Func<int, int>>();

    }
}

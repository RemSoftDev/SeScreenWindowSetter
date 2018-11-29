using SeScreenWindowSetter.FConfig;
using SeScreenWindowSetter.FScreen;
using System;

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
        }

        public IntPtr hProcess;

        public int X;
        public int Y;
        public int Width;
        public int Height;
        
        public Screen Screen;
        public GridType GridType;
        public Position Position;
        public Process Process;

        public MonitorInfo MonitorInfo;
    }
}

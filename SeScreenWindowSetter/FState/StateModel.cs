using SeScreenWindowSetter.FConfig;
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
            Types = s.Types;
            Type = s.Type;
            Positions = s.Positions;
            Position = s.Position;
            Processes = s.Processes;
        }

        public IntPtr hDesktop;

        public int X;
        public int Y;
        public int Width;
        public int Height;

        public IntPtr hProcess;

        public Screen Screen;

        public List<GridType> Types;
        public GridType Type;

        public List<Position> Positions;
        public Position Position;

        public List<Process> Processes;

    }
}

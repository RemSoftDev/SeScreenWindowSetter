namespace SeScreenWindowSetter.FConfig
{
    public class ManagerConfigModel
    {
        public Screen[] Screens { get; set; }
    }

    public class Screen
    {
        public int ScreenNumber { get; set; }
        public Windowposition[] WindowPositions { get; set; }
    }

    public class Windowposition
    {
        public Type[] Types { get; set; }
    }

    public class Type
    {
        public string TypeTitle { get; set; }
        public Position[] Positions { get; set; }
    }

    public class Position
    {
        public string PositionTitle { get; set; }
        public string[] ProcessNames { get; set; }
    }

}

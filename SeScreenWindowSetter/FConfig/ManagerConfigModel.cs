using System.Collections.Generic;

namespace SeScreenWindowSetter.FConfig
{
    public class ManagerConfigModel
    {
        public List<ManagerConfigScreenModel> Screens { get; set; }
    }

    public class ManagerConfigScreenModel
    {
        public string ScreenNumber { get; set; }
        public List<ManagerConfigpositionModel> WindowPositions { get; set; }
    }

    public class ManagerConfigpositionModel
    {
        public string Position { get; set; }
        public string[] ProcessNames { get; set; }
    }
}

using Newtonsoft.Json;
using System.IO;

namespace SeScreenWindowSetter.FConfig
{
    public static class ManagerConfig
    {
        private static string ConfigPath = @"C:\Users\oleksandr.dubyna\Documents\GIT\SE\SeScreenWindowSetter\SeScreenWindowSetter\managerScreen.json";

        public static ManagerConfigModel Init()
        {
            var res = new ManagerConfigModel();

            if (PathExsist(ConfigPath))
            {
                var json = GetJson(ConfigPath);
                res = DeserilizeJson(json);
            }

            return res;
        }

        private static ManagerConfigModel DeserilizeJson(string json)
        {
            ManagerConfigModel res = new ManagerConfigModel();
            res = JsonConvert.DeserializeObject<ManagerConfigModel>(json);
            return res;
        }

        private static bool PathExsist(string path)
        {
            var res = File.Exists(path);
            return res;
        }

        private static string GetJson(string path)
        {
            var res = string.Empty;
            res = File.ReadAllText(path);
            return res;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.IO;
//private static string ConfigPath = @"C:\Users\oleksandr.dubyna\Documents\GIT\SE\SeScreenWindowSetter\SeScreenWindowSetter\managerScreen.json";
namespace SeScreenWindowSetter.FConfig
{
    public static class ManagerConfig
    {
        public static Func<string, ManagerConfigModel> Init = 
            (path) =>
        {
            var res = new ManagerConfigModel();

            if (PathExsist(path))
            {
                var json = GetJson(path);
                res = DeserilizeJson(json);
            }

            return res;
        };

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

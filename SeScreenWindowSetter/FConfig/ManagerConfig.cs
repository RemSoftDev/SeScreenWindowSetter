using Newtonsoft.Json;
using System.IO;

namespace SeScreenWindowSetter.FConfig
{
    public class ManagerConfig
    {
        private string ConfigPath = @"C:\managerScreen.json";
        public ManagerConfig()
        {

        }
        public void Start()
        {
            if (!PathExsist(ConfigPath))
            {
                return;
            }

            var json = GetJson(ConfigPath);
            var obj = DeserilizeJson(json);
        }

        private ManagerConfigModel DeserilizeJson(string json)
        {
            ManagerConfigModel res = new ManagerConfigModel();
            res = JsonConvert.DeserializeObject<ManagerConfigModel>(json);
            return res;
        }

        private bool PathExsist(string path)
        {
            var res = File.Exists(path);
            return res;
        }

        private string GetJson(string path)
        {
            var res = string.Empty;
            res = File.ReadAllText(path);
            return res;
        }
    }
}

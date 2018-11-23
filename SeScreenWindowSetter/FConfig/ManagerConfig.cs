using Newtonsoft.Json;
using SeScreenWindowSetter.FP;
using System;
using System.IO;

namespace SeScreenWindowSetter.FConfig
{
    public static class ManagerConfig
    {
        public static Func<string, Maybe<ManagerConfigModel>> Init =
            (path) =>
                path.
                ReturnMaybe().
                Bind(IsPathExists).
                Bind(GetJson).
                Bind(DeserilizeJson);

        private static Func<string, Maybe<ManagerConfigModel>> DeserilizeJson = (v) =>
            JsonConvert.DeserializeObject<ManagerConfigModel>(v).ReturnMaybe();

        private static Func<string, Maybe<string>> IsPathExists = (v) =>
            File.Exists(v) ? v.ReturnMaybe() : new Nothing<string>();

        private static Func<string, Maybe<string>> GetJson = (v) =>
            File.ReadAllText(v).ReturnMaybe();
    }
}

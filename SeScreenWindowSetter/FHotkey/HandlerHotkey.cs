using SeScreenWindowSetter.FWindow;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SeScreenWindowSetter.FHotkey
{
    public class HandlerHotkey
    {
        public Dictionary<int, ModelHotkey> SetupHandlers()
        {
            Dictionary<int, ModelHotkey> collection = new Dictionary<int, ModelHotkey>();
            collection.Add(9000, new ModelHotkey(ModHotkey.MOD_CONTROL | ModHotkey.MOD_SHIFT, Key.Up, Handler9000));

            return collection;
        }

        private Func<bool> Handler9000 = () =>
        {
            var ss = ManagerWindow.GetActiveWindowProcess();

            return true;
        };
    }
}

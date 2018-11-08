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
            var mod = ModHotkey.MOD_CONTROL | ModHotkey.MOD_SHIFT;
            Dictionary<int, ModelHotkey> collection = new Dictionary<int, ModelHotkey>();
            collection.Add(9000, new ModelHotkey(mod, Key.Up, Handler9000));
            collection.Add(9001, new ModelHotkey(mod, Key.Down, Handler9001));
            collection.Add(9002, new ModelHotkey(mod, Key.Left, Handler9002));
            collection.Add(9003, new ModelHotkey(mod, Key.Right, Handler9003));

            return collection;
        }

        private Func<bool> Handler9000 = () =>
        {
            var awp = ManagerWindow.GetActiveWindowProcess();

            return true;
        };

        private Func<bool> Handler9001 = () =>
        {
            var awp = ManagerWindow.GetActiveWindowProcess();

            return true;
        };

        private Func<bool> Handler9002 = () =>
        {
            var awp = ManagerWindow.GetActiveWindowProcess();

            return true;
        };

        private Func<bool> Handler9003 = () =>
        {
            var awp = ManagerWindow.GetActiveWindowProcess();

            return true;
        };
    }
}

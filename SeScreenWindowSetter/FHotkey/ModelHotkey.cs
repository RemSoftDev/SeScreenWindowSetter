using System;
using System.Windows.Input;

namespace SeScreenWindowSetter.FHotkey
{
    public class ModelHotkey
    {
        public uint Mod { get; }
        public uint Key { get; }
        public Func<bool> Handler { get; }

        public ModelHotkey(uint modHotkey, Key key, Func<bool> handler)
        {
            Mod = modHotkey;
            Key = (uint)KeyInterop.VirtualKeyFromKey(key);
            Handler = handler;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace SeScreenWindowSetter.FHotkey
{
    public class MainHotkey
    {
        public Dictionary<int, ModelHotkey> KeyCollection { get; set; }

        private HwndSource _source;
        private readonly WindowInteropHelper WIH;
        public MainHotkey(Window window)
        {
            WIH = new WindowInteropHelper(window);
        }

        public void HookAdd()
        {
            _source = HwndSource.FromHwnd(WIH.Handle);
            _source.AddHook(HwndHook);
            RegisterHotKey(KeyCollection);
        }

        public void HookRemove()
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey(KeyCollection);
        }

        private void RegisterHotKey(Dictionary<int, ModelHotkey> collection)
        {
            foreach (var item in collection)
            {
                if (!RegisterHotKey(
                    WIH.Handle,
                    item.Key,
                    item.Value.Mod,
                    item.Value.Key))
                {
                    Console.WriteLine("Error");
                }
            }
        }

        private void UnregisterHotKey(Dictionary<int, ModelHotkey> collection)
        {
            foreach (var item in collection)
            {
                UnregisterHotKey(WIH.Handle, item.Key);
            }
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case ModHotkey.WM_HOTKEY:
                    handled = ExecuteHandler(wParam.ToInt32());
                    break;
            }

            return IntPtr.Zero;
        }

        private bool ExecuteHandler(int wParam)
        {
            bool res = false;

            if (KeyCollection.ContainsKey(wParam))
            {
                KeyCollection[wParam].Handler();
                res = true;
            }

            return res;
        }

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
            [In] IntPtr hWnd,
            [In] int id,
            [In] uint fsModifiers,
            [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);
    }
}

using SeScreenWindowSetter.FHotkey;
using SeScreenWindowSetter.FScreen;
using SeScreenWindowSetter.FWindow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace SeScreenWindowSetter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainHotkey mainHotkey;
        private List<ManagerScreen.MonitorInfo> screenInfos;

        public MainWindow()
        {
            InitializeComponent();

            Dictionary<int, ModelHotkey> collection = new Dictionary<int, ModelHotkey>();
            collection.Add(9000, new ModelHotkey(ModHotkey.MOD_CONTROL | ModHotkey.MOD_SHIFT, Key.Up, Handler9000));

            mainHotkey = new MainHotkey(this);
            mainHotkey.KeyCollection = collection;

            ManagerScreen.RefreshActualScreens();
            screenInfos = ManagerScreen.ActualScreens;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            mainHotkey.HookAdd();
        }

        protected override void OnClosed(EventArgs e)
        {
            mainHotkey.HookRemove();
            base.OnClosed(e);
        }

        public Func<bool> Handler9000 = () =>
        {
            var ss = ManagerWindow.GetActiveWindowProcess();

            return true;
        };


    }
}

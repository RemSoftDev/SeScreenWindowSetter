using SeScreenWindowSetter.FConfig;
using SeScreenWindowSetter.FHotkey;
using SeScreenWindowSetter.FScreen;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SeScreenWindowSetter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainHotkey mainHotkey;
        private List<MonitorInfo> screenInfos;

        public MainWindow()
        {
            InitializeComponent();

            HandlerHotkey handlerHotkey = new HandlerHotkey();
            mainHotkey = new MainHotkey(this);
            mainHotkey.KeyCollection = handlerHotkey.SetupHandlers();

            new SplitScreen(ManagerScreen.Init, ManagerConfig.Init());
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
    }
}

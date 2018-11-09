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

            string ConfigPath = @"C:\Users\oleksandr.dubyna\Documents\GIT\SE\SeScreenWindowSetter\SeScreenWindowSetter\managerScreen.json";

            var ActualScreens = ManagerScreen.Init();
            var ManagerConfigModel = ManagerConfig.Init(ConfigPath);
            // TODO: собрать это в один неразрывный пайп лайн.
            BridgeConfigAndScreenInfo.
                Init(SetupState.Init1, ActualScreens, ManagerConfigModel).
                ForEach(z => new SetWindowsInPositionBlock(z));
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

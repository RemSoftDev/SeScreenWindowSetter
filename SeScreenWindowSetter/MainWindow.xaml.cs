using App1;
using FP.SeScreenWindowSetter;
using SeScreenWindowSetter.FConfig;
using SeScreenWindowSetter.FHotkey;
using SeScreenWindowSetter.FScreen;
using SeScreenWindowSetter.FWindow;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var t = BridgeConfigAndScreenInfo.
                     Init(SetupState.Init1, ManagerScreen.Init(), ManagerConfig.Init(ConfigPath)).
                     Aggregate(new List<RectangleWithProcesses[,]>(), SetWindowsInPositionBlock.Init);

            t.PipeForward(ManagerWindow.SetWindowsPositionsFromConfig);
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

using FP.SeScreenWindowSetter;
using SeScreenWindowSetter.FConfig;
using SeScreenWindowSetter.FHotkey;
using SeScreenWindowSetter.FScreen;
using SeScreenWindowSetter.FState;
using SeScreenWindowSetter.FWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

            FillListOfProcess();

            List<Employee> li = new List<Employee>();
            li.Add(new Employee
            {
                Id = 1,
                age = 23,
                name = "Ritesh",
                gender = "M"
            });
            li.Add(new Employee
            {
                Id = 2,
                age = 23,
                name = "sujit",
                gender = "M"
            });
            li.Add(new Employee
            {
                Id = 3,
                age = 23,
                name = "Kabir",
                gender = "F"
            });
            li.Add(new Employee
            {
                Id = 4,
                age = 3,
                name = "mantu",
                gender = "F"
            });
            li.Add(new Employee
            {
                Id = 5,
                age = 24,
                name = "Kamlesh",
                gender = "M"
            });
            li.Add(new Employee
            {
                Id = 6,
                age = 28,
                name = "Manoj",
                gender = "M"
            });
            li.Add(new Employee
            {
                Id = 6,
                age = 28,
                name = "xxxx",
                gender = "M"
            });
            var res = li.ToLookup(x => x.age);
            var res1 = li.OrderBy(x => x.age);
            var t = ManagerConfig.Init(ConfigPath).
                Bind(SetupState.InitNew).
                Bind(ManagerState.InitFromConfig).
                Bind(ManagerState.InitFromScreens.Curry()(ManagerScreen.Init())).
                Bind(ManagerState.InitFlatStructure).
                Bind(ManagerState.InitFromWindowProcesses.Curry()(ProcessManager.GetAllProcesses())).
                Bind(ManagerState.InitParts).
                Bind(ManagerState.SetNewCoordinates).
                Bind(ManagerWindow.SetWindowsPositions);
            //ManagerConfig.Init(ConfigPath).
            //    Bind(ManagerScreen.Init); List<DesktopWindowsCaption>


            //BridgeConfigAndScreenInfo.
            //    Init(SetupState.Init1, ManagerScreen.Init(), ManagerConfig.Init(ConfigPath)).
            //    Aggregate(new List<RectangleWithProcesses[,]>(), SetWindowsInPositionBlock.Init).
            //    PipeForward(ManagerWindow.SetWindowsPositionsFromConfig);
        }

        private void FillListOfProcess()
        {
            listBox.ItemsSource = ProcessManager.GetAllProcesses().Select(z => z.Title).Distinct().OrderBy(z => z);
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

        private void ItemOnMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText((sender as ListBoxItem)?.Content.ToString());
        }
    }
}

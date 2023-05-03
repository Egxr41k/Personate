using Personate.MVVM.View;
using Personate.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Personate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HomeViewModel HomeVM;
        public static FontsViewModel FontsVM;
        public static ThemesViewModel ThemesVM;
        public static IconsViewModel IconsVM;
        public static WallsMenuViewModel WallsMenuVM;
        public static TaskbarViewModel TaskbarVM;
        public static CursorsMenuViewModel CursorsMenuVM;
        public static SettignsViewModel SettingsVM;
        public App()
        {
            HomeVM = new();
            FontsVM = new();
            ThemesVM = new();
            IconsVM = new();
            WallsMenuVM = new();
            TaskbarVM = new();
            CursorsMenuVM = new();
            SettingsVM = new();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}

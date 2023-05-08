using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Personate.UWP.MVVM.Model;

namespace Personate.UWP.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public static string RESOURCEPATH =
            Path.GetFullPath(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\..\Resources"));

        #region Commands
        public ICommand HomeViewCommand { get; set; }
        public ICommand FontsViewCommand { get; set; }
        public ICommand ThemesViewCommand { get; set; }
        public ICommand IconsViewCommand { get; set; }
        public ICommand WallsMenuViewCommand { get; set; }
        public ICommand TaskbarViewCommand { get; set; }
        public ICommand CursorsMenuViewCommand { get; set; }
        public ICommand SettingsViewCommand { get; set; }
        #endregion

        #region AppProps

        private object currentView;
        public object CurrentView
        {
            get => currentView;
            set => SetProperty(ref currentView, value);
        }
        

        #endregion

        private void MenuCommandsInit()
        {
            HomeViewCommand = new RelayCommand(() =>
            {
                CurrentView = App.HomeVM;
            });

            FontsViewCommand = new RelayCommand(() =>
            {
                CurrentView = App.FontsVM;
            });

            ThemesViewCommand = new RelayCommand(() => 
            {
                CurrentView = App.ThemesVM;
            });

            IconsViewCommand = new RelayCommand(() =>
            {
                CurrentView = App.IconsVM;
            });

            WallsMenuViewCommand = new RelayCommand(() =>
            {
                CurrentView = App.WallsMenuVM;
            });

            TaskbarViewCommand = new RelayCommand(() =>
            {
                CurrentView = App.TaskbarVM;
            });

            CursorsMenuViewCommand = new RelayCommand(() =>
            {
                CurrentView = App.CursorsMenuVM;
            });

            SettingsViewCommand = new RelayCommand(() => 
            {
                CurrentView = App.SettingsVM;
            });
        }

        public MainViewModel()
        {

            currentView = App.HomeVM;

            #region Commands init

            MenuCommandsInit();

            WallsMenuViewModel.UploadImageCommand = new RelayCommand(() =>
            {

                CurrentView = new WallViewModel(
                    new Wallpaper(
                        Wallpaper.OpenImage().Result));
                

            });

            //WallCardViewModel.WallViewCommand = new RelayCommand(() =>
            //{
            //    //CurrentView = new WallViewModel();
            //});

            //CursorsMenuViewModel.UploadCursorCommand = new(o =>
            //{
            //    CursorModel.OpenCursor();
            //    CurrentView = new CursorViewModel();
            //});

            //CursorCardViewModel.CursorViewCommand = new(o =>
            //{
            //    CurrentView = new CursorViewModel();
            //});
            #endregion


        }
    }
}
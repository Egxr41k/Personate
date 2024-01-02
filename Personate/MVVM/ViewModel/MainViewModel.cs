global using System;
global using System.Windows;
global using Personate.MVVM.Model;
global using System.Linq;
global using System.IO;
global using System.Windows.Controls;
global using Microsoft.Win32;

namespace Personate.MVVM.ViewModel;
class MainViewModel : Base.ViewModel
{    
    public static string RESOURCEPATH = 
        Path.GetFullPath(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\Resources"));

    #region Commands

    public Base.Command? HomeViewCommand { get; set; }
    public Base.Command? FontsViewCommand { get; set; }
    public Base.Command? ThemesViewCommand { get; set; }
    public Base.Command? IconsViewCommand { get; set; }
    public Base.Command? WallsMenuViewCommand { get; set; }
    public Base.Command? TaskbarViewCommand { get; set; }
    public Base.Command? CursorsMenuViewCommand { get; set; }
    public Base.Command? SettingsViewCommand { get; set; }
    #endregion

    #region AppProps

    private object currentView;
    public object CurrentView
    {
        get => currentView;
        set => Set(ref currentView, value);
    }

    #endregion

    private void MenuCommandsInit()
    {
        HomeViewCommand = new(o =>
        {
            CurrentView = App.HomeVM;
        });

        FontsViewCommand = new(o =>
        {
            CurrentView = App.FontsVM;
        });

        ThemesViewCommand = new(o =>
        {
            CurrentView = App.ThemesVM;
        });

        IconsViewCommand = new(o =>
        {
            CurrentView = App.IconsVM;
        });

        WallsMenuViewCommand = new(o =>
        {
            CurrentView = App.WallsMenuVM;
        });

        TaskbarViewCommand = new(o =>
        {
            CurrentView = App.TaskbarVM;
        });

        CursorsMenuViewCommand = new(o =>
        {
            CurrentView = App.CursorsMenuVM;
        });

        SettingsViewCommand = new(o =>
        {
            CurrentView = App.SettingsVM;
        });
    }

    public MainViewModel()
    {

        currentView = App.HomeVM;

        #region Commands init

        MenuCommandsInit();

        WallsMenuViewModel.UploadImageCommand = new(o =>
        {
            if(WallpaperModel.OpenImage() == true)
            {
                CurrentView = new WallViewModel(
                    new Wallpaper(WallpaperModel.path));
            }
        });

        WallCardViewModel.WallViewCommand = new(o =>
        {
            //CurrentView = new WallViewModel();
        });

        CursorsMenuViewModel.UploadCursorCommand = new(o =>
        {
            CursorModel.OpenCursor();
            CurrentView = new CursorViewModel();
        });

        CursorCardViewModel.CursorViewCommand = new(o =>
        {
            CurrentView = new CursorViewModel();
        });

        #endregion
    }
}
global using System;
global using System.Windows;
global using Personate.MVVM.Model;
global using System.Linq;
global using System.IO;
global using System.Windows.Controls;
global using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace Personate.MVVM.ViewModel;
class MainViewModel : Base.ViewModel
{    
    public static string RESOURCEPATH = 
        Path.GetFullPath(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\Resources"));
    #region Icons
    public BitmapImage MinimizeIcon { get => minimizeIcon; }
    private BitmapImage minimizeIcon;

    public BitmapImage MaximizeIcon { get => maximizeIcon; }
    private BitmapImage maximizeIcon;

    public BitmapImage CloseIcon { get => closeIcon; }
    private BitmapImage closeIcon;

    #endregion

    #region Commands
    public Base.Command? MaximizeCommand { get; set; }
    public Base.Command? MinimizeCommand { get; set; }
    public Base.Command? CloseCommand { get; set; }

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
    private static Window MainWindow
    { 
        get => Application.Current.MainWindow; 
    }

    #endregion

    private void AppCommandsInit()
    {
        MinimizeCommand = new(o =>
        {
            MainWindow.WindowState = WindowState.Minimized;
        });

        MaximizeCommand = new(o =>
        {
            if(MainWindow.WindowState == WindowState.Normal) 
                MainWindow.WindowState = WindowState.Maximized;
            else MainWindow.WindowState = WindowState.Normal;
        });

        CloseCommand = new(o => MainWindow.Close());
    }
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

    private void IconsInit()
    {
        string IconsDirectory = RESOURCEPATH + "\\Icons";
        string[] IconsFiles = Directory.GetFiles(IconsDirectory);

        closeIcon = new BitmapImage(new Uri(IconsFiles[0]));
        maximizeIcon = new BitmapImage(new Uri(IconsFiles[3]));
        minimizeIcon = new BitmapImage(new Uri(IconsFiles[4]));
    }
    public MainViewModel()
    {

        currentView = App.HomeVM;

        #region Commands init
        AppCommandsInit();
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

        IconsInit(); 
    }
}
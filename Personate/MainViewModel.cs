﻿using Personate.Modules.CursorSwitcher;
using Personate.Modules.WallpapperSwitcher;

namespace Personate;
internal class MainViewModel : ObservableObject
{
    public static string ResourcesPath =
        Path.GetFullPath(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\Resources"));

    private ObservableObject currentViewModel;
    public ObservableObject CurrentViewModel
    {
        get => currentViewModel;
        set => SetProperty(ref currentViewModel, value);
    }
    private void NavigateTo(ObservableObject target)
    {
        CurrentViewModel = target;
    }

    public RelayCommand HomeViewCommand { get; set; }
    public RelayCommand FontsViewCommand { get; set; }
    public RelayCommand ThemesViewCommand { get; set; }
    public RelayCommand IconsViewCommand { get; set; }
    public RelayCommand WallsMenuViewCommand { get; set; }
    public RelayCommand TaskbarViewCommand { get; set; }
    public RelayCommand CursorsMenuViewCommand { get; set; }
    public RelayCommand SettingsViewCommand { get; set; }


    public HomeViewModel HomeVM = new();
    //public FontsViewModel FontsVM = new();
    //public ThemesViewModel ThemesVM = new();
    //public IconsViewModel IconsVM = new();
    public WallsMenuViewModel WallsMenuVM = new();
    //public TaskbarViewModel TaskbarVM = new();
    public CursorsMenuViewModel CursorsMenuVM = new();
    public SettignsViewModel SettingsVM = new();

    private void MenuCommandsInit()
    {
        HomeViewCommand = new(() => NavigateTo(HomeVM));
        //FontsViewCommand = new(() => NavigateTo(FontsVM);
        //ThemesViewCommand = new(() => NavigateTo(ThemesVM);
        //IconsViewCommand = new(() => NavigateTo(IconsVM);
        WallsMenuViewCommand = new(() => NavigateTo(WallsMenuVM));
        //TaskbarViewCommand = new(() => NavigateTo(TaskbarVM);
        CursorsMenuViewCommand = new(() => NavigateTo(CursorsMenuVM));
        SettingsViewCommand = new(() => NavigateTo(SettingsVM));
    }

    public MainViewModel()
    {
        CurrentViewModel = HomeVM;

        MenuCommandsInit();

        WallsMenuVM.UploadImageCommand = new(() =>
        {
            var wallViewModel = new WallViewModel(new Wallpaper());
            NavigateTo(wallViewModel);        
        });

        CursorsMenuVM.UploadCursorCommand = new(() =>
        {
            Cursor.OpenCursor();
            NavigateTo(new CursorViewModel());
        });

    }
}
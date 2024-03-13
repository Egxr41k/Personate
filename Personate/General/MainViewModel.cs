using Cursors = Personate.Modules.CursorSwitcher;
using Home = Personate.Modules.Home;
using Settings = Personate.Modules.Settings;
using Wallpapers = Personate.Modules.WallpaperSwitcher;
using Taskbar = Personate.Modules.TBStyler;

namespace Personate.General;
internal class MainViewModel : ObservableObject
{
    public static string PersonateLibPath =
        Path.GetFullPath(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\PersonateLib"));

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
    public RelayCommand WallpapersCommand { get; set; }
    public RelayCommand TaskbarViewCommand { get; set; }
    public RelayCommand CursorsCommand { get; set; }
    public RelayCommand SettingsViewCommand { get; set; }


    public Home.HomeViewModel HomeVM = new();
    //public FontsViewModel FontsVM = new();
    //public ThemesViewModel ThemesVM = new();
    //public IconsViewModel IconsVM = new();
    public Wallpapers.ViewModels.MenuViewModel WallpapersMenu = new();
    public Taskbar.ViewModels.MenuViewModel TaskbarMenu = new();
    public Cursors.ViewModels.MenuViewModel CursorsMenu = new();
    //public Settings.SettignsViewModel SettingsVM = new();

    private void MenuCommandsInit()
    {
        HomeViewCommand = new(() => NavigateTo(HomeVM));
        //FontsViewCommand = new(() => NavigateTo(FontsVM);
        //ThemesViewCommand = new(() => NavigateTo(ThemesVM);
        //IconsViewCommand = new(() => NavigateTo(IconsVM);
        WallpapersCommand = new(() => NavigateTo(WallpapersMenu));
        TaskbarViewCommand = new(() => NavigateTo(TaskbarMenu));
        CursorsCommand = new(() => NavigateTo(CursorsMenu));
        //SettingsViewCommand = new(() => NavigateTo(SettingsVM));
    }

    public MainViewModel()
    {
        CurrentViewModel = HomeVM;

        MenuCommandsInit();

        WallpapersMenu.UploadCommand = new(() =>
        {
            Wallpapers.Models.Model wallpaper = Wallpapers.Models.Model.FromFileExplorer();
            Wallpapers.ViewModels.DetailsViewModel wallpaperDetails = new(wallpaper);
            NavigateTo(wallpaperDetails);
        });

        WallpapersMenu.ToDetailsCommand = new(() =>
        {
            Wallpapers.Models.Model wallpaper = WallpapersMenu.SelectedItem.Wallpaper;
            Wallpapers.ViewModels.DetailsViewModel wallpaperDetails = new(wallpaper);
            NavigateTo(wallpaperDetails);
        });

        CursorsMenu.UploadCommand = new(() =>
        {
            Cursors.Models.Model cursor = Cursors.Models.Model.FromFileExplorer();
            Cursors.ViewModels.DetailsViewModel cursorDetails = new(cursor);
            NavigateTo(cursorDetails);
        });

        CursorsMenu.DetailsViewCommand = new(() =>
        {
            Cursors.Models.Model cursor = CursorsMenu.SelectedItem.Cursor;
            Cursors.ViewModels.DetailsViewModel cursorDetails = new(cursor);
            NavigateTo(cursorDetails);
        });

    }
}
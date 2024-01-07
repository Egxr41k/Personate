using Cursors = Personate.Modules.CursorSwitcher;
using Home = Personate.Modules.Home;
using Settings = Personate.Modules.Settings;
using Wallpapers = Personate.Modules.WallpaperSwitcher;

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
    public Wallpapers.MenuViewModel WallpapersMenu = new();
    //public TaskbarViewModel TaskbarVM = new();
    public Cursors.MenuViewModel CursorsMenu = new();
    public Settings.SettignsViewModel SettingsVM = new();

    private void MenuCommandsInit()
    {
        HomeViewCommand = new(() => NavigateTo(HomeVM));
        //FontsViewCommand = new(() => NavigateTo(FontsVM);
        //ThemesViewCommand = new(() => NavigateTo(ThemesVM);
        //IconsViewCommand = new(() => NavigateTo(IconsVM);
        WallpapersCommand = new(() => NavigateTo(WallpapersMenu));
        //TaskbarViewCommand = new(() => NavigateTo(TaskbarVM);
        CursorsCommand = new(() => NavigateTo(CursorsMenu));
        SettingsViewCommand = new(() => NavigateTo(SettingsVM));
    }

    public MainViewModel()
    {
        CurrentViewModel = HomeVM;

        MenuCommandsInit();

        WallpapersMenu.UploadCommand = new(() =>
        {
            Wallpapers.Model wallpaper = new(null);
            Wallpapers.DetailsViewModel wallpaperDetails = new(wallpaper);
            NavigateTo(wallpaperDetails);
        });

        WallpapersMenu.ToDetailsCommand = new(() =>
        {
            Wallpapers.Model wallpaper = WallpapersMenu.SelectedItem.Wallpaper;
            Wallpapers.DetailsViewModel wallpaperDetails = new(wallpaper);
            NavigateTo(wallpaperDetails);
        });

        CursorsMenu.UploadCommand = new(() =>
        {
            Cursors.Model cursor = new(null);
            Cursors.DetailsViewModel cursorDetails = new(cursor);
            NavigateTo(cursorDetails);
        });

        CursorsMenu.DetailsViewCommand = new(() =>
        {
            Cursors.Model cursor = CursorsMenu.SelectedItem.Cursor;
            Cursors.DetailsViewModel cursorDetails = new(cursor);
            NavigateTo(cursorDetails);
        });

    }
}
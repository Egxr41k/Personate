using Personate.Modules.CursorSwitcher;
using Personate.Modules.Home;
using Personate.Modules.Settings;
using Personate.Modules.WallpapperSwitcher;

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
    public RelayCommand WallpappersMenuCommand { get; set; }
    public RelayCommand TaskbarViewCommand { get; set; }
    public RelayCommand CursorsMenuCommand { get; set; }
    public RelayCommand SettingsViewCommand { get; set; }


    public HomeViewModel HomeVM = new();
    //public FontsViewModel FontsVM = new();
    //public ThemesViewModel ThemesVM = new();
    //public IconsViewModel IconsVM = new();
    public Modules.WallpapperSwitcher.MenuViewModel WallpappersMenu = new();
    //public TaskbarViewModel TaskbarVM = new();
    public Modules.CursorSwitcher.MenuViewModel CursorsMenu = new();
    public SettignsViewModel SettingsVM = new();

    private void MenuCommandsInit()
    {
        HomeViewCommand = new(() => NavigateTo(HomeVM));
        //FontsViewCommand = new(() => NavigateTo(FontsVM);
        //ThemesViewCommand = new(() => NavigateTo(ThemesVM);
        //IconsViewCommand = new(() => NavigateTo(IconsVM);
        WallpappersMenuCommand = new(() => NavigateTo(WallpappersMenu));
        //TaskbarViewCommand = new(() => NavigateTo(TaskbarVM);
        CursorsMenuCommand = new(() => NavigateTo(CursorsMenu));
        SettingsViewCommand = new(() => NavigateTo(SettingsVM));
    }

    public MainViewModel()
    {
        CurrentViewModel = HomeVM;

        MenuCommandsInit();

        WallpappersMenu.UploadCommand = new(() =>
        {
            Modules.WallpapperSwitcher.Model wallpaper = new(null);
            Modules.WallpapperSwitcher.DetailsViewModel wallViewModel = new(wallpaper);
            NavigateTo(wallViewModel);
        });

        WallpappersMenu.ToDetailsCommand = new(() =>
        {
            Modules.WallpapperSwitcher.Model wallpaper = WallpappersMenu.SelectedItem.Wallpaper;
            Modules.WallpapperSwitcher.DetailsViewModel wallViewModel = new(wallpaper);
            NavigateTo(wallViewModel);
        });

        CursorsMenu.UploadCommand = new(() =>
        {
            Modules.CursorSwitcher.Model cursor = new(null);
            Modules.CursorSwitcher.DetailsViewModel cursorViewModel = new(cursor);
            NavigateTo(cursorViewModel);
        });

        CursorsMenu.DetailsViewCommand = new(() =>
        {
            Modules.CursorSwitcher.Model cursor = CursorsMenu.SelectedItem.Cursor;
            Modules.CursorSwitcher.DetailsViewModel cursorViewModel = new(cursor);
            NavigateTo(cursorViewModel);
        });

    }
}
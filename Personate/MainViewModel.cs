using Personate.Modules.CursorSwitcher;
using Personate.Modules.WallpapperSwitcher;

namespace Personate;
internal class MainViewModel : ObservableObject
{
    public static string RESOURCEPATH =
        Path.GetFullPath(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\Resources"));

    #region Commands 
    public RelayCommand HomeViewCommand { get; set; }
    public RelayCommand FontsViewCommand { get; set; }
    public RelayCommand ThemesViewCommand { get; set; }
    public RelayCommand IconsViewCommand { get; set; }
    public RelayCommand WallsMenuViewCommand { get; set; }
    public RelayCommand TaskbarViewCommand { get; set; }
    public RelayCommand CursorsMenuViewCommand { get; set; }
    public RelayCommand SettingsViewCommand { get; set; }
    #endregion

    private object currentView;
    public object CurrentView
    {
        get => currentView;
        set => SetProperty(ref currentView, value);
    }

    public HomeViewModel HomeVM;
    //public FontsViewModel FontsVM;
    //public ThemesViewModel ThemesVM;
    //public IconsViewModel IconsVM;
    public WallsMenuViewModel WallsMenuVM;
    //public TaskbarViewModel TaskbarVM;
    public CursorsMenuViewModel CursorsMenuVM;
    public SettignsViewModel SettingsVM;



    private void MenuCommandsInit()
    {
        HomeViewCommand = new(() =>
        {
            CurrentView = HomeVM;
        });

        FontsViewCommand = new(() =>
        {
            //CurrentView = FontsVM;
        });

        ThemesViewCommand = new(() =>
        {
            //CurrentView = ThemesVM;
        });

        IconsViewCommand = new(() =>
        {
            //CurrentView = App.IconsVM;
        });

        WallsMenuViewCommand = new(() =>
        {
            CurrentView = WallsMenuVM;
        });

        TaskbarViewCommand = new(() =>
        {
            //CurrentView = App.TaskbarVM;
        });

        CursorsMenuViewCommand = new(() =>
        {
            CurrentView = CursorsMenuVM;
        });

        SettingsViewCommand = new(() =>
        {
            CurrentView = SettingsVM;
        });
    }

    public MainViewModel()
    {

        HomeVM = new();
        //FontsVM = new();
        //ThemesVM = new();
        //IconsVM = new();
        WallsMenuVM = new();
        //TaskbarVM = new();
        CursorsMenuVM = new();
        SettingsVM = new();


        currentView = HomeVM;

        #region Commands init

        MenuCommandsInit();

        WallsMenuViewModel.UploadImageCommand = new(() =>
        {
            if (WallpaperModel.OpenImage() == true)
            {
                CurrentView = new WallViewModel(
                    new Wallpaper(WallpaperModel.path));
            }
        });

        WallCardViewModel.WallViewCommand = new(() =>
        {
            //CurrentView = new WallViewModel();
        });

        CursorsMenuViewModel.UploadCursorCommand = new(() =>
        {
            CursorModel.OpenCursor();
            CurrentView = new CursorViewModel();
        });

        CursorCardViewModel.CursorViewCommand = new(() =>
        {
            CurrentView = new CursorViewModel();
        });

        #endregion
    }
}
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

    private ObservableObject currentViewModel;
    public ObservableObject CurrentViewModel
    {
        get => currentViewModel;
        set => SetProperty(ref currentViewModel, value);
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
            CurrentViewModel = HomeVM;
        });

        FontsViewCommand = new(() =>
        {
            //CurrentViewModel = FontsVM;
        });

        ThemesViewCommand = new(() =>
        {
            //CurrentViewModel = ThemesVM;
        });

        IconsViewCommand = new(() =>
        {
            //CurrentViewModel = App.IconsVM;
        });

        WallsMenuViewCommand = new(() =>
        {
            CurrentViewModel = WallsMenuVM;
        });

        TaskbarViewCommand = new(() =>
        {
            //CurrentViewModel = App.TaskbarVM;
        });

        CursorsMenuViewCommand = new(() =>
        {
            CurrentViewModel = CursorsMenuVM;
        });

        SettingsViewCommand = new(() =>
        {
            CurrentViewModel = SettingsVM;
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


        CurrentViewModel = HomeVM;

        #region Commands init

        MenuCommandsInit();

        WallsMenuViewModel.UploadImageCommand = new(() =>
        {
            if (WallpaperModel.OpenImage() == true)
            {
                CurrentViewModel = new WallViewModel(
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
            CurrentViewModel = new CursorViewModel();
        });

        CursorCardViewModel.CursorViewCommand = new(() =>
        {
            CurrentViewModel = new CursorViewModel();
        });

        #endregion
    }
}
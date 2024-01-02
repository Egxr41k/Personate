namespace Personate.Modules.WallpapperSwitcher;
internal class WallViewModel : ObservableObject
{
    public RelayCommand SetImageToBgCommand { get; set; }
    public RelayCommand SaveFileCommand { get; set; }
    public WallViewModel(Wallpaper wallpaper) // : base(wallpaper)
    {
        //Init();

        SetImageToBgCommand = new(() =>
        {
            WallpaperModel.SetImageToBg();
        });

        SaveFileCommand = new(() =>
        {
            WallpaperModel.SaveImage();
        });
    }
}

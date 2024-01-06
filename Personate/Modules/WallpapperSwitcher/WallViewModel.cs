namespace Personate.Modules.WallpapperSwitcher;
internal class WallViewModel : WallCardViewModel
{
    public RelayCommand SetImageToBgCommand { get; set; }
    public RelayCommand SaveFileCommand { get; set; }
    public WallViewModel(Wallpaper wallpaper) : base(wallpaper)
    {
        SetImageToBgCommand = new(() => wallpaper.Set());

        SaveFileCommand = new(() => wallpaper.Save());
    }
}

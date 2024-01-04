namespace Personate.Modules.WallpapperSwitcher;
internal class WallViewModel : ObservableObject
{
    public RelayCommand SetImageToBgCommand { get; set; }
    public RelayCommand SaveFileCommand { get; set; }
    public WallViewModel(Wallpaper wallpaper)
    {
        SetImageToBgCommand = new(() => wallpaper.SetToBg());

        SaveFileCommand = new(() => wallpaper.Save());
    }
}

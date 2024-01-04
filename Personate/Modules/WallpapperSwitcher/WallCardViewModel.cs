using System.Windows.Media.Imaging;

namespace Personate.Modules.WallpapperSwitcher;
internal class WallCardViewModel : ObservableObject
{
    private Wallpaper wallpaper;
    public RelayCommand WallViewCommand { get; set; }
    public string Resolution => wallpaper.resolution;
    public BitmapImage Image => wallpaper.image;
    public WallCardViewModel(Wallpaper wallpaper)
    {
        this.wallpaper = wallpaper;
    }
}
using System.Windows.Media.Imaging;

namespace Personate.Modules.WallpapperSwitcher;
internal class WallCardViewModel : ObservableObject
{
    public Wallpaper Wallpaper { get; private set; }
    public BitmapImage Image => Wallpaper.Image;
    public string Name => Wallpaper.Name;
    public string Resolution => Wallpaper.Resolution;
    public RelayCommand WallViewCommand { get; set; }

    public WallCardViewModel(Wallpaper wallpaper)
    {
        Wallpaper = wallpaper;
    }
}
using System.Windows.Media.Imaging;

namespace Personate.Modules.WallpapperSwitcher;
internal class WallCardViewModel : ObservableObject
{
    public static RelayCommand? WallViewCommand { get; set; }

    private Wallpaper _wallpaper;

    public string Resolution
    {
        get => _wallpaper.resolution;
        set => SetProperty(ref _wallpaper.resolution, value);
    }
    public BitmapImage Image
    {
        get => _wallpaper.image;
        set => SetProperty(ref _wallpaper.image, value);
    }
    public WallCardViewModel(Wallpaper wallpaper)
    {
        _wallpaper = wallpaper;
    }
}
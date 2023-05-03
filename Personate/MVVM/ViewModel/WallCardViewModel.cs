using System.Windows.Media.Imaging;

namespace Personate.MVVM.ViewModel;
public class WallCardViewModel : Base.ViewModel
{
    public static Base.Command? WallViewCommand { get; set; }
    
    private Wallpaper _wallpaper;

    public string Resolution
    {
        get => _wallpaper.resolution;
        set => Set(ref _wallpaper.resolution, value);
    }
    public BitmapImage Image
    {
        get => _wallpaper.image;
        set => Set(ref _wallpaper.image, value);
    }
    public WallCardViewModel(Wallpaper wallpaper)
    {
        this._wallpaper = wallpaper;
    }

}

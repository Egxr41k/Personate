using System.Windows.Media.Imaging;

namespace Personate.MVVM.ViewModel;
internal class WallCardViewModel : Base.ViewModel
{
    public static Base.Command? WallViewCommand { get; set; }
    
    public Wallpaper wallpaper;

    public string Resolution
    {
        get => wallpaper.resolution;
        set => Set(ref wallpaper.resolution, value);
    }
    public BitmapImage Image
    {
        get => wallpaper.image;
        set => Set(ref wallpaper.image, value);
    }
    public WallCardViewModel()
    {
        wallpaper = WallsMenuViewModel.curwallpaper;
    }

}

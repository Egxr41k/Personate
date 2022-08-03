using System.Windows.Media.Imaging;

namespace Personate.MVVM.ViewModel;
internal class WallCardViewModel : Base.ViewModel
{
    private string resolution;
    public string Resolution
    {
        get => resolution;
        //set => Set(ref resolution, value);
    }
    public static Base.Command? WallViewCommand { get; set; }


    private BitmapImage image;
    public BitmapImage Image
    {
        get => image;
        //set => Set(ref image, value);
    }
    public string Name;
    protected virtual void Init()
    {
        image = new BitmapImage(new Uri(WallpaperModel.path));
        Name = WallpaperModel.path.Split('\\').Last();
        resolution = $"{Image.PixelWidth}" + "x" + $"{Image.PixelHeight}";
    }
    public WallCardViewModel()
    {
        Init();
    }

}

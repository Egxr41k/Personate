using Personate.Modules.WallpaperSwitcher.Models;
using System.Windows.Media.Imaging;

namespace Personate.Modules.WallpaperSwitcher.ViewModels;
internal class ItemViewModel : ObservableObject
{
    public Model Wallpaper { get; private set; }
    public BitmapImage Image => Wallpaper.Image;
    public string Name => Wallpaper.Name;
    public string Resolution => Wallpaper.Resolution;
    public string Color => Wallpaper.Color;
    public RelayCommand WallViewCommand { get; set; }

    public ItemViewModel(Model wallpaper)
    {
        Wallpaper = wallpaper;
    }
}
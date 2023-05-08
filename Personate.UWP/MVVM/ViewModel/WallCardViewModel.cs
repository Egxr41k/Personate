using CommunityToolkit.Mvvm.ComponentModel;
using Personate.UWP.MVVM.Model;
using Windows.UI.Xaml.Media.Imaging;

namespace Personate.UWP.MVVM.ViewModel
{
    public class WallCardViewModel : ObservableObject
    {
        //public static Base.Command? WallViewCommand { get; set; }

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
            this._wallpaper = wallpaper;
        }

    }
}
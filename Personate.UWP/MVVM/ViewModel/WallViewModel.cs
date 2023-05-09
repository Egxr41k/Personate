using Personate.UWP.MVVM.ViewModel;
using Personate.UWP.MVVM.Model;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Personate.UWP.MVVM.ViewModel
{
    class WallViewModel : WallCardViewModel
    {
        public ICommand SetAsBgCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }
        public WallViewModel()
        {
            //Init();

            SetAsBgCommand = new RelayCommand(() =>
            {
                //Wallpaper.SetImageToBg();
            });

            SaveFileCommand = new RelayCommand(() =>
            {
                //WallpaperModel.SaveImage();
            });
        }
    }
}
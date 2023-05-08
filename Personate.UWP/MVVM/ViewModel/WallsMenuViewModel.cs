using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Personate.UWP.MVVM.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Personate.UWP.MVVM.ViewModel
{
    public class WallsMenuViewModel : ObservableObject
    {

        readonly string[] PathToWallpapers = Directory.GetFiles(
            MainViewModel.RESOURCEPATH + "\\PersonateLib\\Wallpapers");

        public static ICommand UploadImageCommand { get; set; }
        public ICommand ShowMoreCommand { get; set; }

        private int index = 0; // counter


        private ObservableCollection<WallCardViewModel> _wallCardViewModels;
        public IEnumerable<WallCardViewModel> WallCardViewModels => _wallCardViewModels;


        private async Task WallGridInit()
        {
            for (int column = 0; column < 23; column++)
            {
                if (index < PathToWallpapers.Length)
                {
                    _wallCardViewModels.Add(
                        new WallCardViewModel(
                            new Wallpaper(
                                PathToWallpapers[index]
                            )
                        )
                    );
                    index++;
                }
            }
        }

        public WallsMenuViewModel()
        {
            _wallCardViewModels =
                    new ObservableCollection<WallCardViewModel>();

            WallGridInit();

            ShowMoreCommand = new RelayCommand(() =>
            {
                WallGridInit();
            });
            UploadImageCommand = new RelayCommand(() =>
            {
                var result = Wallpaper.OpenImage();
            });
        }
    }
}
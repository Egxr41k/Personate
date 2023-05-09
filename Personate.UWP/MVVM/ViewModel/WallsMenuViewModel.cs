using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Personate.UWP.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace Personate.UWP.MVVM.ViewModel
{
    public class WallsMenuViewModel : ObservableObject
    {

        

        //readonly string[] PathToWallpapers = Directory.GetFiles(
        //    MainViewModel.RESOURCEPATH + "\\PersonateLib\\Wallpapers");

        public static ICommand UploadImageCommand { get; set; }
        public ICommand ShowMoreCommand { get; set; }

        private int index = 0; // counter


        private ObservableCollection<WallCardViewModel> _wallCardViewModels;
        public IEnumerable<WallCardViewModel> WallCardViewModels => _wallCardViewModels;


        private async Task WallGridInit()
        {
            StorageFolder installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;

            // получаем папку Data
            StorageFolder folder = await installedLocation.GetFolderAsync("\\Resources\\PersonateLib\\Wallpapers");
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();

            //foreach (StorageFile file in files)
            //    filesList.Items.Add(file.Name);

            for (int column = 0; column < 23; column++)
            {
                if (index < files.Count)
                {
                    _wallCardViewModels.Add(new WallCardViewModel()
                    {
                        _wallpaper = new Wallpaper(files[index].Name)
                    });

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
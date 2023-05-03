using Personate.MVVM.ViewModel.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Personate.MVVM.ViewModel;
public class WallsMenuViewModel : Base.ViewModel
{
    readonly string[] PathToWallpapers = Directory.GetFiles(
        MainViewModel.RESOURCEPATH + "\\PersonateLib\\Wallpapers");
    public static Base.Command? UploadImageCommand { get; set; }
    public Base.Command ShowMoreCommand { get; set; }

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

        ShowMoreCommand = new(async o =>
        {
            await WallGridInit();
        });
    }
}
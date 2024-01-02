using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Personate.Modules.WallpapperSwitcher;
internal class WallsMenuViewModel : ObservableObject
{
    readonly string[] PathToWallpapers = Directory.GetFiles(
        MainViewModel.RESOURCEPATH + "\\PersonateLib\\Wallpapers");

    public static RelayCommand? UploadImageCommand { get; set; }
    public RelayCommand ShowMoreCommand { get; set; }


    private ObservableCollection<WallCardViewModel> _wallCardViewModels;
    public IEnumerable<WallCardViewModel> WallCardViewModels => _wallCardViewModels;


    private List<WallCardViewModel> WallCardVMsInit(int cardsCount)
    {
        var result = new List<WallCardViewModel>();
        int targetCount = _wallCardViewModels.Count + cardsCount;
        for (int i = _wallCardViewModels.Count; i < targetCount; i++)
        {
            if (i >= PathToWallpapers.Length) break;
            result.Add(new WallCardViewModel(new Wallpaper(PathToWallpapers[i])));
        }
        return result;
    }

    private void ShowWallCardVMs(int cardsCount)
    {
        WallCardVMsInit(cardsCount).ForEach(card => _wallCardViewModels.Add(card));
    }

    public WallsMenuViewModel()
    {
        _wallCardViewModels = new ObservableCollection<WallCardViewModel>();

        ShowWallCardVMs(10);

        ShowMoreCommand = new(() =>
        {
            ShowWallCardVMs(5);
        });
    }
}
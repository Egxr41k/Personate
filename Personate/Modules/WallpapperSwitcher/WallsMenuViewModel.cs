using System.Collections.Generic;
using System.Collections.ObjectModel;
using Personate.General;

namespace Personate.Modules.WallpapperSwitcher;
internal class WallsMenuViewModel : ObservableObject
{
    private static string WallpaperDirectory = MainViewModel.PersonateLibPath + "\\Wallpapers";
    private readonly string[] wallpaperPaths = Directory.GetFiles(WallpaperDirectory);

    private const int InitialWallCardCount = 10;
    private const int WallCardsToShowPerClick = 5;

    public RelayCommand UploadImageCommand { get; set; }
    public RelayCommand ShowMoreCommand { get; set; }
    public RelayCommand WallViewCommand { get; set; }

    public IEnumerable<WallCardViewModel> WallCardViewModels => wallCardViewModels;
    private readonly ObservableCollection<WallCardViewModel> wallCardViewModels = [];

    private WallCardViewModel selectedWallCardViewModel;
    public WallCardViewModel SelectedWallCardViewModel
    {
        get => selectedWallCardViewModel;
        set => SetProperty(ref selectedWallCardViewModel, value);
    }

    public WallsMenuViewModel()
    {
        ShowWallCards(InitialWallCardCount);
        ShowMoreCommand = new(() => ShowWallCards(WallCardsToShowPerClick));
    }

    private void ShowWallCards(int cardsCount)
    {
        int initialCount = wallCardViewModels.Count;
        int targetCount = initialCount + cardsCount;
        int maxCount = wallpaperPaths.Length;

        for (int i = initialCount; i < targetCount; i++)
        {
            if (i == maxCount) break;
            wallCardViewModels.Add(InitWallCard(wallpaperPaths[i]));
        }
    }

    private WallCardViewModel InitWallCard(string path)
    {
        return new WallCardViewModel(new Wallpaper(path));
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Personate.General;

namespace Personate.Modules.WallpapperSwitcher;
internal class MenuViewModel : ObservableObject
{
    private readonly static string WallpaperDirectory = MainViewModel.PersonateLibPath + "\\Wallpapers";
    private readonly string[] PathToWallpapers = Directory.GetFiles(WallpaperDirectory);

    private const int InitialItemsCount = 10;
    private const int ItemsToShowPerClick = 5;

    public RelayCommand UploadCommand { get; set; }
    public RelayCommand ShowMoreCommand { get; set; }
    public RelayCommand ToDetailsCommand { get; set; }

    public IEnumerable<ItemViewModel> Items => items;
    private readonly ObservableCollection<ItemViewModel> items = [];

    private ItemViewModel selectedItem;
    public ItemViewModel SelectedItem
    {
        get => selectedItem;
        set
        {
            SetProperty(ref selectedItem, value);
            ToDetailsCommand.Execute(null);
        }
    }

    public MenuViewModel()
    {
        ShowItems(InitialItemsCount);
        ShowMoreCommand = new(() => ShowItems(ItemsToShowPerClick));
    }

    private void ShowItems(int itemsCount)
    {
        int initialCount = items.Count;
        int targetCount = initialCount + itemsCount;
        int maxCount = PathToWallpapers.Length;

        for (int i = initialCount; i < targetCount; i++)
        {
            if (i == maxCount) break;

            Model wallpaper = new(PathToWallpapers[i]);
            ItemViewModel item = new(wallpaper);
            items.Add(item);
        }
    }
}
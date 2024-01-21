using Personate.General;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Personate.Modules.CursorSwitcher;
internal class MenuViewModel : ObservableObject
{
    private readonly static string CursorDirectory = MainViewModel.PersonateLibPath + "\\Cursors";
    private readonly string[] PathToCursors = Directory.GetDirectories(CursorDirectory);
    public static string PathToDefaultCursor = CursorDirectory + "\\default";

    private const int InitialItemsCount = 10;
    private const int ItemsToShowPerClick = 5;

    public RelayCommand UploadCommand { get; set; }
    public RelayCommand ShowMoreCommand { get; set; }
    public RelayCommand DetailsViewCommand { get; set; }

    public IEnumerable<ItemViewModel> Items => items;
    private readonly ObservableCollection<ItemViewModel> items = [];

    private ItemViewModel selectedItem;
    public ItemViewModel SelectedItem
    {
        get => selectedItem;
        set
        {
            SetProperty(ref selectedItem, value);
            DetailsViewCommand.Execute(null);
        }
    }

    public MenuViewModel()
    {
        ShowItems(InitialItemsCount);
        ShowMoreCommand = new(() => ShowItems(ItemsToShowPerClick));
    }

    private void ShowItems(int ItemsCount)
    {
        int initialCount = items.Count;
        int targetCount = initialCount + ItemsCount;
        int maxCount = PathToCursors.Length;

        for (int i = initialCount; i < targetCount; i++)
        {
            if (i == maxCount) break;
            if (PathToCursors[i] == PathToDefaultCursor) continue;

            Model cursor = new(PathToCursors[i]);
            ItemViewModel item = new(cursor);
            items.Add(item);
        }
    }
}
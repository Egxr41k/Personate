using Personate.General;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Personate.Modules.CursorSwitcher;
internal class CursorsMenuViewModel : ObservableObject
{
    private readonly static string CursorDirectory = MainViewModel.PersonateLibPath + "\\Cursors";
    private readonly string[] PathToCursors = Directory.GetDirectories(CursorDirectory);

    private const int InitialCursorCardCount = 10;
    private const int CursorCardsToShowPerClick = 5;

    public RelayCommand UploadCommand { get; set; }
    public RelayCommand ShowMoreCommand { get; set; }
    public RelayCommand CursorViewCommand { get; set; }

    public IEnumerable<CursorCardViewModel> CursorCardViewModels => cursorCardViewModels;
    private readonly ObservableCollection<CursorCardViewModel> cursorCardViewModels = [];

    private CursorCardViewModel selectedCursorCardViewModel;
    public CursorCardViewModel SelectedWallCardViewModel
    {
        get => selectedCursorCardViewModel;
        set
        {
            SetProperty(ref selectedCursorCardViewModel, value);
            CursorViewCommand.Execute(null);
        }
    }

    public CursorsMenuViewModel()
    {
        ShowCursorCards(InitialCursorCardCount);
        ShowMoreCommand = new(() => ShowCursorCards(CursorCardsToShowPerClick));
    }

    private void ShowCursorCards(int cardsCount)
    {
        int initialCount = cursorCardViewModels.Count;
        int targetCount = initialCount + cardsCount;
        int maxCount = PathToCursors.Length;

        for (int i = initialCount; i < targetCount; i++)
        {
            if (i == maxCount) break;

            Cursor cursor = new(PathToCursors[i]);
            CursorCardViewModel card = new(cursor);
            cursorCardViewModels.Add(card);
        }
    }
}
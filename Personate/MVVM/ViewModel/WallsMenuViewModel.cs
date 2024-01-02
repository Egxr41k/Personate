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

        ShowMoreCommand = new(o =>
        {
            ShowWallCardVMs(5);
        });
    }
}
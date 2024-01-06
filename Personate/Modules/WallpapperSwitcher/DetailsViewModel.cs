namespace Personate.Modules.WallpapperSwitcher;
internal class DetailsViewModel : ItemViewModel
{
    public RelayCommand SetCommand { get; set; }
    public RelayCommand SaveCommand { get; set; }
    public DetailsViewModel(Model wallpaper) : base(wallpaper)
    {
        SetCommand = new(() => wallpaper.Set());

        SaveCommand = new(() => wallpaper.Save());
    }
}

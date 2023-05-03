namespace Personate.MVVM.ViewModel;
class WallViewModel : WallCardViewModel
{
    public Base.Command SetAsBgCommand { get; set; }
    public Base.Command SaveFileCommand { get; set; }
    public WallViewModel(Wallpaper wallpaper) 
        : base(wallpaper)
    {
        //Init();

        SetAsBgCommand = new(o =>
        {
            WallpaperModel.SetImageToBg();
        });

        SaveFileCommand = new(o =>
        {
            WallpaperModel.SaveImage();
        });
    }
}

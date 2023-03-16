namespace Personate.MVVM.ViewModel;
class WallsMenuViewModel : Base.MenuViewModel
{
    readonly string[] PathToWallpapers = Directory.GetFiles(
        MainViewModel.RESOURCEPATH + "\\PersonateLib\\Wallpapers");
    public static Base.Command? UploadImageCommand { get; set; }
    public static Wallpaper curwallpaper;
    private void WallGridInit()
    {
        WallCardViewModel[] wallsVMs = new WallCardViewModel[36];
        ContentControl[] controls = new ContentControl[36];
        var wallpapers = new Wallpaper[36];
        object curControl;
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                wallpapers[index] = new Wallpaper(
                    PathToWallpapers[index]);
                curwallpaper = wallpapers[index];
                wallsVMs[index] = new();
                //wallsVMs[index].wallpaper = new(PathToWallpapers[index]);

                controls[index] = new()
                {
                    Content = wallsVMs[index]
                };

                var test = wallsVMs[index].Image;
                var test1 = wallsVMs[index].Resolution;
                curControl = controls[index].Content;
                var test2 = MainGrid;
                //var test3 = walls[index].Name;

                SetToGrid(controls[index], row, column);
                index++;
            }
        
         }
    }
    
    public WallsMenuViewModel()
    {
        ColumnsInit(3);
        RowsInit(3, 195);
        WallGridInit();
        ShowMoreCommand = new(o =>
        {
            //helper.RowsInit(3, 195);
        });
        GridControl = MainGrid;
    }
}
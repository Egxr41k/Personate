namespace Personate.MVVM.ViewModel;
class WallsMenuViewModel : Base.MenuViewModel
{
    readonly string[] PathToWallpapers = Directory.GetFiles(
        MainViewModel.RESOURCEPATH + "\\Wallpapers");
    public static Base.Command? UploadImageCommand { get; set; }

    private void WallGridInit()
    {
        WallCardViewModel[] walls = new WallCardViewModel[36];
        ContentControl[] controls = new ContentControl[36];
        object curControl;
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                WallpaperModel.path = PathToWallpapers[index];
                walls[index] = new();

                controls[index] = new()
                {
                    Content = walls[index]
                };

                var test = walls[index].Image;
                var test1 = walls[index].Resolution;
                curControl = controls[index].Content;
                var test2 = MainGrid;
                var test3 = walls[index].Name;

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
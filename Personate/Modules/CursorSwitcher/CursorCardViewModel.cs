using System.Windows.Media.Imaging;

namespace Personate.Modules.CursorSwitcher;
internal class CursorCardViewModel : ObservableObject
{
    public static RelayCommand CursorViewCommand { get; set; }

    private BitmapImage cursorImg;
    public BitmapImage CursorImg
    {
        get => cursorImg;
        set => SetProperty(ref cursorImg, value);
    }

    private string cursorTitle;
    public string CursorTitle
    {
        get => cursorTitle;
        set => SetProperty(ref cursorTitle, value);
    }

    private string cursorCount;
    public string CursorCount
    {
        get => cursorCount;
        set => SetProperty(ref cursorCount, value);
    }

    protected virtual void Init()
    {
        //CursorImg = new BitmapImage(new Uri(CursorModel.CursorImg));
        //CursorTitle = CursorModel.CursorTitle + " " + CursorModel.CursorColor;
        //CursorCount = CursorModel.CursorCount.ToString();
    }

    public CursorCardViewModel()
    {
        Init();
    }
}
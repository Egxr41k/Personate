using System.Windows.Media.Imaging;

namespace Personate.Modules.CursorSwitcher;
internal class CursorCardViewModel : ObservableObject
{
    public Cursor Cursor { get; private set; }
    public BitmapImage Image => Cursor.Image;
    public string Name => Cursor.Name;
    public int Count => Cursor.Count;

    public static RelayCommand CursorViewCommand { get; set; }
    public CursorCardViewModel(Cursor cursor)
    {
        Cursor = cursor;
    }
}
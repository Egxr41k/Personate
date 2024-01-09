using System.Windows.Media.Imaging;

namespace Personate.Modules.CursorSwitcher;
internal class ItemViewModel : ObservableObject
{
    public Model Cursor { get; private set; }
    public BitmapImage Image => Cursor.Image;
    public string Name => Cursor.Name;
    public int Count => Cursor.Count;
    public string Color => Cursor.Color;

    public static RelayCommand CursorViewCommand { get; set; }
    public ItemViewModel(Model cursor)
    {
        Cursor = cursor;
    }
}
namespace Personate.Modules.CursorSwitcher;
internal class CursorViewModel : ObservableObject
{
    public Cursor Cursor { get; private set; }

    public RelayCommand ToDefaultCommand { get; set; }
    public RelayCommand SetCommand { get; set; }

    public CursorViewModel(Cursor cursor)
    {
        Cursor = cursor;

        SetCommand = new(() =>
        {
            Cursor.Set();
        });

        ToDefaultCommand = new(() =>
        {
            Cursor.ToDefault();
        });
    }
}

namespace Personate.Modules.CursorSwitcher;
internal class CursorViewModel : ObservableObject
{
    public RelayCommand ToDefaultCommand { get; set; }
    public RelayCommand SetAsCursorCommand { get; set; }

    private string path = "";
    public string Path
    {
        get => path;
        set => SetProperty(ref path, value);
    }
    public CursorViewModel()
    {
        SetAsCursorCommand = new(() =>
        {
            //Cursor.SetCursor();
            //path = Cursor.path;
        });

        ToDefaultCommand = new(() =>
        {
            //Cursor.path = @"C:\Windows\ToDefault.inf";
            //Cursor.SetCursor();
            //path = Cursor.path;
        });
    }
}

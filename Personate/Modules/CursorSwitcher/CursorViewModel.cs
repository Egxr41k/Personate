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
            CursorModel.SetCursor();
            path = CursorModel.path;
        });
        ToDefaultCommand = new(() =>
        {
            CursorModel.path = @"C:\Windows\ToDefault.inf";
            CursorModel.SetCursor();
            path = CursorModel.path;
        });
    }
}

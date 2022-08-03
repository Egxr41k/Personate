

namespace Personate.MVVM.ViewModel;
internal class CursorViewModel : Base.ViewModel
{
    public Base.Command ToDefaultCommand { get; set; }
    public Base.Command SetAsCursorCommand { get; set; }

    private string path = "";
    public string Path
    {
        get => path;
        set => Set(ref path, value);
    }
    public CursorViewModel()
    {
        SetAsCursorCommand = new(o =>
        {
            CursorModel.SetCursor();
            this.path = CursorModel.path;
        });
        ToDefaultCommand = new(o =>
        {
            CursorModel.path = @"C:\Windows\ToDefault.inf";
            CursorModel.SetCursor();
            this.path = CursorModel.path;
        });
    }
}

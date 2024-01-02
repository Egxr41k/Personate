namespace Personate.Modules.CursorSwitcher;
internal class CursorsMenuViewModel : ObservableObject
{
    readonly string[] PathToCursors = Directory.GetDirectories(
        MainViewModel.RESOURCEPATH + "\\PersonateLib\\Cursors");
    public static RelayCommand? UploadCursorCommand { get; set; }

    public CursorsMenuViewModel()
    {

    }
}
namespace Personate.Modules.CursorSwitcher;
internal class CursorsMenuViewModel : ObservableObject
{
    readonly string[] PathToCursors = Directory.GetDirectories(
        MainViewModel.ResourcesPath + "\\PersonateLib\\Cursors");
    public RelayCommand UploadCursorCommand { get; set; }

    public CursorsMenuViewModel()
    {

    }
}
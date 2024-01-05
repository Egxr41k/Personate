using Personate.General;

namespace Personate.Modules.CursorSwitcher;
internal class CursorsMenuViewModel : ObservableObject
{
    private static string CursorDirectory = MainViewModel.PersonateLibPath + "\\Cursors";
    readonly string[] PathToCursors = Directory.GetDirectories(CursorDirectory);
    public RelayCommand UploadCursorCommand { get; set; }

    public CursorsMenuViewModel()
    {

    }
}
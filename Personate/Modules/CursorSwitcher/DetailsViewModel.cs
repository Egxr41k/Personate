namespace Personate.Modules.CursorSwitcher;
internal class DetailsViewModel : ObservableObject
{
    public Model Cursor { get; private set; }

    public RelayCommand ToDefaultCommand { get; set; }
    public RelayCommand SetCommand { get; set; }

    public DetailsViewModel(Model cursor)
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

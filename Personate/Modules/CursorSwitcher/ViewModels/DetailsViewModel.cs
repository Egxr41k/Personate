using Personate.Modules.CursorSwitcher.Models;

namespace Personate.Modules.CursorSwitcher.ViewModels;
internal class DetailsViewModel : ItemViewModel
{
    public RelayCommand ToDefaultCommand { get; set; }
    public RelayCommand SetCommand { get; set; }

    public DetailsViewModel(Model cursor) : base(cursor)
    {
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

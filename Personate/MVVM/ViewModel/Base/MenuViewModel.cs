using System.Collections.ObjectModel;

namespace Personate.MVVM.ViewModel.Base;
abstract class MenuViewModel : ViewModel
{
    protected Command? ShowMoreCommand { get; set; }

    private int index = 0; // counter

    protected virtual void ContentGridInit(int itemsCount,
        ref ObservableCollection<ViewModel> viewModels)
    {

    }

    //protected void ColumnsInit(int columnsCount)
    //{
    //    ColumnDefinition[] columns = new ColumnDefinition[columnsCount];
    //    for (int c = 0; c < columnsCount; c++)
    //    {
    //        columns[c] = new ColumnDefinition();
    //        MainGrid.ColumnDefinitions.Add(columns[c]);
    //    }
    //}

    //protected void RowsInit(int rowsCount, int height)
    //{
    //    RowDefinition[] rows = new RowDefinition[rowsCount];
    //    for (int r = 0; r < rowsCount; r++)
    //    {
    //        rows[r] = new RowDefinition
    //        {
    //            Height = new GridLength(height)
    //        };
    //        MainGrid.RowDefinitions.Add(rows[r]);
    //    }
    //}

    //protected void SetToGrid(UIElement el, int row, int column)
    //{
    //    Grid.SetRow(el, row);
    //    Grid.SetColumn(el, column);
    //    MainGrid.Children.Add(el);
    //}
}

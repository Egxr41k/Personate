namespace Personate.MVVM.ViewModel.Base;
internal class MenuViewModel : ViewModel
{
    protected Grid MainGrid = new();
    
    public Command? ShowMoreCommand { get; set; }

    public int index = 0;
    //int row = 0;

    private object gridControl;
    public object GridControl
    {
        get => gridControl;
        set => Set(ref gridControl, value);
    }

    protected void ColumnsInit(int columnsCount)
    {
        ColumnDefinition[] columns = new ColumnDefinition[columnsCount];
        for (int c = 0; c < columnsCount; c++)
        {
            columns[c] = new ColumnDefinition();
            MainGrid.ColumnDefinitions.Add(columns[c]);
        }
    }

    protected void RowsInit(int rowsCount, int height)
    {
        RowDefinition[] rows = new RowDefinition[rowsCount];
        for (int r = 0; r < rowsCount; r++)
        {
            rows[r] = new RowDefinition
            {
                Height = new GridLength(height)
            };
            MainGrid.RowDefinitions.Add(rows[r]);
        }
    }

    protected void SetToGrid(UIElement el, int row, int column)
    {
        Grid.SetRow(el, row);
        Grid.SetColumn(el, column);
        MainGrid.Children.Add(el);
    }
    public MenuViewModel()
    {
        gridControl = MainGrid;
    }
}

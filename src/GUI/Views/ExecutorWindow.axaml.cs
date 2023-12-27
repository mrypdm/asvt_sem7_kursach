using System.Linq;
using Avalonia;
using Avalonia.Controls;

namespace GUI.Views;

public partial class ExecutorWindow : Window
{
    public ExecutorWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void DataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var dataGrid = sender as DataGrid;
        dataGrid!.ScrollIntoView(dataGrid.SelectedItem, dataGrid.Columns.FirstOrDefault());
    }
}
using System.Linq;
using Avalonia;
using Avalonia.Controls;

namespace GUI.Views;

/// <summary>
/// Main window view
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void TextBoxResult_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var linesNumber = SourceCodeTextBox.Text!.Split('\n').Length;
        SourceCodeNumerationBox.Text = $"{string.Join("\n", Enumerable.Range(1, linesNumber))}\n";
    }
}
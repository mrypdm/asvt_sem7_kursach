using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUI.Views;

/// <summary>
/// File tab view
/// </summary>
public partial class FileTab : UserControl
{
    /// <summary>
    /// Creates new instance of file tab
    /// </summary>
    public FileTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
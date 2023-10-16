using Avalonia;
using Avalonia.Controls;
using GUI.ViewModels;

namespace GUI.Views;

/// <summary>
/// Main window view
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(this);
#if DEBUG
        this.AttachDevTools();
#endif
    }
}
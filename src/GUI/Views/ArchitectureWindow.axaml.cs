using Avalonia;
using Avalonia.Controls;

namespace GUI.Views;

public partial class ArchitectureWindow : Window
{
    public ArchitectureWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }
}
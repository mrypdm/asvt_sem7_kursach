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
}
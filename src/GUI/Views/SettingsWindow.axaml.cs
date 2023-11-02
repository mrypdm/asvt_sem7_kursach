using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUI.ViewModels;

namespace GUI.Views;

/// <summary>
/// Settings window view
/// </summary>
public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
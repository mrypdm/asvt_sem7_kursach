using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUI.ViewModels;

namespace GUI.Controls;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel(this);
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
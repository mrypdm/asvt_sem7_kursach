using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GUI.ViewModels;
using GUI.Views;

namespace GUI;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = new MainWindowViewModel(new MainWindow());
            desktop.MainWindow = viewModel.View;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
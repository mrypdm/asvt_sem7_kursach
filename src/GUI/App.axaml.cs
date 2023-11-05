using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Domain.Providers;
using GUI.Managers;
using GUI.MessageBoxes;
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
            var viewModel = new MainWindowViewModel(
                new MainWindow(),
                new TabManager(),
                new ProjectManager(new ProjectProvider()),
                new FileManager(),
                new MessageBoxManager());
            desktop.MainWindow = viewModel.View;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
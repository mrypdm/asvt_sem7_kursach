using Avalonia;
using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using GUI.Managers;
using GUI.Models;
using Shared;

namespace GUI;

public static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var configuration = Configuration.BuildFromJson();

        var editorOptions = configuration.GetOptions<EditorOptions>();
        SettingsManager.Create(editorOptions);

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
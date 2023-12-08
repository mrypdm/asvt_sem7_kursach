using Avalonia;
using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using GUI.Managers;
using GUI.Models;
using Shared.Helpers;

namespace GUI;

public static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var configuration = ConfigurationHelper.BuildFromJson();

        var commandLineOptions = CommandLineHelper.ParseCommandLine<CommandLineOptions>(args, out _);
        var editorOptions = configuration.GetOptions<EditorOptions>();

        SettingsManager.Create(editorOptions, commandLineOptions);

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
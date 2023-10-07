using Avalonia;
using System;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using CommandLine;
using CommandLine.Text;
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

        var commandLineOptions = ParseCommandLine(args);
        var editorOptions = configuration.GetOptions<EditorOptions>();

        SettingsManager.Create(editorOptions, commandLineOptions);
        ProjectManager.Create();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
    }

    private static CommandLineOptions ParseCommandLine(string[] args)
    {
        var commandLineParserResult = new Parser(opt =>
        {
            opt.AutoVersion = true;
            opt.AutoHelp = true;
            opt.CaseSensitive = true;
            opt.HelpWriter = null;
            opt.IgnoreUnknownArguments = false;
        }).ParseArguments<CommandLineOptions>(args);

        return commandLineParserResult.WithNotParsed(_ =>
        {
            var helpText = HelpText.AutoBuild(commandLineParserResult, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = $"PDP-11 Simulator GUI {Assembly.GetExecutingAssembly().GetName().Version}";
                h.Copyright = string.Empty;

                return HelpText.DefaultParsingErrorsHandler(commandLineParserResult, h);
            }, e => e);

            Console.WriteLine(helpText);
            Environment.Exit(1);
        }).Value;
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
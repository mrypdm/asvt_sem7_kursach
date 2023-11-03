using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace Shared.Helpers;

/// <summary>
/// Helper for command line
/// </summary>
public static class CommandLineHelper
{
    /// <summary>
    /// Parses command line arguments to <typeparamref name="TResult"/>
    /// </summary>
    /// <param name="args">Arguments</param>
    /// <param name="errorText">Error text is errors occured</param>
    /// <typeparam name="TResult">Type of options</typeparam>
    /// <returns>Command line options</returns>
    /// <exception cref="InvalidOperationException">If cannot determine entry assembly</exception>
    public static TResult ParseCommandLine<TResult>(IEnumerable<string> args, out string errorText)
        where TResult : new()
    {
        var assembly = Assembly.GetEntryAssembly()?.GetName() ??
                       throw new InvalidOperationException("Cannot determine entry assembly");
        errorText = null;

        var commandLineParserResult = new Parser(opt =>
        {
            opt.AutoVersion = true;
            opt.AutoHelp = true;
            opt.CaseSensitive = true;
            opt.HelpWriter = null;
            opt.IgnoreUnknownArguments = false;
        }).ParseArguments<TResult>(args);

        if (commandLineParserResult.Errors.Any())
        {
            errorText = HelpText.AutoBuild(commandLineParserResult, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = $"{assembly.Name} {assembly.Version}";
                h.Copyright = string.Empty;

                return HelpText.DefaultParsingErrorsHandler(commandLineParserResult, h);
            }, e => e);
        }

        return commandLineParserResult.Value;
    }
}
using CommandLine;

namespace GUI.Models;

/// <summary>
/// Options from command line
/// </summary>
public class CommandLineOptions
{
    /// <summary>
    /// Startup project
    /// </summary>
    [Option('p', "project", Required = false, HelpText = "Startup project", Default = null)]
    public string Project { get; set; } = null;
}

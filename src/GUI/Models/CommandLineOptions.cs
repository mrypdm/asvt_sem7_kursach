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
    [Option('p', "project", Required = false, HelpText = "Startup project")]
    public string Project { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Shared.Converters;
using Shared.Helpers;

namespace Domain.Models;

/// <summary>
/// Model of project
/// </summary>
public class ProjectModel
{
    public const string ProjectFileExtension = "pdp11proj";

    /// <summary>
    /// Files of project (relative paths)
    /// </summary>
    public List<string> Files { get; set; } = new();

    /// <summary>
    /// Executable file
    /// </summary>
    public string Executable { get; set; } = string.Empty;

    /// <summary>
    /// List of connected devices
    /// </summary>
    public List<string> Devices { get; set; } = new();

    /// <summary>
    /// Initial address of stack pointer (original value), <see cref="StackAddress"/>
    /// </summary>
    public string StackAddressString { get; set; } = "0o1000";

    /// <summary>
    /// Start address of program (original value), <see cref="ProgramAddress"/>
    /// </summary>
    public string ProgramAddressString { get; set; } = "0o1000";

    /// <summary>
    /// Directory of project
    /// </summary>
    [JsonIgnore]
    public string Directory { get; set; }

    /// <summary>
    /// Project file name
    /// </summary>
    [JsonIgnore]
    public string ProjectFileName { get; set; }

    /// <summary>
    /// Path to project file
    /// </summary>
    [JsonIgnore]
    public string ProjectFilePath => PathHelper.Combine(Directory, ProjectFileName);

    /// <summary>
    /// Absolute paths to project files
    /// </summary>
    [JsonIgnore]
    public IList<string> ProjectFilesPaths => Files.Select(f => PathHelper.Combine(Directory, f)).ToList();

    /// <summary>
    /// Absolute path to executable file
    /// </summary>
    [JsonIgnore]
    public string ExecutableFilePath => Executable == string.Empty ? null : PathHelper.Combine(Directory, Executable);

    /// <summary>
    /// Initial address of stack pointer
    /// </summary>
    [JsonIgnore]
    public int StackAddress
    {
        get => new NumberStringConverter().Convert(StackAddressString);
        set => StackAddressString = $"0o{Convert.ToString(value, 8)}";
    }

    /// <summary>
    /// Start address of program
    /// </summary>
    [JsonIgnore]
    public int ProgramAddress
    {
        get => new NumberStringConverter().Convert(ProgramAddressString);
        set => ProgramAddressString = $"0o{Convert.ToString(value, 8)}";
    }
}
using System;
using System.IO;
using System.Text.Json.Serialization;
using Shared.Converters;

namespace GUI.Models;

/// <summary>
/// Model of project
/// </summary>
public class ProjectModel
{
    public const string ProjectFileExtension = "pdp11proj";

    /// <summary>
    /// Name of project
    /// </summary>
    public string Name { get; set; } = "NewProject";

    /// <summary>
    /// Files of project
    /// </summary>
    public string[] Files { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Executable file
    /// </summary>
    public string Executable { get; set; } = string.Empty;

    /// <summary>
    /// External devices
    /// </summary>
    public string[] ExternalDevices { get; set; } = Array.Empty<string>();

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
    public string ProjectFileName => $"{Name}.{ProjectFileExtension}";

    /// <summary>
    /// Path to project file
    /// </summary>
    [JsonIgnore]
    public string ProjectFilePath => Path.Combine(Directory, ProjectFileName);

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
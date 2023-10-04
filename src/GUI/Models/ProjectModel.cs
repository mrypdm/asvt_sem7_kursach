using System;
using System.IO;
using System.Text.Json.Serialization;

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
    /// Directory of project
    /// </summary>
    [JsonIgnore]
    public string Directory { get; set; } = null;

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
}
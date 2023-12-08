using System.Collections.Generic;
using Shared.Helpers;

namespace Domain.Models;

/// <inheritdoc />
public class Project : IProject
{
    /// <inheritdoc />
    public string Executable { get; set; } = string.Empty;

    /// <inheritdoc />
    public IList<string> Files { get; init; } = new List<string>();

    /// <inheritdoc />
    public IList<string> Devices { get; init; } = new List<string>();

    /// <inheritdoc />
    public ushort StackAddress { get; set; } = 512;

    /// <inheritdoc />
    public ushort ProgramAddress { get; set; } = 512;

    /// <inheritdoc />
    public string ProjectFile { get; init; } = string.Empty;

    /// <inheritdoc />
    public string ProjectDirectory => PathHelper.GetDirectoryName(ProjectFile);

    /// <inheritdoc />
    public string ProjectName => PathHelper.GetFileName(ProjectFile);

    public string ProjectBinary => PathHelper.Combine(ProjectDirectory, $"{ProjectName}.pdp11bin");
}
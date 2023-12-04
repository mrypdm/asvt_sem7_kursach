using System.Collections.Generic;

namespace Domain.Models;

/// <inheritdoc />
public class ProjectDto : IProjectDto
{
    /// <inheritdoc />
    public string Executable { get; set; } = string.Empty;

    /// <inheritdoc />
    public List<string> Files { get; set; } = new();

    /// <inheritdoc />
    public List<string> Devices { get; set; } = new();

    /// <inheritdoc />
    public string StackAddress { get; set; } = "0o1000";

    /// <inheritdoc />
    public string ProgramAddress { get; set; } = "0o1000";
}
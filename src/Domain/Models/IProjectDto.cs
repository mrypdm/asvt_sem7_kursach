using System.Collections.Generic;

namespace Domain.Models;

/// <summary>
/// Dto for <see cref="IProjectDto"/>
/// </summary>
public interface IProjectDto
{
    /// <inheritdoc cref="IProject.Executable"/>
    string Executable { get; set; }

    /// <inheritdoc cref="IProject.Files"/>
    List<string> Files { get; set; }

    /// <inheritdoc cref="IProject.Devices"/>
    List<string> Devices { get; set; }

    /// <inheritdoc cref="IProject.StackAddress"/>
    string StackAddress { get; set; }

    /// <inheritdoc cref="IProject.ProgramAddress"/>
    string ProgramAddress { get; set; }
}
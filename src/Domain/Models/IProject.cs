using System.Collections.Generic;

namespace Domain.Models;

/// <summary>
/// Model of project
/// </summary>
public interface IProject
{
    /// <summary>
    /// Main file
    /// </summary>
    string Executable { get; }

    /// <summary>
    /// Files of project
    /// </summary>
    IList<string> Files { get; }

    /// <summary>
    /// List of connected devices
    /// </summary>
    IList<string> Devices { get; }

    /// <summary>
    /// Stack init address
    /// </summary>
    ushort StackAddress { get; }

    /// <summary>
    /// Program start address
    /// </summary>
    ushort ProgramAddress { get; }

    /// <summary>
    /// Project file
    /// </summary>
    string ProjectFile { get; }

    /// <summary>
    /// Project directory
    /// </summary>
    string ProjectDirectory { get; }

    /// <summary>
    /// Project name
    /// </summary>
    string ProjectName { get; }

    /// <summary>
    /// Project binary file
    /// </summary>
    string ProjectBinary { get; }
}
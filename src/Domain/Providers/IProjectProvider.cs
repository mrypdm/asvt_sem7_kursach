using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Providers;

/// <summary>
/// Provider for <see cref="Project"/>
/// </summary>
public interface IProjectProvider
{
    /// <summary>
    /// Open project from file
    /// </summary>
    /// <param name="filePath">Path to project file</param>
    /// <returns>Project model</returns>
    Task<Project> OpenProjectAsync(string filePath);

    /// <summary>
    /// Tries to open project from file
    /// </summary>
    /// <param name="filePath">Path to project file</param>
    Task<(bool isSuccess, Project projectModel)> TryOpenProjectAsync(string filePath);
}
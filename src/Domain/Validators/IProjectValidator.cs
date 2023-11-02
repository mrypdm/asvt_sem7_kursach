using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Validators;

/// <summary>
/// Validator for <see cref="ProjectModel"/>
/// </summary>
public interface IProjectValidator
{
    /// <summary>
    /// Throws if project file is invalid
    /// </summary>
    /// <param name="projectPath">Path to project file</param>
    Task ThrowIfInvalid(string projectPath);

    /// <summary>
    /// Throws if project model is invalid
    /// </summary>
    /// <param name="projectModel">Project model</param>
    void ThrowIfInvalid(ProjectModel projectModel);
}
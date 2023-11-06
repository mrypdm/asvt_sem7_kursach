using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Validators;

/// <summary>
/// Validator for <see cref="Project"/>
/// </summary>
public interface IProjectValidator
{
    /// <summary>
    /// Throws if project file is invalid
    /// </summary>
    /// <param name="projectPath">Path to project file</param>
    /// <exception cref="ValidationException">If project file is invalid</exception>
    Task ThrowIfFileInvalidAsync(string projectPath);

    /// <summary>
    /// Throws if project model is invalid
    /// </summary>
    /// <param name="project">Project model</param>
    /// <exception cref="ValidationException">If project model is invalid</exception>
    void ThrowIfModelInvalid(Project project);
}
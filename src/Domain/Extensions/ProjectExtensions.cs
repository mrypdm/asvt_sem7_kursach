using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Shared.Converters;
using Shared.Helpers;

namespace Domain.Extensions;

/// <summary>
/// Extensions for <see cref="Project"/> and <see cref="ProjectDto"/>
/// </summary>
public static class ProjectExtensions
{
    /// <summary>
    /// Converts project to JSON DTO
    /// </summary>
    public static ProjectDto ToDto(this IProject project)
    {
        var converter = new OctalStringConverter();
        
        return new ProjectDto
        {
            Executable = PathHelper.GetRelativePath(project.ProjectDirectory, project.Executable),
            Files = project.Files.Select(f => PathHelper.GetRelativePath(project.ProjectDirectory, f)).ToList(),
            Devices = project.Devices.Select(f => PathHelper.GetRelativePath(project.ProjectDirectory, f)).ToList(),
            StackAddress = converter.Convert(project.StackAddress),
            ProgramAddress = converter.Convert(project.ProgramAddress)
        };
    }

    /// <summary>
    /// Writes project to json file
    /// </summary>
    /// <param name="project">Project</param>
    /// <param name="filePath">Path to file (if null, then path from project is used)</param>
    public static async Task ToJsonAsync(this IProject project, string filePath = null)
    {
        await JsonHelper.SerializeToFileAsync(project.ToDto(), filePath ?? project.ProjectFile);
    }

    /// <summary>
    /// Converts JSON DTO to project
    /// </summary>
    /// <param name="projectDto">JSON DTO</param>
    /// <param name="projectFilePath">Path to project file</param>
    public static Project ToProject(this IProjectDto projectDto, string projectFilePath)
    {
        var directory = PathHelper.GetDirectoryName(projectFilePath);
        var converter = new NumberStringConverter();
        
        return new Project
        {
            ProjectFile = projectFilePath,
            Executable = Path.Combine(directory, projectDto.Executable),
            Files = projectDto.Files.Select(f => PathHelper.Combine(directory, f)).ToList(),
            Devices = projectDto.Devices.Select(f => PathHelper.Combine(directory, f)).ToList(),
            StackAddress = converter.Convert(projectDto.StackAddress),
            ProgramAddress = converter.Convert(projectDto.ProgramAddress)
        };
    }
}
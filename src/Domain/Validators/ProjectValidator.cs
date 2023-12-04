using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Providers;
using Shared.Exceptions;
using Shared.Helpers;

namespace Domain.Validators;

/// <inheritdoc />
public class ProjectValidator : IProjectValidator
{
    private readonly IProjectProvider _projectProvider;

    public ProjectValidator(IProjectProvider projectProvider)
    {
        _projectProvider = projectProvider;
    }

    /// <inheritdoc />
    public async Task ThrowIfFileInvalidAsync(string projectPath)
    {
        try
        {
            await _projectProvider.OpenProjectAsync(projectPath);
        }
        catch (JsonException e)
        {
            throw new ValidationException(e.Message, e);
        }
    }

    /// <inheritdoc />
    public void ThrowIfModelInvalid(Project project)
    {
        if (string.IsNullOrWhiteSpace(project.ProjectFile))
        {
            throw new ValidationException("Project file path is not set");
        }

        if (PathHelper.GetPathType(project.ProjectFile) != PathHelper.PathType.File)
        {
            throw new ValidationException("Project file does not exist");
        }

        var badFiles = project.Files
            .Where(p => PathHelper.GetPathType(p) != PathHelper.PathType.File)
            .ToArray();
        if (badFiles.Any())
        {
            throw new ValidationException($"These files do not exist on disk: {string.Join("; ", badFiles)}");
        }

        var badDevices = project.Devices
            .Where(p => PathHelper.GetPathType(p) != PathHelper.PathType.File)
            .ToArray();
        if (badDevices.Any())
        {
            throw new ValidationException($"These files do not exist on disk: {string.Join("; ", badDevices)}");
        }

        if (string.IsNullOrWhiteSpace(project.Executable))
        {
            throw new ValidationException("Executable file is not set");
        }

        if (!project.Files.Contains(project.Executable))
        {
            throw new ValidationException("Executable file is not represented in project files");
        }

        // TODO: Check that device is valid
    }
}
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Providers;
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
    public void ThrowIfModelInvalid(ProjectModel projectModel)
    {
        if (string.IsNullOrWhiteSpace(projectModel.ProjectFilePath))
        {
            throw new ValidationException("Project file path is not set");
        }

        if (PathHelper.GetPathType(projectModel.ProjectFilePath) != PathHelper.PathType.File)
        {
            throw new ValidationException("Project file does not exist");
        }

        var badFiles = projectModel.ProjectFilesPaths
            .Where(p => PathHelper.GetPathType(p) != PathHelper.PathType.File)
            .ToArray();
        if (badFiles.Any())
        {
            throw new ValidationException($"These files do not exist on disk: {string.Join("; ", badFiles)}");
        }

        var badDevices = projectModel.Devices
            .Where(p => PathHelper.GetPathType(p) != PathHelper.PathType.File)
            .ToArray();
        if (badDevices.Any())
        {
            throw new ValidationException($"These files do not exist on disk: {string.Join("; ", badDevices)}");
        }

        if (string.IsNullOrWhiteSpace(projectModel.Executable))
        {
            throw new ValidationException("Executable file is not set");
        }

        if (!projectModel.ProjectFilesPaths.Contains(projectModel.ExecutableFilePath))
        {
            throw new ValidationException("Executable file is not represented in project files");
        }

        if (string.IsNullOrWhiteSpace(projectModel.ProgramAddressString))
        {
            throw new ValidationException("Program start address is not set");
        }

        if (string.IsNullOrWhiteSpace(projectModel.StackAddressString))
        {
            throw new ValidationException("Stack start address is not set");
        }

        // TODO: Check that device is valid
    }
}
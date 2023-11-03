using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Providers;

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
    public async Task ThrowIfInvalid(string projectPath)
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
    public void ThrowIfInvalid(ProjectModel projectModel)
    {
        var unExistingFiles = projectModel.ProjectFilesPaths.Where(p => !File.Exists(p)).ToArray();
        if (unExistingFiles.Any())
        {
            throw new ValidationException($"These files do not exist on disk: {string.Join("; ", unExistingFiles)}");
        }

        var unExistingDevices = projectModel.Devices.Where(p => !File.Exists(p)).ToArray();
        if (unExistingDevices.Any())
        {
            throw new ValidationException($"These files do not exist on disk: {string.Join("; ", unExistingDevices)}");
        }

        if (!projectModel.ProjectFilesPaths.Contains(projectModel.ExecutableFilePath))
        {
            throw new ValidationException("Executable file is not represented in project files");
        }

        // TODO: Check that device is valid
    }
}
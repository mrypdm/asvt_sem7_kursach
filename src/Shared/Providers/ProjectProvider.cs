using System.IO;
using System.Threading.Tasks;
using Shared.Helpers;
using Shared.Models;

namespace Shared.Providers;

/// <inheritdoc />
public class ProjectProvider : IProjectProvider
{
    /// <inheritdoc />
    public async Task<ProjectModel> OpenProjectAsync(string filePath)
    {
        filePath = PathHelper.Combine(Directory.GetCurrentDirectory(), filePath);
        var project = await JsonHelper.DeserializeFileAsync<ProjectModel>(filePath);
        project.Directory = PathHelper.GetDirectoryName(filePath);
        project.ProjectFileName = PathHelper.GetFileName(filePath);
        return project;
    }

    /// <inheritdoc />
    public async Task<(bool isSuccess, ProjectModel projectModel)> TryOpenProjectAsync(string filePath)
    {
        try
        {
            var project = await OpenProjectAsync(filePath);
            return (true, project);
        }
        catch
        {
            return (false, null);
        }
    }
}
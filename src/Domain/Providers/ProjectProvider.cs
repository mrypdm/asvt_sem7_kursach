using System.IO;
using System.Threading.Tasks;
using Domain.Extensions;
using Domain.Models;
using Shared.Helpers;

namespace Domain.Providers;

/// <inheritdoc />
public class ProjectProvider : IProjectProvider
{
    /// <inheritdoc />
    public async Task<IProject> OpenProjectAsync(string filePath)
    {
        filePath = PathHelper.Combine(Directory.GetCurrentDirectory(), filePath);
        var project = await JsonHelper.DeserializeFileAsync<ProjectDto>(filePath);
        return project.ToProject(filePath);
    }

    /// <inheritdoc />
    public async Task<(bool isSuccess, IProject projectModel)> TryOpenProjectAsync(string filePath)
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
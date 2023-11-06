using System.IO;
using System.Threading.Tasks;
using Domain.Models;
using Shared.Helpers;

namespace Domain.Providers;

/// <inheritdoc />
public class ProjectProvider : IProjectProvider
{
    /// <inheritdoc />
    public async Task<Project> OpenProjectAsync(string filePath)
    {
        filePath = PathHelper.Combine(Directory.GetCurrentDirectory(), filePath);
        var project = await JsonHelper.DeserializeFileAsync<Project>(filePath);
        project.ProjectFilePath = filePath;
        return project;
    }

    /// <inheritdoc />
    public async Task<(bool isSuccess, Project projectModel)> TryOpenProjectAsync(string filePath)
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
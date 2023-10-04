using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using GUI.Models;
using Shared.Helpers;

namespace GUI.Managers;

public class ProjectManager
{
    private readonly IStorageProvider _storageProvider;

    public ProjectManager(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }
    
    public ProjectModel Project { get; private set; }

    public async Task CreateProjectAsync(string projectName)
    {
        var projectDir = await _storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Choose project folder...",
            AllowMultiple = false
        });

        if (!projectDir.Any())
        {
            return;
        }

        Project = new ProjectModel
        {
            Name = projectName,
            Directory = projectDir[0].Path.LocalPath
        };

        await JsonHelper.SerializeToFileAsync(Project, Project.ProjectFilePath);
    }

    public async Task OpenProjectAsync()
    {
        var projectFile = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open project file...",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType(ProjectModel.ProjectFileExtension)
                {
                    Patterns = new[] { $"*.{ProjectModel.ProjectFileExtension}" }
                }
            }
        });

        if (!projectFile.Any())
        {
            return;
        }

        Project = await JsonHelper.DeserializeFileAsync<ProjectModel>(projectFile[0].Path.LocalPath);
        Project.Directory = Path.GetDirectoryName(projectFile[0].Path.LocalPath);
    }
}
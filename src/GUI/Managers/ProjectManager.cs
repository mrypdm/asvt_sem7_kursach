using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using GUI.Models;
using Shared.Helpers;

namespace GUI.Managers;

/// <summary>
/// Project manager
/// </summary>
public class ProjectManager
{
    /// <summary>
    /// Instance of project manager
    /// </summary>
    public static ProjectManager Instance { get; private set; }

    private ProjectManager()
    {
    }

    /// <summary>
    /// Creates instance of manager
    /// </summary>
    public static void Create()
    {
        Instance = new ProjectManager();
    }

    /// <summary>
    /// Current project
    /// </summary>
    public ProjectModel Project { get; private set; }

    /// <summary>
    /// Creates new project
    /// </summary>
    /// <param name="storageProvider">Storage provider</param>
    /// <param name="projectName">Name of project</param>
    public async Task CreateProjectAsync(IStorageProvider storageProvider, string projectName)
    {
        var projectDir = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
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

    /// <summary>
    /// Opens project
    /// </summary>
    /// <param name="storageProvider">Storage provider</param>
    public async Task OpenProjectAsync(IStorageProvider storageProvider)
    {
        var projectFile = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
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
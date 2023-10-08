using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using GUI.Models;
using GUI.Notifiers;
using Shared.Helpers;

namespace GUI.Managers;

/// <summary>
/// Project manager
/// </summary>
public class ProjectManager : PropertyChangedNotifier
{
    private ProjectModel _project;

    /// <summary>
    /// Current project
    /// </summary>
    public ProjectModel Project
    {
        get => _project ?? throw new InvalidOperationException("Project is not opened");
        set => SetField(ref _project, value);
    }

    /// <summary>
    /// Is project opened
    /// </summary>
    public bool IsOpened => _project != null;

    /// <summary>
    /// Creates new project
    /// </summary>
    /// <param name="storageProvider">Storage provider</param>
    /// <param name="projectName">Name of project</param>
    public async Task<bool> CreateProjectAsync(IStorageProvider storageProvider, string projectName)
    {
        var projectDir = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Choose project folder...",
            AllowMultiple = false
        });

        if (!projectDir.Any())
        {
            return false;
        }

        var project = new ProjectModel
        {
            Name = projectName,
            Directory = projectDir[0].Path.LocalPath
        };

        await JsonHelper.SerializeToFileAsync(project, project.ProjectFilePath);
        Project = project;

        return true;
    }

    /// <summary>
    /// Opens project
    /// </summary>
    /// <param name="storageProvider">Storage provider</param>
    public async Task<bool> OpenProjectAsync(IStorageProvider storageProvider)
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
            return false;
        }

        await LoadProjectAsync(projectFile[0].Path.LocalPath);
        return true;
    }

    /// <summary>
    /// Load project from disk
    /// </summary>
    public async Task LoadProjectAsync(string projectFilePath)
    {
        projectFilePath = PathHelper.Combine(Directory.GetCurrentDirectory(), projectFilePath);

        try
        {
            var project = await JsonHelper.DeserializeFileAsync<ProjectModel>(projectFilePath);
            project.Directory = PathHelper.GetDirectoryName(projectFilePath);
            Project = project;
        }
        catch (JsonException e)
        {
            throw new FormatException(e.Message, e);
        }
    }

    /// <summary>
    /// Reload project from disk
    /// </summary>
    public Task ReloadProjectAsync() => LoadProjectAsync(Project.ProjectFilePath);

    /// <summary>
    /// Closing project
    /// </summary>
    /// <remarks>This method will not call <see cref="SaveProjectAsync"/></remarks>
    public void CloseProject()
    {
        Project = null;
    }

    /// <summary>
    /// Adds file to project
    /// </summary>
    /// <param name="filePath">File path</param>
    public void AddFileToProject(string filePath)
    {
        if (Project.ProjectFilePath.Contains(filePath))
        {
            return;
        }

        Project.Files.Add(Path.GetRelativePath(Project.Directory, filePath));
    }

    /// <summary>
    /// Remove file from project
    /// </summary>
    /// <param name="filePath">File path</param>
    public void RemoveFileFromProject(string filePath)
    {
        var index = Project.ProjectFilesPaths.IndexOf(filePath);

        if (Project.ExecutableFilePath == filePath)
        {
            Project.Executable = string.Empty;
        }

        Project.Files.RemoveAt(index);
    }

    /// <summary>
    /// Set executable file for project
    /// </summary>
    /// <param name="filePath">File path</param>
    public void SetExecutableFile(string filePath)
    {
        if (Project.ProjectFilesPaths.Contains(filePath))
        {
            Project.Executable = PathHelper.GetRelativePath(Project.Directory, filePath);
        }
        else
        {
            throw new ArgumentException($"The file '{filePath}' does not belong to the project", nameof(filePath));
        }
    }

    /// <summary>
    /// Add external device to project
    /// </summary>
    /// <param name="filePath">Path to external device</param>
    public void AddExternalDevice(string filePath)
    {
        if (Project.ExternalDevices.Contains(filePath))
        {
            return;
        }

        Project.ExternalDevices.Add(filePath);
    }

    /// <summary>
    /// Removes external device from project
    /// </summary>
    /// <param name="filePath">Path to external device</param>
    public void RemoveExternalDevice(string filePath)
    {
        Project.ExternalDevices.Remove(filePath);
    }

    /// <summary>
    /// Saves project to disk and invokes <see cref="PropertyChangedNotifier.PropertyChanged"/>
    /// </summary>
    public async Task SaveProjectAsync()
    {
        await JsonHelper.SerializeToFileAsync(Project, Project.ProjectFilePath);
        OnPropertyChanged(nameof(Project));
    }

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
}
using System;
using System.Collections.Generic;
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
        get => _project;
        set => SetField(ref _project, value);
    }

    public IReadOnlyCollection<string> ExternalDevices => Project.ExternalDevices;

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

        var project = new ProjectModel
        {
            Name = projectName,
            Directory = projectDir[0].Path.LocalPath
        };

        await JsonHelper.SerializeToFileAsync(project, Project.ProjectFilePath);

        Project = project;
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

        await LoadProjectAsync(projectFile[0].Path.LocalPath);
    }

    /// <summary>
    /// Load project from disk
    /// </summary>
    public async Task LoadProjectAsync(string projectFilePath)
    {
        try
        {
            var project = await JsonHelper.DeserializeFileAsync<ProjectModel>(projectFilePath);
            project.Directory = Path.GetDirectoryName(projectFilePath);
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
        Project.Files.Add(Path.GetRelativePath(Project.Directory, filePath));
    }

    /// <summary>
    /// Remove file from project
    /// </summary>
    /// <param name="filePath">File path</param>
    public void RemoveFileFromProject(string filePath)
    {
        Project.Files.Remove(Path.GetRelativePath(Project.Directory, filePath));
    }

    /// <summary>
    /// Set executable file for project
    /// </summary>
    /// <param name="filePath">File path</param>
    public void SetExecutableFile(string filePath)
    {
        Project.Executable = Path.GetRelativePath(Project.Directory, filePath);
    }

    /// <summary>
    /// Add external device to project
    /// </summary>
    /// <param name="filePath">Path to external device</param>
    public void AddExternalDevice(string filePath)
    {
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
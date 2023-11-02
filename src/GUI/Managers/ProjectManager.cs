using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using GUI.Notifiers;
using Shared.Helpers;
using Shared.Models;
using Shared.Providers;

namespace GUI.Managers;

/// <summary>
/// Project manager
/// </summary>
public class ProjectManager : PropertyChangedNotifier
{
    private readonly IProjectProvider _provider;
    private ProjectModel _project;

    public ProjectManager(IProjectProvider provider)
    {
        _provider = provider;
    }

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
            ProjectFileName = $"{projectName}.{ProjectModel.ProjectFileExtension}",
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
        Project = await _provider.OpenProjectAsync(projectFilePath);
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
    /// Saves project to disk and invokes <see cref="PropertyChangedNotifier.PropertyChanged"/>
    /// </summary>
    public async Task SaveProjectAsync()
    {
        await JsonHelper.SerializeToFileAsync(Project, Project.ProjectFilePath);
        OnPropertyChanged(nameof(Project));
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
    /// Add device to project
    /// </summary>
    /// <param name="filePath">Path to device</param>
    public void AddDeviceToProject(string filePath)
    {
        if (Project.Devices.Contains(filePath))
        {
            return;
        }

        Project.Devices.Add(filePath);
    }

    /// <summary>
    /// Removes device from project
    /// </summary>
    /// <param name="filePath">Path to device</param>
    public void RemoveDeviceFromProject(string filePath)
    {
        Project.Devices.Remove(filePath);
    }
}
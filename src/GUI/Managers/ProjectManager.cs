using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Domain.Models;
using Domain.Providers;
using GUI.Notifiers;
using Shared.Helpers;

namespace GUI.Managers;

/// <inheritdoc cref="IProjectManager" />
public class ProjectManager : PropertyChangedNotifier, IProjectManager
{
    private readonly IProjectProvider _provider;
    private Project _project;

    /// <summary>
    /// Creates new instance of <see cref="ProjectManager"/>
    /// </summary>
    /// <param name="provider">Project provider</param>
    public ProjectManager(IProjectProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <inheritdoc />
    public Project Project
    {
        get => _project ?? throw new InvalidOperationException("Project is not opened");
        private set => SetField(ref _project, value);
    }

    /// <inheritdoc />
    public bool IsOpened => _project != null;

    /// <inheritdoc />
    public async Task<bool> CreateProjectAsync(IStorageProvider storageProvider, string projectName)
    {
        if (storageProvider == null)
        {
            throw new ArgumentNullException(nameof(storageProvider));
        }

        if (string.IsNullOrWhiteSpace(projectName))
        {
            throw new ArgumentException("Project name cannot be empty", nameof(projectName));
        }

        var projectDir = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Choose project folder...",
            AllowMultiple = false
        });

        if (!projectDir.Any())
        {
            return false;
        }

        var filePath =
            PathHelper.Combine(projectDir[0].Path.LocalPath, $"{projectName}.{Project.ProjectFileExtension}");
        var project = new Project
        {
            ProjectFilePath = filePath
        };

        await JsonHelper.SerializeToFileAsync(project, filePath);
        Project = project;

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> OpenProjectAsync(IStorageProvider storageProvider)
    {
        if (storageProvider == null)
        {
            throw new ArgumentNullException(nameof(storageProvider));
        }

        var projectFile = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open project file...",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType(Project.ProjectFileExtension)
                {
                    Patterns = new[] { $"*.{Project.ProjectFileExtension}" }
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

    /// <inheritdoc />
    public async Task LoadProjectAsync(string projectFilePath)
    {
        Project = await _provider.OpenProjectAsync(projectFilePath);
    }

    /// <inheritdoc />
    public Task ReloadProjectAsync() => LoadProjectAsync(Project.ProjectFilePath);

    /// <inheritdoc />
    public async Task SaveProjectAsync()
    {
        await JsonHelper.SerializeToFileAsync(Project, Project.ProjectFilePath);
        await ReloadProjectAsync();
    }

    /// <inheritdoc />
    public void AddFileToProject(string filePath)
    {
        filePath = PathHelper.GetFullPath(filePath);
        if (Project.ProjectFilesPaths.Contains(filePath))
        {
            return;
        }

        Project.Files.Add(Path.GetRelativePath(Project.Directory, filePath));
        OnPropertyChanged(nameof(Project.Files));
    }

    /// <inheritdoc />
    public void RemoveFileFromProject(string filePath)
    {
        filePath = PathHelper.GetFullPath(filePath);
        var index = Project.ProjectFilesPaths.IndexOf(filePath);

        if (index == -1)
        {
            return;
        }

        if (Project.ExecutableFilePath == filePath)
        {
            Project.Executable = string.Empty;
        }

        Project.Files.RemoveAt(index);
        OnPropertyChanged(nameof(Project.Files));
    }

    /// <inheritdoc />
    public void SetExecutableFile(string filePath)
    {
        filePath = PathHelper.GetFullPath(filePath);
        if (Project.ProjectFilesPaths.Contains(filePath))
        {
            Project.Executable = filePath;
            OnPropertyChanged(nameof(Project.Executable));
        }
        else
        {
            throw new ArgumentException($"The file '{filePath}' does not belong to the project", nameof(filePath));
        }
    }

    /// <inheritdoc />
    public void AddDeviceToProject(string filePath)
    {
        filePath = PathHelper.GetFullPath(filePath);
        if (Project.Devices.Contains(filePath))
        {
            return;
        }

        Project.Devices.Add(filePath);
        OnPropertyChanged(nameof(Project.Devices));
    }

    /// <inheritdoc />
    public void RemoveDeviceFromProject(string filePath)
    {
        filePath = PathHelper.GetFullPath(filePath);
        Project.Devices.Remove(filePath);
        OnPropertyChanged(nameof(Project.Devices));
    }
}
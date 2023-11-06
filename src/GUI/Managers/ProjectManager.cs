using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Domain.Extensions;
using Domain.Models;
using Domain.Providers;
using GUI.Notifiers;
using Shared.Helpers;

namespace GUI.Managers;

/// <inheritdoc cref="IProjectManager" />
public class ProjectManager : PropertyChangedNotifier, IProjectManager
{
    public const string ProjectExtension = "pdp11proj";

    private readonly IProjectProvider _provider;
    private Project _project;

    private Project SafeProject => _project ?? throw new InvalidOperationException("Project is not opened");

    /// <summary>
    /// Creates new instance of <see cref="ProjectManager"/>
    /// </summary>
    /// <param name="provider">Project provider</param>
    public ProjectManager(IProjectProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <inheritdoc />
    public IProject Project
    {
        get => SafeProject;
        private set => SetField(ref _project, value as Project);
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
            PathHelper.Combine(projectDir[0].Path.LocalPath, $"{projectName}.{ProjectExtension}");
        var project = new Project
        {
            ProjectFile = filePath
        };

        await project.ToJsonAsync();
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
                new FilePickerFileType(ProjectExtension)
                {
                    Patterns = new[] { $"*.{ProjectExtension}" }
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
    public Task ReloadProjectAsync() => LoadProjectAsync(SafeProject.ProjectFile);

    /// <inheritdoc />
    public async Task SaveProjectAsync()
    {
        await SafeProject.ToJsonAsync();
        OnPropertyChanged(nameof(Project));
    }

    /// <inheritdoc />
    public void AddFileToProject(string filePath)
    {
        filePath = PathHelper.GetFullPath(filePath);
        if (SafeProject.Files.Contains(filePath))
        {
            return;
        }

        SafeProject.Files.Add(filePath);
        OnPropertyChanged(nameof(SafeProject.Files));
    }

    /// <inheritdoc />
    public void RemoveFileFromProject(string filePath)
    {
        filePath = PathHelper.GetFullPath(filePath);

        if (SafeProject.Executable == filePath)
        {
            SafeProject.Executable = string.Empty;
            OnPropertyChanged(nameof(SafeProject.Executable));
        }

        SafeProject.Files.Remove(filePath);
        OnPropertyChanged(nameof(SafeProject.Files));
    }

    /// <inheritdoc />
    public void SetExecutableFile(string filePath)
    {
        filePath = PathHelper.GetFullPath(filePath);
        if (SafeProject.Files.Contains(filePath))
        {
            SafeProject.Executable = filePath;
            OnPropertyChanged(nameof(SafeProject.Executable));
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
        if (SafeProject.Devices.Contains(filePath))
        {
            return;
        }

        SafeProject.Devices.Add(filePath);
        OnPropertyChanged(nameof(SafeProject.Devices));
    }

    /// <inheritdoc />
    public void RemoveDeviceFromProject(string filePath)
    {
        filePath = PathHelper.GetFullPath(filePath);
        SafeProject.Devices.Remove(filePath);
        OnPropertyChanged(nameof(SafeProject.Devices));
    }
}
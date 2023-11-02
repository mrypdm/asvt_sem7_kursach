using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Domain.Models;
using GUI.Notifiers;
using Shared.Helpers;
using Shared.Providers;

namespace GUI.Managers;

/// <inheritdoc cref="IProjectManager" />
public class ProjectManager : PropertyChangedNotifier, IProjectManager
{
    private readonly IProjectProvider _provider;
    private ProjectModel _project;

    /// <summary>
    /// Creates new instance of <see cref="ProjectManager"/>
    /// </summary>
    /// <param name="provider">Project provider</param>
    public ProjectManager(IProjectProvider provider)
    {
        _provider = provider;
    }

    /// <inheritdoc />
    public ProjectModel Project
    {
        get => _project ?? throw new InvalidOperationException("Project is not opened");
        set => SetField(ref _project, value);
    }

    /// <inheritdoc />
    public bool IsOpened => _project != null;

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task LoadProjectAsync(string projectFilePath)
    {
        Project = await _provider.OpenProjectAsync(projectFilePath);
    }

    /// <inheritdoc />
    public Task ReloadProjectAsync() => LoadProjectAsync(Project.ProjectFilePath);

    /// <inheritdoc />
    public void CloseProject()
    {
        Project = null;
    }

    /// <inheritdoc />
    public async Task SaveProjectAsync()
    {
        await JsonHelper.SerializeToFileAsync(Project, Project.ProjectFilePath);
        await ReloadProjectAsync();
    }

    /// <inheritdoc />
    public void AddFileToProject(string filePath)
    {
        if (Project.ProjectFilePath.Contains(filePath))
        {
            return;
        }

        Project.Files.Add(Path.GetRelativePath(Project.Directory, filePath));
        OnPropertyChanged(nameof(Project));
    }

    /// <inheritdoc />
    public void RemoveFileFromProject(string filePath)
    {
        var index = Project.ProjectFilesPaths.IndexOf(filePath);

        if (Project.ExecutableFilePath == filePath)
        {
            Project.Executable = string.Empty;
        }

        Project.Files.RemoveAt(index);
        OnPropertyChanged(nameof(Project));
    }

    /// <inheritdoc />
    public void SetExecutableFile(string filePath)
    {
        if (Project.ProjectFilesPaths.Contains(filePath))
        {
            Project.Executable = PathHelper.GetRelativePath(Project.Directory, filePath);
            OnPropertyChanged(nameof(Project));
        }
        else
        {
            throw new ArgumentException($"The file '{filePath}' does not belong to the project", nameof(filePath));
        }
    }

    /// <inheritdoc />
    public void AddDeviceToProject(string filePath)
    {
        if (Project.Devices.Contains(filePath))
        {
            return;
        }

        Project.Devices.Add(filePath);
        OnPropertyChanged(nameof(Project));
    }

    /// <inheritdoc />
    public void RemoveDeviceFromProject(string filePath)
    {
        Project.Devices.Remove(filePath);
        OnPropertyChanged(nameof(Project));
    }
}
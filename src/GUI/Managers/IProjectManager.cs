using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Domain.Models;
using GUI.Notifiers;

namespace GUI.Managers;

/// <summary>
/// Manager for <see cref="ProjectModel"/>
/// </summary>
public interface IProjectManager : INotifyPropertyChanged
{
    /// <summary>
    /// Current project
    /// </summary>
    ProjectModel Project { get; }

    /// <summary>
    /// Is project opened
    /// </summary>
    bool IsOpened { get; }

    /// <summary>
    /// Creates new project
    /// </summary>
    /// <param name="storageProvider">Storage provider</param>
    /// <param name="projectName">Name of project</param>
    Task<bool> CreateProjectAsync(IStorageProvider storageProvider, string projectName);

    /// <summary>
    /// Opens project
    /// </summary>
    /// <param name="storageProvider">Storage provider</param>
    Task<bool> OpenProjectAsync(IStorageProvider storageProvider);

    /// <summary>
    /// Load project from disk
    /// </summary>
    Task LoadProjectAsync(string projectFilePath);

    /// <summary>
    /// Reload project from disk
    /// </summary>
    Task ReloadProjectAsync();

    /// <summary>
    /// Closing project
    /// </summary>
    /// <remarks>This method will not call <see cref="ProjectManager.SaveProjectAsync"/></remarks>
    void CloseProject();

    /// <summary>
    /// Saves project to disk and invokes <see cref="PropertyChangedNotifier.PropertyChanged"/>
    /// </summary>
    Task SaveProjectAsync();

    /// <summary>
    /// Adds file to project
    /// </summary>
    /// <param name="filePath">File path</param>
    void AddFileToProject(string filePath);

    /// <summary>
    /// Remove file from project
    /// </summary>
    /// <param name="filePath">File path</param>
    void RemoveFileFromProject(string filePath);

    /// <summary>
    /// Set executable file for project
    /// </summary>
    /// <param name="filePath">File path</param>
    void SetExecutableFile(string filePath);

    /// <summary>
    /// Add device to project
    /// </summary>
    /// <param name="filePath">Path to device</param>
    void AddDeviceToProject(string filePath);

    /// <summary>
    /// Removes device from project
    /// </summary>
    /// <param name="filePath">Path to device</param>
    void RemoveDeviceFromProject(string filePath);
}
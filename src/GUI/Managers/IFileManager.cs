using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using GUI.Models;

namespace GUI.Managers;

/// <summary>
/// Manager for <see cref="FileModel"/>
/// </summary>
public interface IFileManager
{
    /// <summary>
    /// Selects file on disk for saving
    /// </summary>
    /// <param name="storageProvider">Provider for files and directories</param>
    /// <param name="options">File picker options. Only <see cref="FilePickerOpenOptions"/> and <see cref="FilePickerSaveOptions"/> supported</param>
    /// <remarks>If <paramref name="options"/> is <see cref="FilePickerOpenOptions"/>, <see cref="FilePickerOpenOptions.AllowMultiple"/> must be false</remarks>
    /// <returns>Path to file</returns>
    /// <exception cref="InvalidOperationException">If <paramref name="options"/> is of invalid type or <see cref="FilePickerOpenOptions.AllowMultiple"/> is true</exception>
    Task<string> GetFileAsync(IStorageProvider storageProvider, PickerOptions options);

    /// <summary>
    /// Creates new file
    /// </summary>
    /// <param name="storageProvider">Provider for files and directories</param>
    /// <param name="directoryPath">Initial directory</param>
    /// <param name="fileName">Initial file name</param>
    /// <returns>File info</returns>
    Task<FileModel> CreateFile(IStorageProvider storageProvider, string directoryPath, string fileName);

    /// <summary>
    /// Opens file
    /// </summary>
    /// <param name="storageProvider">Provider for files and directories</param>
    /// <returns>File info</returns>
    Task<ICollection<FileModel>> OpenFilesAsync(IStorageProvider storageProvider);

    /// <summary>
    /// Open file on path
    /// </summary>
    /// <param name="filePath">File path</param>
    /// <returns>File info</returns>
    Task<FileModel> OpenFileAsync(string filePath);

    /// <summary>
    /// Flushes file to disk
    /// </summary>
    /// <param name="file">File info</param>
    Task WriteFileAsync(FileModel file);

    /// <summary>
    /// Deletes file
    /// </summary>
    /// <param name="file">File info</param>
    Task DeleteAsync(FileModel file);
}
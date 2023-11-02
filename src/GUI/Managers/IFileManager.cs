using System.Collections.Generic;
using System.Threading.Tasks;
using GUI.Models;

namespace GUI.Managers;

/// <summary>
/// Manager for <see cref="FileModel"/>
/// </summary>
public interface IFileManager
{
    /// <summary>
    /// Selects file on disk
    /// </summary>
    /// <param name="directoryPath">Initial directory</param>
    /// <param name="fileName">Initial file name</param>
    /// <returns>Path to file</returns>
    Task<string> GetFileAsync(string directoryPath, string fileName);

    /// <summary>
    /// Creates new file
    /// </summary>
    /// <param name="directoryPath">Initial directory</param>
    /// <param name="fileName">Initial file name</param>
    /// <returns>File info</returns>
    Task<FileModel> CreateFile(string directoryPath, string fileName);

    /// <summary>
    /// Opens file
    /// </summary>
    /// <returns>File info</returns>
    Task<ICollection<FileModel>> OpenFilesAsync();

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
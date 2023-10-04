using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using GUI.Models;

namespace GUI.Managers;

/// <summary>
/// File manager
/// </summary>
public class FileManager
{
    private readonly IStorageProvider _storageProvider;

    /// <summary>
    /// Creates new instance of file manager
    /// </summary>
    /// <param name="storageProvider">File System storage service used for file pickers and bookmarks</param>
    public FileManager(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    /// <summary>
    /// Opens file
    /// </summary>
    /// <returns>File info</returns>
    public async Task<ICollection<FileModel>> OpenFilesAsync()
    {
        var files = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open files...",
            AllowMultiple = true
        });

        if (!files.Any())
        {
            return null;
        }

        var filesList = new List<FileModel>();

        foreach (var file in files)
        {
            filesList.Add(await OpenFileAsync(file.Path.LocalPath));
        }

        return filesList;
    }

    public async Task<FileModel> OpenFileAsync(string filePath) => new(Path.GetFileName(filePath))
    {
        FilePath = filePath,
        Text = await File.ReadAllTextAsync(filePath)
    };

    /// <summary>
    /// Creates new file
    /// </summary>
    /// <param name="fileName">Initial file name</param>
    /// <returns>Path to file</returns>
    public async Task<string> CreateFile(string fileName)
    {
        var newFile = await _storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save file as...",
            ShowOverwritePrompt = true,
            SuggestedFileName = fileName,
            DefaultExtension = "asm"
        });

        return newFile?.Path.LocalPath;
    }

    /// <summary>
    /// Flushes file to disk
    /// </summary>
    /// <param name="file">File info</param>
    public async Task WriteFileAsync(FileModel file)
    {
        await File.WriteAllTextAsync(file.FilePath, file.Text);
        file.IsNeedSave = false;
    }

    /// <summary>
    /// Deletes file
    /// </summary>
    /// <param name="file">File info</param>
    public void Delete(FileModel file)
    {
        if (file.FilePath != null)
        {
            File.Delete(file.FilePath);
        }
    }
}
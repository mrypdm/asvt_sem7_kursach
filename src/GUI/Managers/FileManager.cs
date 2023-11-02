using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using GUI.Models;

namespace GUI.Managers;

/// <inheritdoc />
public class FileManager : IFileManager
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

    /// <inheritdoc />
    public async Task<string> GetFileAsync(string directoryPath, string fileName)
    {
        var newFile = await _storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save file as...",
            ShowOverwritePrompt = true,
            SuggestedFileName = fileName,
            SuggestedStartLocation = await _storageProvider.TryGetFolderFromPathAsync(directoryPath)
        });

        return newFile?.Path.LocalPath;
    }

    /// <inheritdoc />
    public async Task<FileModel> CreateFile(string directoryPath, string fileName)
    {
        var filePath = await GetFileAsync(directoryPath, fileName);

        if (filePath == null)
        {
            return null;
        }

        var file = new FileModel
        {
            FilePath = filePath
        };

        await WriteFileAsync(file);

        return file;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<FileModel> OpenFileAsync(string filePath) => new()
    {
        FilePath = filePath,
        Text = await File.ReadAllTextAsync(filePath)
    };

    /// <inheritdoc />
    public async Task WriteFileAsync(FileModel file)
    {
        await File.WriteAllTextAsync(file.FilePath, file.Text);
        file.IsNeedSave = false;
    }

    /// <inheritdoc />
    public Task DeleteAsync(FileModel file) => Task.Run(() => File.Delete(file.FilePath));
}
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
    /// <inheritdoc />
    public async Task<string> GetFileAsync(IStorageProvider storageProvider, string directoryPath, string fileName)
    {
        var newFile = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save file as...",
            ShowOverwritePrompt = true,
            SuggestedFileName = fileName,
            SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(directoryPath)
        });

        return newFile?.Path.LocalPath;
    }

    /// <inheritdoc />
    public async Task<FileModel> CreateFile(IStorageProvider storageProvider, string directoryPath, string fileName)
    {
        var filePath = await GetFileAsync(storageProvider, directoryPath, fileName);

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
    public async Task<ICollection<FileModel>> OpenFilesAsync(IStorageProvider storageProvider)
    {
        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
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
using System;
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
    public async Task<string> GetFileAsync(IStorageProvider storageProvider, PickerOptions options)
    {
        if (storageProvider == null)
        {
            throw new ArgumentNullException(nameof(storageProvider));
        }

        switch (options)
        {
            case FilePickerSaveOptions saveOptions:
            {
                var newFile = await storageProvider.SaveFilePickerAsync(saveOptions);
                return newFile?.Path.LocalPath;
            }
            case FilePickerOpenOptions { AllowMultiple: true }:
                throw new InvalidOperationException($"{nameof(FilePickerOpenOptions.AllowMultiple)} must be false");
            case FilePickerOpenOptions openOptions:
            {
                var file = await storageProvider.OpenFilePickerAsync(openOptions);
                return file.Any() ? file[0].Path.LocalPath : null;
            }
            default:
                throw new InvalidOperationException($"Invalid type of {nameof(options)} - {options.GetType().Name}");
        }
    }

    /// <inheritdoc />
    public async Task<FileModel> CreateFile(IStorageProvider storageProvider, string directoryPath, string fileName)
    {
        if (storageProvider == null)
        {
            throw new ArgumentNullException(nameof(storageProvider));
        }

        var options = new FilePickerSaveOptions
        {
            Title = "Create file...",
            ShowOverwritePrompt = true,
            SuggestedFileName = fileName,
            SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(directoryPath)
        };

        var filePath = await GetFileAsync(storageProvider, options);

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
        if (storageProvider == null)
        {
            throw new ArgumentNullException(nameof(storageProvider));
        }

        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open files...",
            AllowMultiple = true
        });

        if (!files.Any())
        {
            return Array.Empty<FileModel>();
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
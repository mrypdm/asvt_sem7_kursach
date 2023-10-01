using System.IO;
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
    /// Creates empty file
    /// </summary>
    /// <returns>File info</returns>
    public async Task<FileModel> CreateFileAsync()
    {
        var file = await _storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Create file...",
            ShowOverwritePrompt = true
        });

        if (file == null)
        {
            return null;
        }

        var fileModel = new FileModel
        {
            FilePath = file.Path.AbsolutePath,
            Text = string.Empty
        };

        await SaveFileAsync(fileModel);

        return fileModel;
    }

    /// <summary>
    /// Opens file
    /// </summary>
    /// <returns>File info</returns>
    public async Task<FileModel> OpenFileAsync()
    {
        var files = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open file...",
            AllowMultiple = false
        });

        if (files.Count != 1)
        {
            return null;
        }

        return await OpenFileAsync(files[0].Path.AbsolutePath);
    }

    /// <summary>
    /// Open file on path
    /// </summary>
    /// <param name="filePath">Path ot file</param>
    /// <returns>File info</returns>
    public async Task<FileModel> OpenFileAsync(string filePath)
    {
        return new FileModel
        {
            FilePath = filePath,
            Text = await File.ReadAllTextAsync(filePath)
        };
    }

    /// <summary>
    /// Saves file
    /// </summary>
    /// <param name="file">File info</param>
    public async Task SaveFileAsync(FileModel file)
    {
        if (file.FilePath != null)
        {
            await File.WriteAllTextAsync(file.FilePath, file.Text);
        }
        else
        {
            await SaveFileAsAsync(file);
        }
    }

    /// <summary>
    /// Saves file on new path and update file info
    /// </summary>
    /// <param name="fileModel">File info</param>
    public async Task SaveFileAsAsync(FileModel fileModel)
    {
        var file = await _storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save file as...",
            ShowOverwritePrompt = true,
            SuggestedFileName = fileModel.FileName
        });

        if (file == null)
        {
            return;
        }

        fileModel.FilePath = file.Path.AbsolutePath;

        await SaveFileAsync(fileModel);
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
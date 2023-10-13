using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Models;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="FileTab"/>
/// </summary>
public class FileTabViewModel
{
    /// <summary>
    /// Constructor for designer
    /// </summary>
    public FileTabViewModel()
    {
    }

    /// <summary>
    /// Creates new instance of view model
    /// </summary>
    /// <param name="fileTab">File tab</param>
    /// <param name="file">File model</param>
    /// <param name="selectCommand">Command to select tab</param>
    /// <param name="closeCommand">Command to close tab</param>
    public FileTabViewModel(FileTab fileTab, FileModel file, Func<FileTab, Task> selectCommand, Func<FileTab, Task> closeCommand)
    {
        File = file;
        SelectTabCommand = ReactiveCommand.CreateFromTask(async () => await selectCommand(fileTab));
        CloseTabCommand = ReactiveCommand.CreateFromTask(async () => await closeCommand(fileTab));
    }
    
    /// <summary>
    /// File model
    /// </summary>
    public FileModel File { get; }

    /// <summary>
    /// Command for select tab
    /// </summary>
    public ICommand SelectTabCommand { get; }
    
    /// <summary>
    /// Command for closing tab
    /// </summary>
    public ICommand CloseTabCommand { get; }
}
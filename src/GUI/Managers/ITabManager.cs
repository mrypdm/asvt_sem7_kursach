using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using GUI.Exceptions;
using GUI.Models;
using GUI.ViewModels;

namespace GUI.Managers;

/// <summary>
/// Manager for <see cref="FileTabViewModel"/>
/// </summary>
public interface ITabManager : INotifyPropertyChanged
{
    /// <summary>
    /// Current selected tab
    /// </summary>
    FileTabViewModel Tab { get; set; }

    /// <summary>
    /// Collection of all tabs
    /// </summary>
    ObservableCollection<FileTabViewModel> Tabs { get; }

    /// <summary>
    /// Creates new tab for file and put it into <see cref="TabManager.Tabs"/>
    /// </summary>
    /// <param name="file">File info (or default with file name 'new_N')</param>
    /// <param name="selectCommand">Command on tab selection</param>
    /// <param name="closeCommand">Command on tab closing</param>
    /// <returns>Created tab</returns>
    /// <exception cref="TabExistsException">If tab for file already exists</exception>
    FileTabViewModel CreateTab(FileModel file, Func<FileTabViewModel, Task> selectCommand,
        Func<FileTabViewModel, Task> closeCommand);

    /// <summary>
    /// Deletes tab. If there are no tabs left, it creates an empty tab.
    /// </summary>
    /// <param name="tab">Tab reference</param>
    void DeleteTab(FileTabViewModel tab);

    /// <summary>
    /// Changes current tab
    /// </summary>
    /// <param name="tab">Tab reference</param>
    void SelectTab(FileTabViewModel tab);
}
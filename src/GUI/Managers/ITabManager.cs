using System.Collections.ObjectModel;
using GUI.Exceptions;
using GUI.Models;
using GUI.ViewModels;

namespace GUI.Managers;

/// <summary>
/// Manager for <see cref="FileTabViewModel"/>
/// </summary>
public interface ITabManager
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
    /// <returns>Created tab</returns>
    /// <exception cref="TabExistsException">If tab for file already exists</exception>
    FileTabViewModel CreateTab(FileModel file = null);

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
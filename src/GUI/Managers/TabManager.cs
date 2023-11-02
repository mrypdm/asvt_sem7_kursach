using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GUI.Exceptions;
using GUI.Models;
using GUI.Notifiers;
using GUI.Views;

namespace GUI.Managers;

/// <summary>
/// Tab manager
/// </summary>
public class TabManager : PropertyChangedNotifier
{
    private readonly Func<FileTab, Task> _selectCommand;
    private readonly Func<FileTab, Task> _closeCommand;

    private FileTab _tab;

    /// <summary>
    /// Creates new instance of tab manager
    /// </summary>
    /// <param name="selectCommand">Command to select tab</param>
    /// <param name="closeCommand">Command to close tab</param>
    public TabManager(Func<FileTab, Task> selectCommand, Func<FileTab, Task> closeCommand)
    {
        _selectCommand = selectCommand;
        _closeCommand = closeCommand;
    }

    /// <summary>
    /// Current selected tab
    /// </summary>
    public FileTab Tab
    {
        get => _tab;
        private set => SetField(ref _tab, value);
    }

    /// <summary>
    /// Collection of all tabs
    /// </summary>
    public ObservableCollection<FileTab> Tabs { get; } = new();

    /// <summary>
    /// Creates new tab for file and put it into <see cref="Tabs"/>
    /// </summary>
    /// <param name="file">File info (or default with file name 'new_N')</param>
    /// <returns>Created tab</returns>
    /// <exception cref="TabExistsException">If tab for file already exists</exception>
    public FileTab CreateTab(FileModel file = null)
    {
        if (file != null)
        {
            var existingTab = Tabs.SingleOrDefault(t => t.File.FilePath == file.FilePath);
            if (existingTab != null)
            {
                throw new TabExistsException("Tab for that file already exists")
                {
                    Tab = existingTab
                };
            }
        }

        var tab = new FileTab(file ?? new FileModel(), _selectCommand, _closeCommand);

        Tabs.Add(tab);

        return tab;
    }

    /// <summary>
    /// Deletes tab. If there are no tabs left, it creates an empty tab.
    /// </summary>
    /// <param name="tab">Tab reference</param>
    public void DeleteTab(FileTab tab)
    {
        var index = Tabs.IndexOf(tab) - 1;

        Tabs.Remove(tab);

        var tabToSelect = Tabs.ElementAtOrDefault(index == -1 ? 0 : index);
        SelectTab(tabToSelect);
    }

    /// <summary>
    /// Changes current tab
    /// </summary>
    /// <param name="tab">Tab reference</param>
    public void SelectTab(FileTab tab)
    {
        Tab?.ViewModel.SetTabUnselected();
        Tab = tab;
        Tab?.ViewModel.SetTabSelected();
    }
}
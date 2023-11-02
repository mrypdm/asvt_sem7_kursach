using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GUI.Exceptions;
using GUI.Models;
using GUI.Notifiers;
using GUI.ViewModels;
using GUI.Views;

namespace GUI.Managers;

/// <summary>
/// Tab manager
/// </summary>
public class TabManager : PropertyChangedNotifier
{
    private readonly Func<FileTabViewModel, Task> _selectCommand;
    private readonly Func<FileTabViewModel, Task> _closeCommand;

    private FileTabViewModel _tab;

    /// <summary>
    /// Creates new instance of tab manager
    /// </summary>
    /// <param name="selectCommand">Command to select tab</param>
    /// <param name="closeCommand">Command to close tab</param>
    public TabManager(Func<FileTabViewModel, Task> selectCommand, Func<FileTabViewModel, Task> closeCommand)
    {
        _selectCommand = selectCommand;
        _closeCommand = closeCommand;
    }

    /// <summary>
    /// Current selected tab
    /// </summary>
    public FileTabViewModel Tab
    {
        get => _tab;
        set => SetField(ref _tab, value);
    }

    /// <summary>
    /// Collection of all tabs
    /// </summary>
    public ObservableCollection<FileTabViewModel> Tabs { get; } = new();

    /// <summary>
    /// Creates new tab for file and put it into <see cref="Tabs"/>
    /// </summary>
    /// <param name="file">File info (or default with file name 'new_N')</param>
    /// <returns>Created tab</returns>
    /// <exception cref="TabExistsException">If tab for file already exists</exception>
    public FileTabViewModel CreateTab(FileModel file = null)
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

        var viewModel = new FileTabViewModel(new FileTab(), file ?? new FileModel(), _selectCommand, _closeCommand);
        Tabs.Add(viewModel);
        return viewModel;
    }

    /// <summary>
    /// Deletes tab. If there are no tabs left, it creates an empty tab.
    /// </summary>
    /// <param name="tab">Tab reference</param>
    public void DeleteTab(FileTabViewModel tab)
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
    public void SelectTab(FileTabViewModel tab)
    {
        Tab?.SetTabUnselected();
        Tab = tab;
        Tab?.SetTabSelected();
    }
}
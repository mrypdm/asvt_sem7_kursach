using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media;
using GUI.Controls;
using GUI.Exceptions;
using GUI.Models;

namespace GUI.Managers;

/// <summary>
/// Tab manager
/// </summary>
public class TabManager
{
    private static readonly IBrush DefaultBackground = new SolidColorBrush(Colors.White);
    private static readonly IBrush SelectedBackground = new SolidColorBrush(Colors.LightSkyBlue, 0.5D);

    private readonly ICommand _defaultCommand;

    /// <summary>
    /// Creates new instance of tab manager
    /// </summary>
    /// <param name="defaultCommand">Default tab command</param>
    public TabManager(ICommand defaultCommand)
    {
        _defaultCommand = defaultCommand;
        SelectTab(CreateTab(new FileModel()));
    }

    /// <summary>
    /// Current selected tab
    /// </summary>
    public FileTab CurrentTab { get; private set; }

    /// <summary>
    /// Collection of all tabs
    /// </summary>
    public ObservableCollection<FileTab> Tabs { get; } = new();

    /// <summary>
    /// Creates new tab for file and put it into <see cref="Tabs"/>
    /// </summary>
    /// <param name="file">File info</param>
    /// <param name="command">Tab command (or default, if null)</param>
    /// <returns>Created tab</returns>
    /// <exception cref="TabExistsException">If tab for file already exists</exception>
    public FileTab CreateTab(FileModel file, ICommand command = null)
    {
        var tab = Tabs.SingleOrDefault(t => t.Tag as FileModel == file);
        if (tab != null)
        {
            throw new TabExistsException("Tab for that file already exists")
            {
                Tab = tab
            };
        }

        tab = new FileTab(file)
        {
            Header = file.FileName,
            Margin = new Thickness(5, 0, 5, 0),
            Command = command ?? _defaultCommand
        };
        tab.CommandParameter = tab;

        Tabs.Add(tab);

        return tab;
    }

    /// <summary>
    /// Deletes tab. If there are no tabs left, it creates an empty tab.
    /// </summary>
    /// <param name="tab">Tab reference</param>
    public void DeleteTab(FileTab tab)
    {
        Tabs.Remove(tab);

        if (!Tabs.Any())
        {
            SelectTab(CreateTab(new FileModel()));
        }
    }

    /// <summary>
    /// Changes current tab
    /// </summary>
    /// <param name="tab">Tab reference</param>
    public void SelectTab(FileTab tab)
    {
        if (CurrentTab != null)
        {
            CurrentTab.Background = DefaultBackground;
        }

        CurrentTab = tab;
        CurrentTab.Background = SelectedBackground;
    }
}
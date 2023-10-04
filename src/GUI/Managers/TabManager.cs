using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Avalonia.Media;
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
    public static readonly IBrush DefaultBackground = new SolidColorBrush(Colors.White);
    public static readonly IBrush SelectedBackground = new SolidColorBrush(Colors.LightGray, 0.5D);

    public static readonly IBrush DefaultForeground = new SolidColorBrush(Colors.Black);
    public static readonly IBrush NeedSaveForeground = new SolidColorBrush(Colors.DodgerBlue);

    private readonly ICommand _defaultCommand;

    private FileTab _tab;

    /// <summary>
    /// Creates new instance of tab manager
    /// </summary>
    /// <param name="defaultCommand">Default tab command</param>
    public TabManager(ICommand defaultCommand)
    {
        _defaultCommand = defaultCommand;
        SelectTab(CreateTab());
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
    /// <param name="command">Tab command (or default, if null)</param>
    /// <returns>Created tab</returns>
    /// <exception cref="TabExistsException">If tab for file already exists</exception>
    public FileTab CreateTab(FileModel file = null, ICommand command = null)
    {
        if (file?.FilePath != null)
        {
            var existingTab = Tabs.SingleOrDefault(t => t.File == file);
            if (existingTab != null)
            {
                throw new TabExistsException("Tab for that file already exists")
                {
                    Tab = existingTab
                };
            }
        }

        file ??= new FileModel($"new {GenerateNewTabNumber()}");

        var tab = new FileTab(file)
        {
            Content = file.FileName,
            MinWidth = 100,
            Command = command ?? _defaultCommand
        };
        tab.CommandParameter = tab;

        Tabs.Add(tab);

        return tab;
    }

    /// <summary>
    /// Changes tab header
    /// </summary>
    /// <param name="tab">Tab reference</param>
    /// <param name="header">New header</param>
    public void RenameTab(FileTab tab, string header)
    {
        tab.Content = header;
    }

    /// <summary>
    /// Deletes tab. If there are no tabs left, it creates an empty tab.
    /// </summary>
    /// <remarks>
    /// After deletion, the previous tab will be selected
    /// </remarks>
    /// <param name="tab">Tab reference</param>
    public void DeleteTab(FileTab tab)
    {
        var index = Tabs.IndexOf(tab) - 1;

        Tabs.Remove(tab);

        var tabToSelect = Tabs.ElementAtOrDefault(index == -1 ? 0 : index) ?? CreateTab();
        SelectTab(tabToSelect);
    }

    /// <summary>
    /// Changes current tab
    /// </summary>
    /// <param name="tab">Tab reference</param>
    public void SelectTab(FileTab tab)
    {
        if (Tab != null)
        {
            Tab.Background = DefaultBackground;
        }

        Tab = tab;
        Tab.Background = SelectedBackground;
    }

    /// <summary>
    /// Changes tab foreground
    /// </summary>
    /// <param name="tab">Tab reference</param>
    /// <param name="brush">Brush</param>
    public void ChangeForeground(FileTab tab, IBrush brush)
    {
        tab.Foreground = brush;
    }

    private int GenerateNewTabNumber()
    {
        var patter = new Regex("^new ([0-9]+)$");
        var num = Tabs
            .Select(t => patter.Match((t.Content as string)!))
            .Where(m => m.Groups.Count == 2)
            .Select(m => m.Groups.Values.ElementAt(1))
            .DefaultIfEmpty()
            .Max(g => int.Parse(g?.Value ?? "0"));
        return ++num;
    }
}
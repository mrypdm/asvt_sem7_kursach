using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GUI.Exceptions;
using GUI.Models;
using GUI.Notifiers;
using GUI.ViewModels;
using GUI.ViewModels.Abstraction;
using GUI.Views;

namespace GUI.Managers;

/// <inheritdoc cref="ITabManager" />
public class TabManager : PropertyChangedNotifier, ITabManager
{
    private FileTabViewModel _tab;

    /// <inheritdoc />
    public IFileTabViewModel Tab
    {
        get => _tab;
        set => SetField(ref _tab, value as FileTabViewModel);
    }

    /// <inheritdoc />
    public ObservableCollection<IFileTabViewModel> Tabs { get; } = new();

    /// <inheritdoc />
    public IFileTabViewModel CreateTab(FileModel file, Func<IFileTabViewModel, Task> selectCommand,
        Func<IFileTabViewModel, Task> closeCommand)
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

        var viewModel = new FileTabViewModel(new FileTab(), file ?? new FileModel(), selectCommand, closeCommand);
        Tabs.Add(viewModel);
        return viewModel;
    }

    /// <inheritdoc />
    public void DeleteTab(IFileTabViewModel tab)
    {
        var index = Tabs.IndexOf(tab) - 1;

        Tabs.Remove(tab);

        var tabToSelect = Tabs.ElementAtOrDefault(index == -1 ? 0 : index);
        SelectTab(tabToSelect);
    }

    /// <inheritdoc />
    public void SelectTab(IFileTabViewModel tab)
    {
        if (_tab != null)
        {
            _tab.IsSelected = false;
        }

        Tab = tab;

        if (_tab != null)
        {
            _tab.IsSelected = true;
        }
    }

    /// <inheritdoc />
    public void UpdateForeground(IFileTabViewModel tab)
    {
        (tab as FileTabViewModel)?.NotifyForegroundChanged();
    }

    /// <inheritdoc />
    public void UpdateHeader(IFileTabViewModel tab)
    {
        (tab as FileTabViewModel)?.NotifyHeaderChanged();
    }
}
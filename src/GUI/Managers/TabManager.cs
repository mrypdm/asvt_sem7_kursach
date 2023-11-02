﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GUI.Exceptions;
using GUI.Models;
using GUI.Notifiers;
using GUI.ViewModels;
using GUI.Views;

namespace GUI.Managers;

/// <inheritdoc cref="ITabManager" />
public class TabManager : PropertyChangedNotifier, ITabManager
{
    private readonly Func<FileTabViewModel, Task> _selectCommand;
    private readonly Func<FileTabViewModel, Task> _closeCommand;

    private FileTabViewModel _tab;

    /// <inheritdoc />
    public TabManager(Func<FileTabViewModel, Task> selectCommand, Func<FileTabViewModel, Task> closeCommand)
    {
        _selectCommand = selectCommand;
        _closeCommand = closeCommand;
    }

    /// <inheritdoc />
    public FileTabViewModel Tab
    {
        get => _tab;
        set => SetField(ref _tab, value);
    }

    /// <inheritdoc />
    public ObservableCollection<FileTabViewModel> Tabs { get; } = new();

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void DeleteTab(FileTabViewModel tab)
    {
        var index = Tabs.IndexOf(tab) - 1;

        Tabs.Remove(tab);

        var tabToSelect = Tabs.ElementAtOrDefault(index == -1 ? 0 : index);
        SelectTab(tabToSelect);
    }

    /// <inheritdoc />
    public void SelectTab(FileTabViewModel tab)
    {
        Tab?.SetTabUnselected();
        Tab = tab;
        Tab?.SetTabSelected();
    }
}
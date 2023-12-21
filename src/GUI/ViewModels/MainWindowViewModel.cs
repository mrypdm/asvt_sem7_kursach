using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Assembler;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Devices.Providers;
using Devices.Validators;
using GUI.Exceptions;
using GUI.Managers;
using GUI.Models;
using GUI.Views;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using Shared.Helpers;
using Domain.Models;
using GUI.Extensions;
using GUI.MessageBoxes;
using GUI.Providers;

namespace GUI.ViewModels;

/// <inheritdoc cref="IMainWindowViewModel"/>
public class MainWindowViewModel : WindowViewModel<MainWindow>, IMainWindowViewModel
{
    private const string DefaultWindowTitle = "PDP-11 Simulator";
    private const string MainFileName = "main.asm";

    private readonly IFileManager _fileManager;
    private readonly IMessageBoxManager _messageBoxManager;
    private readonly IWindowProvider _windowProvider;
    private readonly ITabManager _tabManager;
    private readonly IProjectManager _projectManager;

    /// <summary>
    /// Empty constructor for designer
    /// </summary>
    public MainWindowViewModel() : base(null)
    {
    }

    /// <summary>
    /// Creates new instance of main window view model
    /// </summary>
    /// <param name="window">Reference to <see cref="MainWindow"/></param>
    /// <param name="projectManager">Project manager</param>
    /// <param name="fileManager">File manager</param>
    /// <param name="tabManager">Tab manager</param>
    /// <param name="messageBoxManager">Message box manager</param>
    /// <param name="windowProvider">Window provider</param>
    public MainWindowViewModel(MainWindow window, ITabManager tabManager, IProjectManager projectManager,
        IFileManager fileManager, IMessageBoxManager messageBoxManager,
        IWindowProvider windowProvider) : base(window)
    {
        CreateFileCommand = ReactiveCommand.CreateFromTask(CreateFileAsync);
        OpenFileCommand = ReactiveCommand.CreateFromTask(OpenFileAsync);
        SaveFileCommand = ReactiveCommand.CreateFromTask<bool>(
            async saveAs => await SaveFileAndUpdateTab(_tabManager!.Tab, saveAs));
        SaveAllFilesCommand = ReactiveCommand.CreateFromTask(SaveAllFilesAsync);
        DeleteFileCommand = ReactiveCommand.CreateFromTask(DeleteFileAsync);
        CreateProjectCommand = ReactiveCommand.CreateFromTask(async () => { await CreateProjectAsync(); });
        OpenProjectCommand = ReactiveCommand.CreateFromTask(async () => { await OpenProjectAsync(); });
        OpenSettingsWindowCommand = ReactiveCommand.CreateFromTask(OpenSettingsWindowAsync);
        OpenExecutorWindowCommand = ReactiveCommand.CreateFromTask(OpenExecutorWindowAsync);
        BuildProjectCommand = ReactiveCommand.CreateFromTask(BuildProjectAsync);

        _fileManager = fileManager;
        _messageBoxManager = messageBoxManager;
        _windowProvider = windowProvider;

        _projectManager = projectManager;
        _projectManager.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(_projectManager.Project))
            {
                this.RaisePropertyChanged(nameof(WindowTitle));
                OnProjectUpdated();
            }
        };

        _tabManager = tabManager;
        _tabManager.Tabs.CollectionChanged += (_, _) => { this.RaisePropertyChanged(nameof(Tabs)); };
        _tabManager.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(_tabManager.Tab))
            {
                this.RaisePropertyChanged(nameof(FileContent));
            }
        };

        window.Closing += OnClosingWindow;
        window.Opened += async (_, _) =>
        {
            if (!await InitProjectAsync())
            {
                View.Close();
            }
        };

        SettingsManager.Instance.PropertyChanged += (_, args) => this.RaisePropertyChanged(args.PropertyName);

        InitContext();
    }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> CreateFileCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<bool, Unit> SaveFileCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> SaveAllFilesCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> DeleteFileCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> CreateProjectCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> OpenProjectCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> OpenExecutorWindowCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> BuildProjectCommand { get; }

    public string WindowTitle => _projectManager?.IsOpened == true
        ? $"{DefaultWindowTitle} - {_projectManager.Project.ProjectName}"
        : DefaultWindowTitle;

    /// <inheritdoc />
    public ObservableCollection<FileTab> Tabs => _tabManager.Tabs.Select(t => t.View).ToObservableCollection();

    /// <inheritdoc />
    public string FileContent
    {
        get => File.Text;
        set
        {
            File.Text = value;
            File.IsNeedSave = true;
            _tabManager.UpdateForeground(_tabManager.Tab);
            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    /// Reference to current file
    /// </summary>
    private FileModel File => _tabManager.Tab.File;

    #region Tabs and files

    private async Task CreateTabForFiles(IEnumerable<FileModel> files)
    {
        IFileTabViewModel tab = null;

        foreach (var file in files)
        {
            try
            {
                tab = _tabManager.CreateTab(file, t =>
                {
                    _tabManager.SelectTab(t);
                    return Task.CompletedTask;
                }, t => CloseTabAsync(t, true));
            }
            catch (TabExistsException e)
            {
                tab = e.Tab;

                var res = await _messageBoxManager.ShowCustomMessageBoxAsync("Warning",
                    $"File '{file.FileName}' is already open", Icon.Warning, View, Buttons.ReopenButton,
                    Buttons.SkipButton);

                if (res == Buttons.ReopenButton.Name)
                {
                    e.Tab.File.Text = file.Text;
                    if (ReferenceEquals(e.Tab, _tabManager.Tab))
                    {
                        this.RaisePropertyChanged(nameof(FileContent));
                    }
                }
            }
        }

        if (tab != null)
        {
            _tabManager.SelectTab(tab);
        }
    }

    /// <summary>
    /// Creates new file and tab for it
    /// </summary>
    private async Task CreateFileAsync()
    {
        var file = await _fileManager.CreateFile(View.StorageProvider,
            _projectManager.IsOpened ? _projectManager.Project.ProjectDirectory : null, null);

        if (file != null)
        {
            await CreateTabForFiles(new[] { file });
            _projectManager.AddFileToProject(file.FilePath);
            await _projectManager.SaveProjectAsync();
        }
    }

    /// <summary>
    /// Opens file and creates tab for it
    /// </summary>
    private async Task OpenFileAsync()
    {
        var files = await _fileManager.OpenFilesAsync(View.StorageProvider);
        await CreateTabForFiles(files);
    }

    /// <summary>
    /// Save file on new path
    /// </summary>
    /// <param name="file">File info</param>
    /// <returns>True if file was saved</returns>
    private async Task<bool> SaveFileAsAsync(FileModel file)
    {
        var paths = _tabManager.Tabs
            .Where(t => t.File.FilePath != file.FilePath)
            .Select(t => t.File.FilePath)
            .ToHashSet();

        var options = new FilePickerSaveOptions
        {
            Title = "Save file as...",
            ShowOverwritePrompt = true,
            SuggestedFileName = file.FileName
        };

        do
        {
            var filePath = await _fileManager.GetFileAsync(View.StorageProvider, options);

            if (filePath == null)
            {
                return false;
            }

            if (!paths.Contains(filePath))
            {
                file.FilePath = filePath;
                await _fileManager.WriteFileAsync(file);
                return true;
            }

            await _messageBoxManager.ShowErrorMessageBox("That file already opened", View);
        } while (true);
    }

    /// <summary>
    /// Saves project file
    /// </summary>
    /// <param name="file">File info</param>
    /// <returns>True if saved</returns>
    private async Task<bool> SaveProjectFile(FileModel file)
    {
        var error = await JsonHelper.ValidateJsonAsync<ProjectDto>(file.Text);

        if (error == null)
        {
            await _fileManager.WriteFileAsync(file);
            await _projectManager.ReloadProjectAsync();
            return true;
        }

        await _messageBoxManager.ShowErrorMessageBox(error, View);

        return false;
    }

    /// <summary>
    /// Is tab represents the project file
    /// </summary>
    private bool IsProjectTab(IFileTabViewModel tab) => IsProjectFile(tab.File);

    /// <summary>
    /// Is file the project file
    /// </summary>
    private bool IsProjectFile(FileModel file) =>
        _projectManager.IsOpened && file.FilePath == _projectManager.Project.ProjectFile;

    /// <summary>
    /// Saves file
    /// </summary>
    /// <param name="file">File info</param>
    /// <param name="saveAs">Save as new file</param>
    /// <returns>True if file was saved</returns>
    private async Task<bool> SaveFileAsync(FileModel file, bool saveAs)
    {
        if (IsProjectFile(file))
        {
            if (!saveAs)
            {
                return await SaveProjectFile(file);
            }

            await _messageBoxManager.ShowErrorMessageBox("This feature is not available for project file", View);
            return false;
        }

        if (saveAs)
        {
            return await SaveFileAsAsync(file);
        }

        await _fileManager.WriteFileAsync(file);
        return true;
    }

    /// <summary>
    /// Saves all files
    /// </summary>
    private async Task SaveAllFilesAsync()
    {
        foreach (var tab in _tabManager.Tabs)
        {
            await SaveFileAndUpdateTab(tab, false);
        }
    }

    private async Task SaveFileAndUpdateTab(IFileTabViewModel tab, bool saveAs)
    {
        if (await SaveFileAsync(tab.File, saveAs))
        {
            _tabManager.UpdateForeground(tab);
            _tabManager.UpdateHeader(tab);
        }
    }

    /// <summary>
    /// Deletes current file
    /// </summary>
    private async Task DeleteFileAsync()
    {
        if (IsProjectTab(_tabManager.Tab))
        {
            await _messageBoxManager.ShowErrorMessageBox("Cannot delete project file", View);
            return;
        }

        var res = await _messageBoxManager.ShowMessageBoxAsync("Confirmation",
            $"Are you sure you want to delete the file '{File.FileName}'?", ButtonEnum.YesNo, Icon.Question, View);

        if (res == ButtonResult.Yes)
        {
            _projectManager.RemoveFileFromProject(File.FilePath);
            await _projectManager.SaveProjectAsync();
            await _fileManager.DeleteAsync(File);
            _tabManager.DeleteTab(_tabManager.Tab);
        }
    }

    /// <summary>
    /// Closes tab
    /// </summary>
    private async Task CloseTabAsync(IFileTabViewModel tab, bool isUi)
    {
        if (IsProjectTab(tab) && isUi)
        {
            await _messageBoxManager.ShowErrorMessageBox("Cannot close project file", View);
            return;
        }

        if (tab.File.IsNeedSave)
        {
            var res = await _messageBoxManager.ShowMessageBoxAsync("Confirmation",
                $"Do you want to save the file '{File.FileName}'?", ButtonEnum.YesNo, Icon.Question, View);

            if (res == ButtonResult.Yes)
            {
                await SaveFileAsync(tab.File, false);
            }
        }

        _tabManager.DeleteTab(tab);
    }

    /// <summary>
    /// Closes all tabs
    /// </summary>
    private async Task CloseAllTabs()
    {
        var tabs = _tabManager.Tabs.ToList();

        foreach (var tab in tabs)
        {
            await CloseTabAsync(tab, false);
        }
    }

    #endregion

    #region Project

    /// <summary>
    /// Create or open project at startup
    /// </summary>
    /// <returns>True if created or opened</returns>
    private async Task<bool> InitProjectAsync()
    {
        if (SettingsManager.Instance.CommandLineOptions?.Project != null &&
            await OpenProjectAsync(SettingsManager.Instance.CommandLineOptions.Project))
        {
            return true;
        }

        while (true)
        {
            var boxRes = await _messageBoxManager.ShowCustomMessageBoxAsync("Init", "Create or open project", Icon.Info,
                View, Buttons.CreateButton, Buttons.OpenButton, Buttons.CancelButton
            );

            if (boxRes == Buttons.CreateButton.Name && await CreateProjectAsync()
                || boxRes == Buttons.OpenButton.Name && await OpenProjectAsync())
            {
                return true;
            }

            if (boxRes == Buttons.CancelButton.Name || boxRes == null)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Asks for tabs closing
    /// </summary>
    /// <returns>True if we user agreed</returns>
    private async Task<bool> NewProjectValidation()
    {
        if (!Tabs.Any())
        {
            return true;
        }

        var res = await _messageBoxManager
            .ShowMessageBoxAsync("Warning", "This action closes current project and all tabs",
                ButtonEnum.OkAbort, Icon.Warning, View);

        return res == ButtonResult.Ok;
    }

    /// <summary>
    /// Opens new project files and closes old project files
    /// </summary>
    private async Task OpenProjectFilesAsync()
    {
        await CloseAllTabs();

        var projectFile = await _fileManager.OpenFileAsync(_projectManager.Project.ProjectFile);

        var files = new List<FileModel> { projectFile };

        foreach (var filePath in _projectManager.Project.Files)
        {
            try
            {
                var file = await _fileManager.OpenFileAsync(filePath);
                files.Add(file);
            }
            catch (FileNotFoundException e)
            {
                await _messageBoxManager.ShowErrorMessageBox($"{e.Message} Skipping it.", View);
            }
        }

        await CreateTabForFiles(files);
    }

    private async Task<bool> CreateProjectAsync()
    {
        if (!await NewProjectValidation())
        {
            return false;
        }

        bool successCreation;
        while (true)
        {
            var (res, projectName) = await _messageBoxManager.ShowInputMessageBoxAsync("Create project",
                "Enter project name", ButtonEnum.OkCancel, Icon.Setting, View, "Project name");

            if (res == ButtonResult.Cancel)
            {
                return false;
            }

            try
            {
                successCreation = await _projectManager.CreateProjectAsync(View.StorageProvider, projectName.Trim());
            }
            catch (ArgumentException e)
            {
                await _messageBoxManager.ShowErrorMessageBox(e.Message, View);
                continue;
            }

            break;
        }

        if (!successCreation)
        {
            return false;
        }

        var mainFile = new FileModel
        {
            FilePath = PathHelper.Combine(_projectManager.Project.ProjectDirectory, MainFileName)
        };
        await _fileManager.WriteFileAsync(mainFile);
        _projectManager.AddFileToProject(mainFile.FilePath);
        _projectManager.SetExecutableFile(mainFile.FilePath);
        await _projectManager.SaveProjectAsync();

        await OpenProjectFilesAsync();
        return true;
    }

    private async Task<bool> OpenProjectAsync(string projectPath = null)
    {
        if (!await NewProjectValidation())
        {
            return false;
        }

        try
        {
            if (projectPath != null)
            {
                try
                {
                    await _projectManager.LoadProjectAsync(projectPath);
                    await OpenProjectFilesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await _messageBoxManager.ShowErrorMessageBox(e.Message, View);
                }
            }

            if (await _projectManager.OpenProjectAsync(View.StorageProvider))
            {
                await OpenProjectFilesAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            await _messageBoxManager.ShowErrorMessageBox(e.Message, View);
            return false;
        }

        return false;
    }

    #endregion

    /// <summary>
    /// Opens <see cref="SettingsWindow"/>
    /// </summary>
    private async Task OpenSettingsWindowAsync()
    {
        var viewModel = _windowProvider.CreateWindow<SettingsWindow, SettingsViewModel>(_projectManager, _fileManager,
            new DeviceValidator(new DeviceProvider()), _messageBoxManager);
        await viewModel.ShowDialog(View);
    }

    /// <summary>
    /// Opens <see cref="ExecutorWindow"/>
    /// </summary>
    private async Task OpenExecutorWindowAsync()
    {
        var executor = new Executor.Executor();
        await executor.LoadProgram( _projectManager.Project);

        var viewModel = _windowProvider.CreateWindow<ExecutorWindow, ExecutorViewModel>(executor, _messageBoxManager);
        await viewModel.ShowDialog(View);
    }

    #region Handlers

    private async void OnClosingWindow(object sender, WindowClosingEventArgs args)
    {
        args.Cancel = true;

        if (_tabManager.Tabs.Any(t => t.File.IsNeedSave))
        {
            var res = await _messageBoxManager.ShowMessageBoxAsync("Warning",
                "You have unsaved files. Save all of them?", ButtonEnum.YesNoCancel, Icon.Warning, View);

            if (res == ButtonResult.Cancel)
            {
                return;
            }

            if (res == ButtonResult.Yes)
            {
                await SaveAllFilesAsync();
            }
        }

        View.Closing -= OnClosingWindow;
        View.Close();
    }

    private async void OnProjectUpdated()
    {
        if (!_projectManager.IsOpened)
        {
            return;
        }

        var projectTab = _tabManager.Tabs.SingleOrDefault(IsProjectTab);
        if (projectTab != null)
        {
            var fileOnDisk = await _fileManager.OpenFileAsync(projectTab.File.FilePath);
            projectTab.File.Text = fileOnDisk.Text;
            this.RaisePropertyChanged(nameof(FileContent));
        }
    }

    #endregion

    private async Task BuildProjectAsync()
    {
        await SaveAllFilesAsync();

        var assembler = new Compiler();

        try
        {
            await assembler.Compile(_projectManager.Project);
            await _messageBoxManager.ShowMessageBoxAsync("Build", "Completed", ButtonEnum.Ok, Icon.Info, View);
        }
        catch (Exception e)
        {
            await _messageBoxManager.ShowErrorMessageBox(e.Message, View);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using GUI.Exceptions;
using GUI.Managers;
using GUI.Models;
using GUI.Views;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using Shared.Helpers;
using MessageBoxManager = GUI.Managers.MessageBoxManager;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="MainWindow"/>
/// </summary>
public class MainWindowViewModel : ReactiveObject
{
    private const string DefaultWindowTitle = "PDP-11 Simulator";
    private const string MainFileName = "main.asm";

    private readonly Window _window;
    private readonly FileManager _fileManager;
    private readonly TabManager _tabManager;
    private SettingsWindow _settingsWindow;

    /// <summary>
    /// Empty constructor for designer
    /// </summary>
    public MainWindowViewModel()
    {
    }

    /// <summary>
    /// Creates new instance of main window view model
    /// </summary>
    /// <param name="window">Reference to <see cref="MainWindow"/></param>
    public MainWindowViewModel(Window window)
    {
        _window = window;
        CreateFileCommand = ReactiveCommand.CreateFromTask(CreateFileAsync);
        OpenFileCommand = ReactiveCommand.CreateFromTask(OpenFileAsync);
        SaveFileCommand = ReactiveCommand.CreateFromTask<bool>(
            async saveAs => await SaveFileAndUpdateTab(_tabManager!.Tab, saveAs));
        SaveAllFilesCommand = ReactiveCommand.CreateFromTask(SaveAllFilesAsync);
        DeleteFileCommand = ReactiveCommand.CreateFromTask(DeleteFileAsync);
        CloseFileCommand = ReactiveCommand.CreateFromTask(async () => await CloseTabAsync(_tabManager!.Tab, true));
        CreateProjectCommand = ReactiveCommand.CreateFromTask(async () => { await CreateProjectAsync(); });
        OpenProjectCommand = ReactiveCommand.CreateFromTask(async () => { await OpenProjectAsync(); });
        OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);

        _fileManager = new FileManager(window.StorageProvider);
        _tabManager = new TabManager(tab => 
        {
            _tabManager!.SelectTab(tab);
            return Task.CompletedTask;
        }, async tab => await CloseTabAsync(tab, true));

        window.Closing += OnClosingWindow;

        window.Opened += async (_, _) =>
        {
            if (!await InitProject())
            {
                _window.Close();
            }
        };

        ProjectManager.Instance.PropertyChanged += (_, _) =>
        {
            this.RaisePropertyChanged(nameof(WindowTitle));
            OnProjectUpdated();
        };

        SettingsManager.Instance.PropertyChanged += (_, args) => this.RaisePropertyChanged(args.PropertyName);

        _tabManager.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(_tabManager.Tab))
            {
                this.RaisePropertyChanged(nameof(FileContent));
            }
        };
    }

    /// <summary>
    /// Command for creating file
    /// </summary>
    public ReactiveCommand<Unit, Unit> CreateFileCommand { get; }

    /// <summary>
    /// Command for opening file
    /// </summary>
    public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }

    /// <summary>
    /// Command for saving file
    /// </summary>
    public ReactiveCommand<bool, Unit> SaveFileCommand { get; }

    /// <summary>
    /// Command for saving all files
    /// </summary>
    public ReactiveCommand<Unit, Unit> SaveAllFilesCommand { get; }

    /// <summary>
    /// Command for deleting file
    /// </summary>
    public ReactiveCommand<Unit, Unit> DeleteFileCommand { get; }

    /// <summary>
    /// Command for closing file
    /// </summary>
    public ReactiveCommand<Unit, Unit> CloseFileCommand { get; }

    /// <summary>
    /// Command for creating project
    /// </summary>
    public ReactiveCommand<Unit, Unit> CreateProjectCommand { get; }

    /// <summary>
    /// Command for opening project
    /// </summary>
    public ReactiveCommand<Unit, Unit> OpenProjectCommand { get; }

    /// <summary>
    /// Command for opening <see cref="SettingsWindow"/>
    /// </summary>
    public ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }

    public string WindowTitle => ProjectManager.Instance.IsOpened
        ? $"{DefaultWindowTitle} - {ProjectManager.Instance.Project.Name}"
        : DefaultWindowTitle;

    /// <summary>
    /// Collection of tabs
    /// </summary>
    public ObservableCollection<FileTab> Tabs => _tabManager.Tabs;

    /// <summary>
    /// Current text of <see cref="MainWindow.SourceCodeTextBox"/>
    /// </summary>
    public string FileContent
    {
        get => File.Text;
        set
        {
            File.Text = value;
            File.IsNeedSave = true;
            _tabManager.ChangeForeground(_tabManager.Tab, TabManager.NeedSaveForeground);
            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    /// Reference to current file
    /// </summary>
    private FileModel File => _tabManager.Tab.File;

    /// <inheritdoc cref="SettingsManager.FontFamily"/>
    public FontFamily FontFamily => SettingsManager.Instance.FontFamily;

    /// <inheritdoc cref="SettingsManager.FontSize"/>
    public double FontSize => SettingsManager.Instance.FontSize;

    #region Tabs and files

    private async Task CreateTabForFiles(IEnumerable<FileModel> files)
    {
        FileTab tab = null;

        foreach (var file in files)
        {
            try
            {
                tab = _tabManager.CreateTab(file);
            }
            catch (TabExistsException e)
            {
                tab = e.Tab;

                var res = await MessageBoxManager
                    .ShowCustomMessageBoxAsync(
                        "Warning", $"File '{file.FileName}' is already open", Icon.Warning, _window,
                        MessageBoxManager.ReopenButton, MessageBoxManager.SkipButton);

                if (res == MessageBoxManager.ReopenButton.Name)
                {
                    e.Tab.File.Text = file.Text;
                    if (ReferenceEquals(e.Tab, _tabManager.Tab))
                    {
                        this.RaisePropertyChanged(nameof(FileContent));
                    }
                }
            }
        }

        _tabManager.SelectTab(tab);
    }

    /// <summary>
    /// Creates new file and tab for it
    /// </summary>
    private async Task CreateFileAsync()
    {
        var file = await _fileManager.CreateFile(
            ProjectManager.Instance.IsOpened ? ProjectManager.Instance.Project.Directory : null, null);

        if (file != null)
        {
            await CreateTabForFiles(new[] { file });
            ProjectManager.Instance.AddFileToProject(file.FilePath);
            await ProjectManager.Instance.SaveProjectAsync();
        }
    }

    /// <summary>
    /// Opens file and creates tab for it
    /// </summary>
    private async Task OpenFileAsync()
    {
        var files = await _fileManager.OpenFilesAsync();

        if (files != null)
        {
            await CreateTabForFiles(files);
        }
    }

    /// <summary>
    /// Save file on new path
    /// </summary>
    /// <param name="file">File info</param>
    /// <returns>True if file was saved</returns>
    private async Task<bool> SaveFileAsAsync(FileModel file)
    {
        var paths = Tabs
            .Where(t => t.File.FilePath != file.FilePath)
            .Select(t => t.File.FilePath)
            .ToHashSet();

        do
        {
            var filePath = await _fileManager.SelectFileAsync(null, file.FileName);

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

            await MessageBoxManager.ShowErrorMessageBox("That file already opened", _window);
        } while (true);
    }

    /// <summary>
    /// Saves project file
    /// </summary>
    /// <param name="file">File info</param>
    /// <returns>True if saved</returns>
    private async Task<bool> SaveProjectFile(FileModel file)
    {
        var error = await JsonHelper.ValidateJson<ProjectModel>(file.Text);

        if (error == null)
        {
            await _fileManager.WriteFileAsync(file);
            await ProjectManager.Instance.ReloadProjectAsync();
            return true;
        }

        await MessageBoxManager.ShowErrorMessageBox(error, _window);

        return false;
    }

    /// <summary>
    /// Saves file
    /// </summary>
    /// <param name="file">File info</param>
    /// <param name="saveAs">Save as new file</param>
    /// <returns>True if file was saved</returns>
    private async Task<bool> SaveFileAsync(FileModel file, bool saveAs)
    {
        if (file.FilePath ==
            (ProjectManager.Instance.IsOpened ? ProjectManager.Instance.Project.ProjectFilePath : null))
        {
            return await SaveProjectFile(file);
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
        foreach (var tab in Tabs)
        {
            await SaveFileAndUpdateTab(tab, false);
        }
    }

    private async Task SaveFileAndUpdateTab(FileTab tab, bool saveAs)
    {
        if (await SaveFileAsync(tab.File, saveAs))
        {
            _tabManager.RenameTab(tab, tab.File.FileName);
            _tabManager.ChangeForeground(tab, TabManager.DefaultForeground);
        }
    }

    /// <summary>
    /// Deletes current file
    /// </summary>
    private async Task DeleteFileAsync()
    {
        var res = await MessageBoxManager
            .ShowMessageBoxAsync("Confirmation", $"Are you sure you want to delete the file '{File.FileName}'?",
                ButtonEnum.YesNo, Icon.Question, _window);

        if (res == ButtonResult.Yes)
        {
            ProjectManager.Instance.RemoveFileFromProject(File.FilePath);
            await ProjectManager.Instance.SaveProjectAsync();
            await _fileManager.DeleteAsync(File);
            _tabManager.DeleteTab(_tabManager.Tab);
        }
    }

    /// <summary>
    /// Closes tab
    /// </summary>
    private async Task CloseTabAsync(FileTab tab, bool isUi)
    {
        if (tab.File.FilePath == ProjectManager.Instance.Project.ProjectFilePath && isUi)
        {
            await MessageBoxManager.ShowErrorMessageBox("Cannot close project file", _window);
            return;
        }

        if (tab.File.IsNeedSave)
        {
            var res = await MessageBoxManager
                .ShowMessageBoxAsync("Confirmation", $"Do you want to save the file '{File.FileName}'?",
                    ButtonEnum.YesNo, Icon.Question, _window);

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
        var tabs = Tabs.ToList();

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
    private async Task<bool> InitProject()
    {
        if (SettingsManager.Instance.CommandLineOptions.Project != null &&
            await OpenProjectAsync(SettingsManager.Instance.CommandLineOptions.Project))
        {
            return true;
        }

        while (true)
        {
            var boxRes = await MessageBoxManager.ShowCustomMessageBoxAsync("Init", "Create or open project", Icon.Info,
                _window, MessageBoxManager.CreateButton, MessageBoxManager.OpenButton, MessageBoxManager.CancelButton
            );

            if (boxRes == MessageBoxManager.CreateButton.Name && await CreateProjectAsync()
                || boxRes == MessageBoxManager.OpenButton.Name && await OpenProjectAsync())
            {
                return true;
            }

            if (boxRes == MessageBoxManager.CancelButton.Name || boxRes == null)
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

        var res = await MessageBoxManager
            .ShowMessageBoxAsync("Warning", "This action closes current project and all tabs",
                ButtonEnum.OkAbort, Icon.Warning, _window);

        return res == ButtonResult.Ok;
    }

    /// <summary>
    /// Opens new project files and closes old project files
    /// </summary>
    private async Task OpenProjectFilesAsync()
    {
        await CloseAllTabs();

        var projectFile = await _fileManager.OpenFileAsync(ProjectManager.Instance.Project.ProjectFilePath);

        var files = new List<FileModel> { projectFile };

        foreach (var filePath in ProjectManager.Instance.Project.ProjectFilesPaths)
        {
            try
            {
                var file = await _fileManager.OpenFileAsync(filePath);
                files.Add(file);
            }
            catch (FileNotFoundException e)
            {
                await MessageBoxManager.ShowErrorMessageBox($"{e.Message} Skipping it.", _window);
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

        string projectName;
        while (true)
        {
            (var res, projectName) = await MessageBoxManager.ShowInputMessageBoxAsync("Create project",
                "Enter project name", ButtonEnum.OkCancel, Icon.Setting, _window, "Project name");

            if (res == ButtonResult.Cancel)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                await MessageBoxManager.ShowErrorMessageBox("Project name cannot be empty", _window);
                continue;
            }

            break;
        }

        if (await ProjectManager.Instance.CreateProjectAsync(_window.StorageProvider, projectName.Trim()))
        {
            var mainFile = new FileModel
            {
                FilePath = PathHelper.Combine(ProjectManager.Instance.Project.Directory, MainFileName)
            };
            await _fileManager.WriteFileAsync(mainFile);
            ProjectManager.Instance.AddFileToProject(mainFile.FilePath);
            ProjectManager.Instance.SetExecutableFile(mainFile.FilePath);
            await ProjectManager.Instance.SaveProjectAsync();

            await OpenProjectFilesAsync();
            return true;
        }

        return false;
    }

    private async Task<bool> OpenProjectAsync(string projectPath = null)
    {
        if (!await NewProjectValidation())
        {
            return false;
        }

        try
        {
            if (projectPath == null && await ProjectManager.Instance.OpenProjectAsync(_window.StorageProvider))
            {
                await OpenProjectFilesAsync();
                return true;
            }

            if (projectPath != null)
            {
                await ProjectManager.Instance.LoadProjectAsync(projectPath);
                await OpenProjectFilesAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            await MessageBoxManager.ShowErrorMessageBox(e.Message, _window);
            return false;
        }

        return false;
    }

    #endregion

    /// <summary>
    /// Opens <see cref="SettingsWindow"/>
    /// </summary>
    private void OpenSettingsWindow()
    {
        if (_settingsWindow is not { IsLoaded: true })
        {
            _settingsWindow = new SettingsWindow();
            _settingsWindow.Show();
        }
        else
        {
            _settingsWindow.Activate();
        }
    }

    #region Handlers

    private async void OnClosingWindow(object sender, WindowClosingEventArgs args)
    {
        args.Cancel = true;

        if (Tabs.Any(t => t.File.IsNeedSave))
        {
            var res = await MessageBoxManager
                .ShowMessageBoxAsync("Warning", "You have unsaved files. Save all of them?",
                    ButtonEnum.YesNoCancel, Icon.Warning, _window);

            if (res == ButtonResult.Cancel)
            {
                return;
            }

            if (res == ButtonResult.Yes)
            {
                await SaveAllFilesAsync();
            }
        }

        _window.Closing -= OnClosingWindow;
        _window.Close();
    }

    private async void OnProjectUpdated()
    {
        if (!ProjectManager.Instance.IsOpened)
        {
            return;
        }

        var projectTab = Tabs.SingleOrDefault(t => t.File.FilePath == ProjectManager.Instance.Project.ProjectFilePath);
        if (projectTab != null)
        {
            var fileOnDisk = await _fileManager.OpenFileAsync(projectTab.File.FilePath);
            projectTab.File.Text = fileOnDisk.Text;
            this.RaisePropertyChanged(nameof(Tabs));
            this.RaisePropertyChanged(nameof(FileContent));
        }
    }

    #endregion
}
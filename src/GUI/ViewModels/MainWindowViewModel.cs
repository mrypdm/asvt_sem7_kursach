using System.Collections.ObjectModel;
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
using MessageBoxManager = GUI.Managers.MessageBoxManager;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="MainWindow"/>
/// </summary>
public class MainWindowViewModel : ReactiveObject
{
    private const string DefaultWindowTitle = "PDP-11 Simulator";
    
    private readonly Window _window;
    private readonly FileManager _fileManager;
    private readonly TabManager _tabManager;
    private readonly ProjectManager _projectManager;

    private string _windowTitle = DefaultWindowTitle;

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
        CreateFileCommand = ReactiveCommand.Create(CreateFileAsync);
        OpenFileCommand = ReactiveCommand.CreateFromTask(OpenFileAsync);
        SaveFileCommand = ReactiveCommand.CreateFromTask<bool>(
            async saveAs => await SaveFileAndUpdateTab(_tabManager!.Tab, saveAs));
        SaveAllFilesCommand = ReactiveCommand.CreateFromTask(SaveAllFilesAsync);
        DeleteFileCommand = ReactiveCommand.CreateFromTask(DeleteFileAsync);
        CloseFileCommand = ReactiveCommand.CreateFromTask(async () => await CloseTabAsync(_tabManager!.Tab));
        SelectTabCommand = ReactiveCommand.Create<FileTab>(tab => _tabManager!.SelectTab(tab));
        CreateProjectCommand = ReactiveCommand.CreateFromTask(CreateProjectAsync);
        OpenProjectCommand = ReactiveCommand.CreateFromTask(OpenProjectAsync);

        OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);

        _fileManager = new FileManager(window.StorageProvider);
        _tabManager = new TabManager(SelectTabCommand);
        _projectManager = new ProjectManager(window.StorageProvider);

        window.Closing += OnClosingWindow;

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

    /// <summary>
    /// Command for selecting tab. Sets to tab at runtime
    /// </summary>
    private ReactiveCommand<FileTab, Unit> SelectTabCommand { get; }

    public string WindowTitle
    {
        get => _windowTitle;
        set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
    }
    
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

    /// <summary>
    /// Creates new file and tab for it
    /// </summary>
    private void CreateFileAsync()
    {
        var tab = _tabManager.CreateTab();
        _tabManager.SelectTab(tab);
    }

    /// <summary>
    /// Opens file and creates tab for it
    /// </summary>
    private async Task OpenFileAsync()
    {
        try
        {
            var files = await _fileManager.OpenFileAsync();
            if (files == null)
            {
                return;
            }

            FileTab tab = null;

            foreach (var file in files)
            {
                tab = _tabManager.CreateTab(file);
            }

            if (_tabManager.Tab.File.FilePath == null && string.IsNullOrWhiteSpace(_tabManager.Tab.File.Text))
            {
                _tabManager.DeleteTab(_tabManager.Tab);
            }

            _tabManager.SelectTab(tab);
        }
        catch (TabExistsException e)
        {
            await MessageBoxManager
                .GetMessageBox("Error", "That file already opened", ButtonEnum.Ok, Icon.Error, _window)
                .ShowWindowDialogAsync(_window);
            _tabManager.SelectTab(e.Tab);
        }
    }

    /// <summary>
    /// Saves file
    /// </summary>
    /// <param name="file">File tab reference</param>
    /// <param name="saveAs">Save as new file</param>
    private async Task<bool> SaveFileAsync(FileModel file, bool saveAs)
    {
        if (!saveAs && file.FilePath != null)
        {
            await _fileManager.WriteFileAsync(file);
            return true;
        }

        var paths = Tabs
            .Where(t => t.File.FilePath != null && t.File != file)
            .Select(t => t.File.FilePath)
            .ToHashSet();

        do
        {
            var filePath = await _fileManager.CreateFile(file.FileName);

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

            await MessageBoxManager
                .GetMessageBox("Error", "That file already opened", ButtonEnum.Ok, Icon.Error, _window)
                .ShowWindowDialogAsync(_window);
        } while (true);
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
            .GetMessageBox("Confirmation", $"Are you sure you want to delete the file '{File.FileName}'?",
                ButtonEnum.YesNo, Icon.Question, _window)
            .ShowWindowDialogAsync(_window);

        if (res == ButtonResult.Yes)
        {
            _fileManager.Delete(File);
            _tabManager.DeleteTab(_tabManager.Tab);
        }
    }

    /// <summary>
    /// Closes current file
    /// </summary>
    private async Task CloseTabAsync(FileTab tab)
    {
        if (tab.File.IsNeedSave)
        {
            var res = await MessageBoxManager
                .GetMessageBox("Confirmation", $"Do you want to save the file '{File.FileName}'?",
                    ButtonEnum.YesNo, Icon.Question, _window)
                .ShowWindowDialogAsync(_window);

            if (res == ButtonResult.Yes)
            {
                await SaveFileAsync(tab.File, false);
            }
        }

        _tabManager.DeleteTab(tab);
    }

    private async Task CreateProjectAsync()
    {
        var box = MessageBoxManager.GetInputMessageBox("Create project", "Enter project name", ButtonEnum.OkCancel,
            Icon.Setting, _window, "Project name");

        var res = await box.ShowWindowDialogAsync(_window);
        if (res == ButtonResult.Cancel)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(box.InputValue))
        {
            await MessageBoxManager
                .GetMessageBox("Error", "Project name cannot be empty", ButtonEnum.Ok, Icon.Error, _window)
                .ShowWindowDialogAsync(_window);
            return;
        }

        await _projectManager.CreateProjectAsync(box.InputValue.Trim());
        WindowTitle = $"{DefaultWindowTitle} - {_projectManager.Project.Name}";
    }

    private async Task OpenProjectAsync()
    {
        await _projectManager.OpenProjectAsync();
    }

    /// <summary>
    /// Opens <see cref="SettingsWindow"/>
    /// </summary>
    private void OpenSettingsWindow()
    {
        var settingsWindow = new SettingsWindow();
        settingsWindow.Show();
    }

    private async void OnClosingWindow(object sender, WindowClosingEventArgs args)
    {
        args.Cancel = true;

        if (Tabs.Any(t => t.File.IsNeedSave))
        {
            var res = await MessageBoxManager
                .GetMessageBox("Warning", "You have unsaved files. Save all of them?",
                    ButtonEnum.YesNoCancel, Icon.Warning, _window)
                .ShowWindowDialogAsync(_window);

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
}
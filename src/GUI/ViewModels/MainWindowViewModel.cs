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
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="MainWindow"/>
/// </summary>
public class MainWindowViewModel : ReactiveObject
{
    private readonly FileManager _fileManager;
    private readonly TabManager _tabManager;
    private readonly SettingsManager _settingsManager;

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
    public MainWindowViewModel(TopLevel window)
    {
        CreateFileCommand = ReactiveCommand.CreateFromTask(CreateFileAsync);
        OpenFileCommand = ReactiveCommand.CreateFromTask(OpenFileAsync);
        SaveFileCommand = ReactiveCommand.CreateFromTask(SaveFileAsync);
        SaveFileAsCommand = ReactiveCommand.CreateFromTask(SaveFileAsAsync);
        SaveAllFilesCommand = ReactiveCommand.CreateFromTask(SaveAllFilesAsync);
        DeleteFileCommand = ReactiveCommand.CreateFromTask(DeleteFileAsync);
        CloseFileCommand = ReactiveCommand.CreateFromTask(CloseFileAsync);
        SelectTabCommand = ReactiveCommand.CreateFromTask<FileTab>(SelectTabAsync);

        OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);

        _fileManager = new FileManager(window.StorageProvider);
        _tabManager = new TabManager(SelectTabCommand);

        SettingsManager.Instance.PropertyChanged += (_, args) => this.RaisePropertyChanged(args.PropertyName);
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
    public ReactiveCommand<Unit, Unit> SaveFileCommand { get; }

    /// <summary>
    /// Command for saving file on new path
    /// </summary>
    public ReactiveCommand<Unit, Unit> SaveFileAsCommand { get; }

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
    /// Command for opening <see cref="SettingsWindow"/>
    /// </summary>
    public ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }

    /// <summary>
    /// Command for selecting tab. Sets to tab at runtime
    /// </summary>
    private ReactiveCommand<FileTab, Unit> SelectTabCommand { get; }

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
    private async Task CreateFileAsync()
    {
        try
        {
            var file = await _fileManager.CreateFileAsync();
            if (file == null)
            {
                return;
            }

            var tab = _tabManager.CreateTab(file);
            await SelectTabAsync(tab);
        }
        catch (TabExistsException e)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Warning", e.Message, ButtonEnum.Ok, Icon.Warning)
                .ShowAsync();
            await SelectTabAsync(e.Tab);
        }
    }

    /// <summary>
    /// Opens file and creates tab for it
    /// </summary>
    private async Task OpenFileAsync()
    {
        try
        {
            var file = await _fileManager.OpenFileAsync();
            if (file == null)
            {
                return;
            }

            var tab = _tabManager.CreateTab(file);
            await SelectTabAsync(tab);
        }
        catch (TabExistsException e)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Warning", e.Message, ButtonEnum.Ok, Icon.Warning)
                .ShowAsync();
            await SelectTabAsync(e.Tab);
        }
    }

    /// <summary>
    /// Saves current file
    /// </summary>
    private async Task SaveFileAsync()
    {
        await _fileManager.SaveFileAsync(File);
    }

    /// <summary>
    /// Saves current file on new path
    /// </summary>
    private async Task SaveFileAsAsync()
    {
        await _fileManager.SaveFileAsAsync(File);
    }

    /// <summary>
    /// Saves all files
    /// </summary>
    private async Task SaveAllFilesAsync()
    {
        var tasks = Tabs.Select(t => t.File).Select(f => _fileManager.SaveFileAsync(f));
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Deletes current file
    /// </summary>
    private async Task DeleteFileAsync()
    {
        var res = await MessageBoxManager
            .GetMessageBoxStandard("Confirmation",
                $"Are you sure you want to delete the file '{File.FileName}'?",
                ButtonEnum.YesNo, Icon.Question)
            .ShowAsync();

        if (res == ButtonResult.Yes)
        {
            _fileManager.Delete(File);
            _tabManager.DeleteTab(_tabManager.Tab);
        }
    }

    /// <summary>
    /// Closes current file
    /// </summary>
    private async Task CloseFileAsync()
    {
        var res = await MessageBoxManager
            .GetMessageBoxStandard("Confirmation", $"Do you want to save the file '{File.FileName}'?",
                ButtonEnum.YesNo, Icon.Question)
            .ShowAsync();

        if (res == ButtonResult.Yes)
        {
            await SaveFileAsync();
        }

        _tabManager.DeleteTab(_tabManager.Tab);
    }

    /// <summary>
    /// Synchronizes a file in memory with a file on disk
    /// </summary>
    /// <param name="file">Current file info</param>
    private async Task SyncFile(FileModel file)
    {
        if (string.IsNullOrWhiteSpace(file.FilePath))
        {
            return;
        }

        var fileOnDisk = await _fileManager.OpenFileAsync(file.FilePath);

        if (fileOnDisk.Text != file.Text)
        {
            var res = await MessageBoxManager
                .GetMessageBoxStandard("Confirmation",
                    $"The file '{file.FileName}' has been modified. Load changes from disk?",
                    ButtonEnum.YesNo, Icon.Question)
                .ShowAsync();

            if (res == ButtonResult.Yes)
            {
                file.Text = fileOnDisk.Text;
            }
        }
    }

    /// <summary>
    /// Change current tab
    /// </summary>
    /// <param name="tab">Tab to switch to</param>
    private async Task SelectTabAsync(FileTab tab)
    {
        await SyncFile(tab.File);

        _tabManager.SelectTab(tab);

        this.RaisePropertyChanged(nameof(FileContent));
    }

    /// <summary>
    /// Opens <see cref="SettingsWindow"/>
    /// </summary>
    private void OpenSettingsWindow()
    {
        var settingsWindow = new SettingsWindow();
        settingsWindow.Show();
    }
}
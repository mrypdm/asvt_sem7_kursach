using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using GUI.Controls;
using GUI.Exceptions;
using GUI.Managers;
using GUI.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace GUI.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly FileManager _fileManager;
    private readonly TabManager _tabManager;

    public MainWindowViewModel()
    {
        // only for designer
    }

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

        _fileManager = new FileManager(window.StorageProvider);
        _tabManager = new TabManager(SelectTabCommand);
    }

    #region Commands

    public ReactiveCommand<Unit, Unit> CreateFileCommand { get; }

    public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }

    public ReactiveCommand<Unit, Unit> SaveFileCommand { get; }

    public ReactiveCommand<Unit, Unit> SaveFileAsCommand { get; }

    public ReactiveCommand<Unit, Unit> SaveAllFilesCommand { get; }

    public ReactiveCommand<Unit, Unit> DeleteFileCommand { get; }

    public ReactiveCommand<Unit, Unit> CloseFileCommand { get; }

    #endregion

    private ReactiveCommand<FileTab, Unit> SelectTabCommand { get; }

    public ObservableCollection<FileTab> Tabs => _tabManager.Tabs;

    public string CurrentFileText
    {
        get => CurrentFile.Text;
        set
        {
            CurrentFile.Text = value;
            this.RaisePropertyChanged();
        }
    }

    private FileModel CurrentFile => _tabManager.CurrentTab.File;

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

    private async Task SaveFileAsync()
    {
        await _fileManager.SaveFileAsync(CurrentFile);
    }

    private async Task SaveFileAsAsync()
    {
        await _fileManager.SaveFileAsAsync(CurrentFile);
    }

    private Task SaveAllFilesAsync()
    {
        var tasks = Tabs.Select(t => t.File).Select(f => _fileManager.SaveFileAsync(f));
        return Task.WhenAll(tasks);
    }

    private async Task DeleteFileAsync()
    {
        var res = await MessageBoxManager
            .GetMessageBoxStandard("Confirmation",
                $"Are you sure you want to delete the file '{CurrentFile.FileName}'?",
                ButtonEnum.YesNo, Icon.Question)
            .ShowAsync();

        if (res == ButtonResult.Yes)
        {
            _fileManager.Delete(CurrentFile);
            _tabManager.DeleteTab(_tabManager.CurrentTab);
        }
    }

    private async Task CloseFileAsync()
    {
        var res = await MessageBoxManager
            .GetMessageBoxStandard("Confirmation", $"Do you want to save the file '{CurrentFile.FileName}'?",
                ButtonEnum.YesNo, Icon.Question)
            .ShowAsync();

        if (res == ButtonResult.Yes)
        {
            await SaveFileAsync();
        }

        _tabManager.DeleteTab(_tabManager.CurrentTab);
    }

    private async Task SyncFile(FileModel currentFile)
    {
        if (string.IsNullOrWhiteSpace(currentFile.FilePath))
        {
            return;
        }

        var fileOnDisk = await _fileManager.OpenFileAsync(currentFile.FilePath);

        if (fileOnDisk.Text != currentFile.Text)
        {
            var res = await MessageBoxManager
                .GetMessageBoxStandard("Confirmation",
                    $"The file '{currentFile.FileName}' has been modified. Load changes from disk?",
                    ButtonEnum.YesNo, Icon.Question)
                .ShowAsync();

            if (res == ButtonResult.Yes)
            {
                currentFile.Text = fileOnDisk.Text;
            }
        }
    }

    private async Task SelectTabAsync(FileTab tab)
    {
        await SyncFile(tab.File);

        _tabManager.SelectTab(tab);

        this.RaisePropertyChanged(nameof(CurrentFileText));
    }
}
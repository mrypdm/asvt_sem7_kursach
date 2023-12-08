using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Media;
using GUI.Models;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

/// <inheritdoc cref="IFileTabViewModel"/>
public class FileTabViewModel : BaseViewModel<FileTab>, IFileTabViewModel
{
    public static readonly IBrush DefaultBackground = new SolidColorBrush(Colors.White);
    public static readonly IBrush SelectedBackground = new SolidColorBrush(Colors.LightGray, 0.5D);

    public static readonly IBrush DefaultForeground = new SolidColorBrush(Colors.Black);
    public static readonly IBrush NeedSaveForeground = new SolidColorBrush(Colors.DodgerBlue);

    private IBrush _currentBackground;

    /// <summary>
    /// Constructor for designer
    /// </summary>
    public FileTabViewModel() : base(null)
    {
    }

    /// <summary>
    /// Creates new instance of view model
    /// </summary>
    /// <param name="fileTab">File tab</param>
    /// <param name="file">File model</param>
    /// <param name="selectCommand">Command to select tab</param>
    /// <param name="closeCommand">Command to close tab</param>
    public FileTabViewModel(FileTab fileTab, FileModel file, Func<FileTabViewModel, Task> selectCommand,
        Func<FileTabViewModel, Task> closeCommand) : base(fileTab)
    {
        File = file;
        TabBackground = DefaultBackground;
        SelectTabCommand = ReactiveCommand.CreateFromTask(async () => await selectCommand(this));
        CloseTabCommand = ReactiveCommand.CreateFromTask(async () => await closeCommand(this));
        
        InitContext();
    }

    /// <inheritdoc />
    public FileModel File { get; }

    /// <inheritdoc />
    public string TabHeader => File.FileName;

    /// <inheritdoc />
    public IBrush TabForeground => File.IsNeedSave ? NeedSaveForeground : DefaultForeground;

    /// <inheritdoc />
    public IBrush TabBackground
    {
        get => _currentBackground;
        set => this.RaiseAndSetIfChanged(ref _currentBackground, value);
    }
    
    /// <inheritdoc />
    public bool IsSelected
    {
        get => ReferenceEquals(TabBackground, SelectedBackground);
        set => TabBackground = value ? SelectedBackground : DefaultBackground;
    }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> SelectTabCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> CloseTabCommand { get; }

    /// <summary>
    /// Notifies that header is changed
    /// </summary>
    public void NotifyHeaderChanged()
    {
        this.RaisePropertyChanged(nameof(TabHeader));
    }

    /// <summary>
    /// Notifies that foreground is changed
    /// </summary>
    public void NotifyForegroundChanged()
    {
        this.RaisePropertyChanged(nameof(TabForeground));
    }
}
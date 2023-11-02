using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using GUI.Models;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="FileTab"/>
/// </summary>
public class FileTabViewModel : BaseViewModel<FileTab>
{
    private static readonly IBrush DefaultBackground = new SolidColorBrush(Colors.White);
    private static readonly IBrush SelectedBackground = new SolidColorBrush(Colors.LightGray, 0.5D);

    private static readonly IBrush DefaultForeground = new SolidColorBrush(Colors.Black);
    private static readonly IBrush NeedSaveForeground = new SolidColorBrush(Colors.DodgerBlue);

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
    public FileTabViewModel(FileTab fileTab, FileModel file, Func<FileTab, Task> selectCommand,
        Func<FileTab, Task> closeCommand) : base(fileTab)
    {
        File = file;
        TabBackground = DefaultBackground;
        SelectTabCommand = ReactiveCommand.CreateFromTask(async () => await selectCommand(fileTab));
        CloseTabCommand = ReactiveCommand.CreateFromTask(async () => await closeCommand(fileTab));
    }

    /// <summary>
    /// File model
    /// </summary>
    public FileModel File { get; }

    /// <summary>
    /// Header for tab
    /// </summary>
    public string TabHeader => File.FileName;

    /// <summary>
    /// Current foreground
    /// </summary>
    public IBrush TabForeground => File.IsNeedSave ? NeedSaveForeground : DefaultForeground;

    /// <summary>
    /// Current background
    /// </summary>
    public IBrush TabBackground
    {
        get => _currentBackground;
        set => this.RaiseAndSetIfChanged(ref _currentBackground, value);
    }

    /// <summary>
    /// Command for select tab
    /// </summary>
    public ICommand SelectTabCommand { get; }

    /// <summary>
    /// Command for closing tab
    /// </summary>
    public ICommand CloseTabCommand { get; }

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

    /// <summary>
    /// Select tab
    /// </summary>
    public void SetTabSelected()
    {
        TabBackground = SelectedBackground;
    }

    /// <summary>
    /// Unselect tab
    /// </summary>
    public void SetTabUnselected()
    {
        TabBackground = DefaultBackground;
    }
}
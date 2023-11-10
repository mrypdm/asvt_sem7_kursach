using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Media;
using GUI.Managers;
using GUI.Views;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="MainWindowViewModel"/>
/// </summary>
public interface IMainWindowViewModel : IWindowViewModel<MainWindow>
{
    /// <summary>
    /// Command for creating file
    /// </summary>
    ICommand CreateFileCommand { get; }

    /// <summary>
    /// Command for opening file
    /// </summary>
    ICommand OpenFileCommand { get; }

    /// <summary>
    /// Command for saving file
    /// </summary>
    ICommand SaveFileCommand { get; }

    /// <summary>
    /// Command for saving all files
    /// </summary>
    ICommand SaveAllFilesCommand { get; }

    /// <summary>
    /// Command for deleting file
    /// </summary>
    ICommand DeleteFileCommand { get; }

    /// <summary>
    /// Command for creating project
    /// </summary>
    ICommand CreateProjectCommand { get; }

    /// <summary>
    /// Command for opening project
    /// </summary>
    ICommand OpenProjectCommand { get; }

    /// <summary>
    /// Command for opening <see cref="SettingsWindow"/>
    /// </summary>
    ICommand OpenSettingsWindowCommand { get; }

    /// <summary>
    /// Main window header
    /// </summary>
    string WindowTitle { get; }

    /// <summary>
    /// Collection of tabs
    /// </summary>
    ObservableCollection<FileTab> Tabs { get; }

    /// <summary>
    /// Current text of <see cref="MainWindow.SourceCodeTextBox"/>
    /// </summary>
    string FileContent { get; set; }

    /// <inheritdoc cref="SettingsManager.FontFamily"/>
    FontFamily FontFamily { get; }

    /// <inheritdoc cref="SettingsManager.FontSize"/>
    double FontSize { get; }
}
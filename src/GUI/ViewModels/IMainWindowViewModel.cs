using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Media;
using GUI.Managers;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="MainWindowViewModel"/>
/// </summary>
public interface IMainWindowViewModel : IWindowViewModel<MainWindow>
{
    /// <summary>
    /// Command for creating file
    /// </summary>
    ReactiveCommand<Unit, Unit> CreateFileCommand { get; }

    /// <summary>
    /// Command for opening file
    /// </summary>
    ReactiveCommand<Unit, Unit> OpenFileCommand { get; }

    /// <summary>
    /// Command for saving file
    /// </summary>
    ReactiveCommand<bool, Unit> SaveFileCommand { get; }

    /// <summary>
    /// Command for saving all files
    /// </summary>
    ReactiveCommand<Unit, Unit> SaveAllFilesCommand { get; }

    /// <summary>
    /// Command for deleting file
    /// </summary>
    ReactiveCommand<Unit, Unit> DeleteFileCommand { get; }

    /// <summary>
    /// Command for creating project
    /// </summary>
    ReactiveCommand<Unit, Unit> CreateProjectCommand { get; }

    /// <summary>
    /// Command for opening project
    /// </summary>
    ReactiveCommand<Unit, Unit> OpenProjectCommand { get; }

    /// <summary>
    /// Command for opening <see cref="SettingsWindow"/>
    /// </summary>
    ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }

    /// <summary>
    /// Command for opening <see cref="ExecutorWindow"/>
    /// </summary>
    ReactiveCommand<Unit, Unit> OpenExecutorWindowCommand { get; }

    /// <summary>
    /// Command for opening <see cref="ArchitectureWindow"/>
    /// </summary>
    ReactiveCommand<Unit, Unit> OpenArchitectureWindowCommand { get; }

    /// <summary>
    /// Command for opening <see cref="TutorialWindow"/>
    /// </summary>
    ReactiveCommand<Unit, Unit> OpenTutorialWindowCommand { get; }

    /// <summary>
    /// Command for building project
    /// </summary>
    ReactiveCommand<Unit, Unit> BuildProjectCommand { get; }

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
}
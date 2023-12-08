using System.Reactive;
using Avalonia.Media;
using GUI.Models;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="FileTab"/>
/// </summary>
public interface IFileTabViewModel : IViewModel<FileTab>
{
    /// <summary>
    /// Current foreground
    /// </summary>
    IBrush TabForeground { get; }

    /// <summary>
    /// Current background
    /// </summary>
    IBrush TabBackground { get; }

    /// <summary>
    /// Is tab selected
    /// </summary>
    bool IsSelected { get; }

    /// <summary>
    /// File model
    /// </summary>
    FileModel File { get; }

    /// <summary>
    /// Header for tab
    /// </summary>
    string TabHeader { get; }

    /// <summary>
    /// Command for select tab
    /// </summary>
    ReactiveCommand<Unit, Unit> SelectTabCommand { get; }

    /// <summary>
    /// Command for closing tab
    /// </summary>
    ReactiveCommand<Unit, Unit> CloseTabCommand { get; }
}
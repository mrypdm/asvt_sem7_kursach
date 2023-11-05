using System.Windows.Input;
using Avalonia.Media;
using GUI.Models;
using GUI.Views;

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
    ICommand SelectTabCommand { get; }

    /// <summary>
    /// Command for closing tab
    /// </summary>
    ICommand CloseTabCommand { get; }
}
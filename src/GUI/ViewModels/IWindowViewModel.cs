using System.Threading.Tasks;
using Avalonia.Controls;

namespace GUI.ViewModels;

/// <summary>
/// Base view model for <see cref="Window"/> heirs
/// </summary>
/// <typeparam name="TWindow">Heir of <see cref="Window"/></typeparam>
public interface IWindowViewModel<out TWindow> : IViewModel<TWindow> where TWindow: Window
{
    /// <inheritdoc cref="Window.Show()"/>
    void Show();

    /// <inheritdoc cref="Window.ShowDialog(Window)"/>
    Task ShowDialog(Window owner);

    /// <inheritdoc cref="Window.ShowDialog{TResult}(Window)"/>
    Task<TResult> ShowDialog<TResult>(Window owner);
}
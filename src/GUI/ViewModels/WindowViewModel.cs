using System.Threading.Tasks;
using Avalonia.Controls;

namespace GUI.ViewModels;

/// <summary>
/// Base view model for <see cref="Window"/> heirs
/// </summary>
/// <typeparam name="TWindow">Heir of <see cref="Window"/></typeparam>
public abstract class WindowViewModel<TWindow> : BaseViewModel<TWindow> where TWindow: Window
{
    protected WindowViewModel(TWindow view) : base(view)
    {
    }

    /// <inheritdoc cref="Window.Show()"/>
    public void Show() => View.Show();

    /// <inheritdoc cref="Window.ShowDialog(Window)"/>
    public Task ShowDialog(Window owner) => View.ShowDialog(owner);
    
    /// <inheritdoc cref="Window.ShowDialog{TResult}(Window)"/>
    public Task<TResult> ShowDialog<TResult>(Window owner) => View.ShowDialog<TResult>(owner);
}
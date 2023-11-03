using Avalonia.Controls;
using ReactiveUI;

namespace GUI.ViewModels;

/// <summary>
/// Base view model for <see cref="Control"/> heirs
/// </summary>
/// <typeparam name="TView">Heir of <see cref="Control"/></typeparam>
public abstract class BaseViewModel<TView> : ReactiveObject where TView : Control
{
    /// <summary>
    /// View
    /// </summary>
    public TView View { get; }

    protected BaseViewModel(TView view)
    {
        View = view;
    }

    /// <summary>
    /// Set this to <see cref="View"/>.<see cref="Control.DataContext"/>
    /// </summary>
    protected void InitContext()
    {
        View.DataContext = this;
    }
}
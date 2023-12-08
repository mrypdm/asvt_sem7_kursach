using Avalonia.Controls;
using ReactiveUI;

namespace GUI.ViewModels;

/// <inheritdoc cref="IViewModel{TView}"/>
public abstract class BaseViewModel<TView> : ReactiveObject, IViewModel<TView> where TView : Control
{
    /// <inheritdoc />
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
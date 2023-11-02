using Avalonia.Controls;
using ReactiveUI;

namespace GUI.ViewModels;

public abstract class BaseViewModel<TView> : ReactiveObject where TView : Control
{
    public TView View { get; }

    protected BaseViewModel(TView view)
    {
        View = view;

        if (view != null)
        {
            view.DataContext = this;
        }
    }
}
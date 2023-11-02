using ReactiveUI;

namespace GUI.ViewModels;

public abstract class BaseViewModel<TView> : ReactiveObject
{
    protected TView View { get; }

    protected BaseViewModel(TView view)
    {
        View = view;
    }
}
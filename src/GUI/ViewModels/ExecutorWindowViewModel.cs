using GUI.Views;

namespace GUI.ViewModels;

public class ExecutorViewModel : WindowViewModel<ExecutorWindow>, IExecutorViewModel
{
    /// <summary>
    /// Constructor for designer
    /// </summary>
    public ExecutorViewModel() : base(null)
    {
    }

    public ExecutorViewModel(ExecutorWindow view) : base(view)
    {
    }
}
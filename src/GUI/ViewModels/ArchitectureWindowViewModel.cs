using GUI.ViewModels.Abstraction;
using GUI.Views;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="ArchitectureWindow"/>
/// </summary>
public class ArchitectureWindowViewModel : WindowViewModel<ArchitectureWindow>
{
    public ArchitectureWindowViewModel(ArchitectureWindow view) : base(view)
    {
    }
}
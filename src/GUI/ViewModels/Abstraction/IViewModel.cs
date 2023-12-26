using Avalonia.Controls;

namespace GUI.ViewModels.Abstraction;

/// <summary>
/// View model for heirs of <see cref="Control"/>
/// </summary>
/// <typeparam name="TView">Heir of <see cref="Control"/></typeparam>
public interface IViewModel<out TView> where TView: Control
{
    /// <summary>
    /// View
    /// </summary>
    TView View { get; }
}
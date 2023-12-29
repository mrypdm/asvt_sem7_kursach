using System.Threading.Tasks;
using Avalonia.Controls;
using GUI.ViewModels.Abstraction;

namespace GUI.Providers;

/// <summary>
/// Provider for <see cref="Window"/>
/// </summary>
public interface IWindowProvider
{
    /// <summary>
    /// Shows windows as dialog
    /// </summary>
    /// <param name="owner">Owner of dialog</param>
    /// <param name="args">Args for view model constructor</param>
    /// <typeparam name="TWindow">Window to create</typeparam>
    /// <typeparam name="TViewModel">ViewModel of window</typeparam>
    Task ShowDialog<TWindow, TViewModel>(Window owner, params object[] args)
        where TWindow : Window, new()
        where TViewModel : IWindowViewModel<TWindow>;

    /// <summary>
    /// Show window or focus if already created
    /// </summary>
    /// <param name="args">Args for view model constructor</param>
    /// <typeparam name="TWindow">Window to create</typeparam>
    /// <typeparam name="TViewModel">ViewModel of window</typeparam>
    void Show<TWindow, TViewModel>(params object[] args)
        where TWindow : Window, new()
        where TViewModel : IWindowViewModel<TWindow>;

    /// <summary>
    /// Closed early created window
    /// </summary>
    /// <param name="dialogResult">Result of closing</param>
    /// <typeparam name="TWindow">Window to close</typeparam>
    /// <typeparam name="TViewModel">ViewModel of window</typeparam>
    void Close<TWindow, TViewModel>(object dialogResult = null) where TWindow : Window, new()
        where TViewModel : IWindowViewModel<TWindow>;
}
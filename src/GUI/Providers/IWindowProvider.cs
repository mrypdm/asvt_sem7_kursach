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
    /// Creates new window view model
    /// </summary>
    /// <param name="args">Args for view model constructor</param>
    /// <typeparam name="TWindow">Window to create</typeparam>
    /// <typeparam name="TViewModel">ViewModel of window</typeparam>
    /// <returns>View model of window</returns>
    IWindowViewModel<TWindow> CreateWindow<TWindow, TViewModel>(params object[] args)
        where TWindow : Window, new()
        where TViewModel : IWindowViewModel<TWindow>;

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
}
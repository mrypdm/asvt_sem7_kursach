using Avalonia.Controls;
using GUI.ViewModels;

namespace GUI.Providers;

/// <summary>
/// Provider for <see cref="Window"/>
/// </summary>
public interface IWindowProvider<TWindow> where TWindow : Window
{
    /// <summary>
    /// Creates new window view model
    /// </summary>
    /// <param name="args">Args for view model constructor</param>
    /// <typeparam name="TWindow">Window to create</typeparam>
    /// <typeparam name="TViewModel">ViewModel of window</typeparam>
    /// <returns>View model of window</returns>
    IWindowViewModel<TWindow> CreateWindow<TViewModel>(params object[] args)
        where TViewModel : IWindowViewModel<TWindow>;
}
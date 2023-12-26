using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using GUI.ViewModels.Abstraction;

namespace GUI.Providers;

/// <inheritdoc />
public class WindowProvider : IWindowProvider
{
    private static readonly HashSet<object> Windows = new();

    /// <inheritdoc />
    public IWindowViewModel<TWindow> CreateWindow<TWindow, TViewModel>(params object[] args)
        where TWindow : Window, new()
        where TViewModel : IWindowViewModel<TWindow>
    {
        args = new[] { new TWindow() }.Union(args).ToArray();
        return Activator.CreateInstance(typeof(TViewModel), args) as IWindowViewModel<TWindow>;
    }

    /// <inheritdoc />
    public Task ShowDialog<TWindow, TViewModel>(Window owner, params object[] args) where TWindow : Window, new()
        where TViewModel : IWindowViewModel<TWindow>
        => CreateWindow<TWindow, TViewModel>(args).ShowDialog(owner);

    /// <inheritdoc />
    public void Show<TWindow, TViewModel>(params object[] args) where TWindow : Window, new()
        where TViewModel : IWindowViewModel<TWindow>
    {
        var savedViewModel = Windows.FirstOrDefault(m => m is TViewModel);

        if (savedViewModel != null)
        {
            if (((TViewModel)savedViewModel).View.IsLoaded)
            {
                ((TViewModel)savedViewModel).View.WindowState = WindowState.Normal;
                ((TViewModel)savedViewModel).View.Activate();
                return;
            }

            Windows.Remove(savedViewModel);
        }

        var viewModel = CreateWindow<TWindow, TViewModel>(args);

        Windows.Add(viewModel);

        viewModel.Show();
    }
}
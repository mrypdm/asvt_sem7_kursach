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

    private static IWindowViewModel<TWindow> CreateWindow<TWindow, TViewModel>(params object[] args)
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
        var savedViewModel = (TViewModel)Windows.FirstOrDefault(m => m is TViewModel);

        if (savedViewModel != null)
        {
            if (savedViewModel.View.IsLoaded)
            {
                savedViewModel.View.WindowState = WindowState.Normal;
                savedViewModel.View.Activate();
                return;
            }

            Windows.Remove(savedViewModel);
        }

        var viewModel = CreateWindow<TWindow, TViewModel>(args);

        Windows.Add(viewModel);

        viewModel.Show();
    }


    /// <inheritdoc />
    public void Close<TWindow, TViewModel>(object dialogResult = null) where TWindow : Window, new()
        where TViewModel : IWindowViewModel<TWindow>
    {
        var savedViewModel = (TViewModel)Windows.FirstOrDefault(m => m is TViewModel);

        if (savedViewModel == null)
        {
            return;
        }

        if (savedViewModel.View.IsLoaded)
        {
            if (dialogResult != null)
            {
                savedViewModel.View.Close(dialogResult);
            }
            else
            {
                savedViewModel.View.Close();
            }
        }

        Windows.Remove(savedViewModel);
    }
}
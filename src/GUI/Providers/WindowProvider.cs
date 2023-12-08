using System;
using System.Linq;
using GUI.ViewModels;
using GUI.Views;

namespace GUI.Providers;

/// <inheritdoc />
public class SettingsWindowProvider : IWindowProvider<SettingsWindow>
{
    /// <inheritdoc />
    public IWindowViewModel<SettingsWindow> CreateWindow<TViewModel>(params object[] args)
        where TViewModel : IWindowViewModel<SettingsWindow>
    {
        args = new[] { new SettingsWindow() }.Union(args).ToArray();
        return Activator.CreateInstance(typeof(TViewModel), args) as IWindowViewModel<SettingsWindow>;
    }
}
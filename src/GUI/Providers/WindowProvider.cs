﻿using System;
using System.Linq;
using Avalonia.Controls;
using GUI.ViewModels;

namespace GUI.Providers;

/// <inheritdoc />
public class WindowProvider : IWindowProvider
{
    /// <inheritdoc />
    public IWindowViewModel<TWindow> CreateWindow<TWindow, TViewModel>(params object[] args)
        where TWindow : Window, new()
        where TViewModel : IWindowViewModel<TWindow>
    {
        args = new[] { new TWindow() }.Union(args).ToArray();
        return Activator.CreateInstance(typeof(TViewModel), args) as IWindowViewModel<TWindow>;
    }
}
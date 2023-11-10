using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Media;
using GUI.Managers;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="SettingsWindow"/>
/// </summary>
public interface ISettingsViewModel : IWindowViewModel<SettingsWindow>
{
    /// <summary>
    /// Command for adding devices
    /// </summary>
    ReactiveCommand<Unit, Unit> AddDeviceCommand { get; }
    
    /// <summary>
    /// Command for deleting devices
    /// </summary>
    ReactiveCommand<Unit, Unit> DeleteDeviceCommand { get; }
    
    /// <summary>
    /// Collection of added devices
    /// </summary>
    ObservableCollection<string> Devices { get; }
    
    /// <summary>
    /// Collection of selected devices (see <see cref="SettingsWindow"/>)
    /// </summary>
    ObservableCollection<string> SelectedDevices { get; }
    
    /// <summary>
    /// Collection of all available fonts
    /// </summary>
    ObservableCollection<FontFamily> AllFontFamilies { get; }
    
    /// <inheritdoc cref="SettingsManager.FontFamily"/>
    FontFamily FontFamily { get; }
    
    /// <inheritdoc cref="SettingsManager.FontSize"/>
    double FontSize { get; }
}
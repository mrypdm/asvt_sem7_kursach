using System.Collections.ObjectModel;
using System.Reactive;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels.Abstraction;

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
    /// Command for validating devices
    /// </summary>
    ReactiveCommand<Unit, Unit> ValidateDevicesCommand { get; }

    /// <summary>
    /// Collection of added devices
    /// </summary>
    ObservableCollection<string> Devices { get; }
    
    /// <summary>
    /// Collection of selected devices (see <see cref="SettingsWindow"/>)
    /// </summary>
    ObservableCollection<string> SelectedDevices { get; }
}
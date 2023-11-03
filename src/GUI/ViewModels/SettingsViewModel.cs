using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Domain.Models;
using GUI.Managers;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="SettingsWindow"/>
/// </summary>
public class SettingsViewModel : WindowViewModel<SettingsWindow>
{
    private readonly IProjectManager _projectManager;

    /// <summary>
    /// Empty constructor for designer
    /// </summary>
    public SettingsViewModel() : base(null)
    {
    }

    /// <summary>
    /// Creates new instance of settings window view model
    /// </summary>
    /// <param name="window">Reference to <see cref="SettingsWindow"/></param>
    /// <param name="projectManager">Project manager</param>
    public SettingsViewModel(SettingsWindow window, IProjectManager projectManager) : base(window)
    {
        _projectManager = projectManager;
        AllFontFamilies = new ObservableCollection<FontFamily>(FontManager.Current.SystemFonts);

        AddDeviceCommand = ReactiveCommand.CreateFromTask(AddDeviceAsync);
        DeleteDeviceCommand = ReactiveCommand.CreateFromTask(DeleteDevices);

        projectManager.PropertyChanged += ProjectPropertyChanged;

        window.Closing += async (_, _) =>
        {
            projectManager.PropertyChanged -= ProjectPropertyChanged;
            await SettingsManager.Instance.SaveGlobalSettings();
        };
        
        InitContext();
    }

    /// <summary>
    /// Command for adding devices
    /// </summary>
    public ReactiveCommand<Unit, Unit> AddDeviceCommand { get; }

    /// <summary>
    /// Command for deleting devices
    /// </summary>
    public ReactiveCommand<Unit, Unit> DeleteDeviceCommand { get; }

    /// <summary>
    /// Collection with all fonts
    /// </summary>
    public ObservableCollection<FontFamily> AllFontFamilies { get; }

    /// <inheritdoc cref="ProjectModel.Devices"/>
    public ObservableCollection<string> Devices => new(_projectManager.IsOpened
        ? _projectManager.Project.Devices
        : Array.Empty<string>());

    /// <summary>
    /// Collection with selected devices
    /// </summary>
    public ObservableCollection<string> SelectedDevices { get; set; } = new();

    /// <inheritdoc cref="SettingsManager.FontFamily"/>
    public FontFamily FontFamily
    {
        get => SettingsManager.Instance.FontFamily;
        set => SettingsManager.Instance.FontFamily = value;
    }

    /// <inheritdoc cref="SettingsManager.FontSize"/>
    public double FontSize
    {
        get => SettingsManager.Instance.FontSize;
        set => SettingsManager.Instance.FontSize = value;
    }

    private async Task AddDeviceAsync()
    {
        var file = await View.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open device library...",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("DLL") { Patterns = new[] { "*.dll" } } }
        });

        if (file.Count != 1)
        {
            return;
        }

        _projectManager.AddDeviceToProject(file[0].Path.LocalPath);
        await _projectManager.SaveProjectAsync();
    }

    private async Task DeleteDevices()
    {
        var devices = SelectedDevices.ToList();
        foreach (var device in devices)
        {
            _projectManager.RemoveDeviceFromProject(device);
        }
        await _projectManager.SaveProjectAsync();
    }

    private void ProjectPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        this.RaisePropertyChanged(nameof(Devices));
    }
}
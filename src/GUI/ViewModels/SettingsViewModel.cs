using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Domain.Models;
using GUI.Managers;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

/// <inheritdoc cref="ISettingsViewModel"/>
public class SettingsViewModel : WindowViewModel<SettingsWindow>, ISettingsViewModel
{
    private readonly IProjectManager _projectManager;
    private readonly IFileManager _fileManager;

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
    /// <param name="fileManager">File manager</param>
    public SettingsViewModel(SettingsWindow window, IProjectManager projectManager, IFileManager fileManager) :
        base(window)
    {
        _projectManager = projectManager;
        _fileManager = fileManager;
        AllFontFamilies = new ObservableCollection<FontFamily>(FontManager.Current.SystemFonts);

        AddDeviceCommand = ReactiveCommand.CreateFromTask(AddDeviceAsync);
        DeleteDeviceCommand = ReactiveCommand.CreateFromTask(DeleteDevices);

        projectManager.PropertyChanged += ProjectPropertyChanged;

        window.Closing += async (_, _) =>
        {
            projectManager.PropertyChanged -= ProjectPropertyChanged;
            await SettingsManager.Instance.SaveGlobalSettingsAsync();
        };

        InitContext();
    }

    /// <inheritdoc />
    public ICommand AddDeviceCommand { get; }

    /// <inheritdoc />
    public ICommand DeleteDeviceCommand { get; }

    /// <inheritdoc />
    public ObservableCollection<FontFamily> AllFontFamilies { get; }

    /// <inheritdoc cref="ProjectModel.Devices"/>
    public ObservableCollection<string> Devices => new(_projectManager.IsOpened
        ? _projectManager.Project.Devices
        : Array.Empty<string>());

    /// <inheritdoc />
    public ObservableCollection<string> SelectedDevices { get; set; } = new();

    /// <inheritdoc />
    public FontFamily FontFamily
    {
        get => SettingsManager.Instance.FontFamily;
        set => SettingsManager.Instance.FontFamily = value;
    }

    /// <inheritdoc />
    public double FontSize
    {
        get => SettingsManager.Instance.FontSize;
        set => SettingsManager.Instance.FontSize = value;
    }

    private async Task AddDeviceAsync()
    {
        var options = new FilePickerOpenOptions
        {
            Title = "Open device library...",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("DLL") { Patterns = new[] { "*.dll" } } }
        };

        var file = await _fileManager.GetFileAsync(View.StorageProvider, options);

        if (file == null)
        {
            return;
        }

        _projectManager.AddDeviceToProject(file);
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

    private void ProjectPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName is nameof(_projectManager.Project) or nameof(_projectManager.Project.Devices))
        {
            this.RaisePropertyChanged(nameof(Devices));
        }
    }
}
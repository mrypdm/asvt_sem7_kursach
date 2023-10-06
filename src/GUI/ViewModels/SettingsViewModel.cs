using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using DynamicData.Binding;
using GUI.Managers;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

public class SettingsViewModel : ReactiveObject
{
    private readonly IStorageProvider _storageProvider;

    /// <summary>
    /// Empty constructor for designer
    /// </summary>
    public SettingsViewModel()
    {
    }

    /// <summary>
    /// Creates new instance of settings window view model
    /// </summary>
    /// <param name="window">Reference to <see cref="SettingsWindow"/></param>
    public SettingsViewModel(Window window)
    {
        _storageProvider = window.StorageProvider;
        AllFontFamilies = new ObservableCollectionExtended<FontFamily>(FontManager.Current.SystemFonts);

        AddExternalDeviceCommand = ReactiveCommand.CreateFromTask(AddExternalDeviceAsync);
        DeleteExternalDeviceCommand = ReactiveCommand.CreateFromTask(DeleteExternalDevices);

        ProjectManager.Instance.PropertyChanged += (_, _) => this.RaisePropertyChanged(nameof(ExternalDevices));

        window.Closing += async (_, _) => { await SettingsManager.Instance.SaveGlobalSettings(); };
    }

    /// <summary>
    /// Command for adding external devices
    /// </summary>
    public ReactiveCommand<Unit, Unit> AddExternalDeviceCommand { get; }

    /// <summary>
    /// Command for deleting external devices
    /// </summary>
    public ReactiveCommand<Unit, Unit> DeleteExternalDeviceCommand { get; }

    /// <summary>
    /// Collection with all fonts
    /// </summary>
    public ObservableCollection<FontFamily> AllFontFamilies { get; }

    /// <inheritdoc cref="SettingsManager.ExternalDevices"/>
    public ObservableCollection<string> ExternalDevices => new(ProjectManager.Instance.ExternalDevices);

    /// <summary>
    /// Collection with selected external devices
    /// </summary>
    public ObservableCollection<string> SelectedExternalDevices { get; set; } = new();

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

    private async Task AddExternalDeviceAsync()
    {
        var file = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open external device library...",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("DLL") { Patterns = new[] { "*.dll" } } }
        });

        if (file.Count != 1)
        {
            return;
        }

        ProjectManager.Instance.AddExternalDevice(file[0].Path.LocalPath);
        await ProjectManager.Instance.SaveProjectAsync();
        this.RaisePropertyChanged(nameof(ExternalDevices));
    }

    private async Task DeleteExternalDevices()
    {
        var externalDevices = SelectedExternalDevices.ToList();

        foreach (var device in externalDevices)
        {
            ProjectManager.Instance.RemoveExternalDevice(device);
        }

        await ProjectManager.Instance.SaveProjectAsync();
        this.RaisePropertyChanged(nameof(ExternalDevices));
    }
}
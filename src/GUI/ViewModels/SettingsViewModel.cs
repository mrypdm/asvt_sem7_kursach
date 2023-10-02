using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using DynamicData;
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
    public SettingsViewModel(TopLevel window)
    {
        _storageProvider = window.StorageProvider;
        AllFontFamilies = new ObservableCollectionExtended<FontFamily>(FontManager.Current.SystemFonts);
        
        AddExternalDeviceCommand = ReactiveCommand.CreateFromTask(AddExternalDeviceAsync);
        DeleteExternalDeviceCommand = ReactiveCommand.Create(DeleteExternalDevices);
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
    public ObservableCollection<string> ExternalDevices
    {
        get => SettingsManager.Instance.ExternalDevices;
        set
        {
            SettingsManager.Instance.ExternalDevices = value;
            this.RaisePropertyChanged();
        }
    }

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

    /// <inheritdoc cref="SettingsManager.ProgramAddress"/>
    public int ProgramAddress
    {
        get => SettingsManager.Instance.ProgramAddress;
        set => SettingsManager.Instance.ProgramAddress = value;
    }

    /// <inheritdoc cref="SettingsManager.StackAddress"/>
    public int StackAddress
    {
        get => SettingsManager.Instance.StackAddress;
        set => SettingsManager.Instance.StackAddress = value;
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

        SettingsManager.Instance.ExternalDevices.Add(file[0].Path.AbsolutePath);
    }

    private void DeleteExternalDevices()
    {
        SettingsManager.Instance.ExternalDevices.RemoveMany(SelectedExternalDevices);
    }
}
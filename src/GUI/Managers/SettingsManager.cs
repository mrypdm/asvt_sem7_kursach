using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media;
using GUI.Models;
using GUI.Notifiers;
using Microsoft.Extensions.Configuration;
using Shared.Helpers;

namespace GUI.Managers;

/// <summary>
/// Setting manager
/// </summary>
public sealed class SettingsManager : PropertyChangedNotifier
{
    private FontFamily _fontFamily;
    private double _fontSize;

    private int _programAddress = 1024; // 0o2000
    private int _stackAddress = 1024; // 0o2000

    /// <summary>
    /// Current editor font family
    /// </summary>
    public FontFamily FontFamily
    {
        get => _fontFamily;
        set => SetField(ref _fontFamily, value);
    }

    /// <summary>
    /// Current editor font size
    /// </summary>
    public double FontSize
    {
        get => _fontSize;
        set => SetField(ref _fontSize, value);
    }

    /// <summary>
    /// Current connected external devices
    /// </summary>
    public ObservableCollection<string> ExternalDevices { get; set; } = new();

    /// <summary>
    /// Address in memory where the program will be located
    /// </summary>
    public int ProgramAddress
    {
        get => _programAddress;
        set => SetField(ref _programAddress, value);
    }

    /// <summary>
    /// Stack pointer starting address
    /// </summary>
    public int StackAddress
    {
        get => _stackAddress;
        set => SetField(ref _stackAddress, value);
    }

    /// <summary>
    /// Get instance of settings manager
    /// </summary>
    public static SettingsManager Instance { get; private set; }

    private SettingsManager()
    {
    }

    /// <summary>
    /// Creates settings manager
    /// </summary>
    /// <param name="globalConfiguration">Global configuration of editor</param>
    public static void Create(IConfigurationRoot globalConfiguration)
    {
        var editorOptions = globalConfiguration.GetOptions<EditorOptions>();

        Instance ??= new SettingsManager
        {
            FontFamily = editorOptions.FontFamily,
            FontSize = editorOptions.FontSize
        };
    }

    /// <summary>
    /// Save editor settings to disk <see cref="FontFamily"/>, <see cref="FontSize"/>
    /// </summary>
    public async Task SaveGlobalSettings()
    {
        await ConfigurationHelper.SaveToJson(new Dictionary<string, object>
        {
            {
                nameof(EditorOptions), new EditorOptions
                {
                    FontFamily = FontFamily.Name,
                    FontSize = FontSize
                }
            }
        });
    }
}
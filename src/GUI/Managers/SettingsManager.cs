using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media;
using GUI.Models;
using GUI.Notifiers;
using Shared.Helpers;

namespace GUI.Managers;

/// <summary>
/// Setting manager
/// </summary>
public sealed class SettingsManager : PropertyChangedNotifier
{
    private FontFamily _fontFamily;
    private double _fontSize;

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
    /// Get instance of settings manager
    /// </summary>
    public static SettingsManager Instance { get; private set; }

    private SettingsManager(EditorOptions options)
    {
        FontFamily = options.FontFamily;
        FontSize = options.FontSize;
    }

    /// <summary>
    /// Creates settings manager
    /// </summary>
    /// <param name="editorOptions">Editor options</param>
    public static void Create(EditorOptions editorOptions)
    {
        Instance ??= new SettingsManager(editorOptions);
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
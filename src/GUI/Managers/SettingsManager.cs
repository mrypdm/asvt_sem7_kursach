using System.Collections.ObjectModel;
using Avalonia.Media;

namespace GUI.Managers;

public sealed class SettingsManager : PropertyChangedNotifier
{
    private FontFamily _fontFamily = new("Courier New");
    private double _fontSize = 24;

    public FontFamily FontFamily
    {
        get => _fontFamily;
        set => SetField(ref _fontFamily, value);
    }

    public double FontSize
    {
        get => _fontSize;
        set => SetField(ref _fontSize, value);
    }

    public ObservableCollection<string> ExternalDevices { get; set; } = new();

    private SettingsManager()
    {
    }

    private static SettingsManager _instance;

    public static SettingsManager Create()
    {
        _instance ??= new SettingsManager();
        return _instance;
    }
}
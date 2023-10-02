﻿using System.Collections.ObjectModel;
using Avalonia.Media;
using GUI.Models;
using GUI.Notifiers;

namespace GUI.Managers;

/// <summary>
/// Setting manager
/// </summary>
public sealed class SettingsManager : PropertyChangedNotifier
{
    private FontFamily _fontFamily = new("Courier New");
    private double _fontSize = 24;
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
    /// <param name="options"></param>
    public static void Create(EditorOptions options)
    {
        Instance ??= new SettingsManager
        {
            FontFamily = options.FontFamily,
            FontSize = options.FontSize
        };
    }
}
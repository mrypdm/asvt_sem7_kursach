using System;
using GUI.Notifiers;
using HarfBuzzSharp;

namespace GUI.Models.Executor;

/// <summary>
/// Model of code line
/// </summary>
public class CodeLine : PropertyChangedNotifier
{
    private bool _breakpoint;
    private ushort _code;

    public CodeLine(ushort address, ushort machineCode, bool breakpoint, string sourceCode = null)
    {
        Address = address;
        Code = machineCode;
        Text = sourceCode ?? string.Empty;
        _breakpoint = breakpoint;
    }

    /// <summary>
    /// Is line breakpoint
    /// </summary>
    public bool Breakpoint
    {
        get => _breakpoint;
        set => SetField(ref _breakpoint, value);
    }

    /// <summary>
    /// Address of code line
    /// </summary>
    public ushort Address { get; }

    /// <summary>
    /// Machine code
    /// </summary>
    public ushort Code
    {
        get => _code;
        set => SetField(ref _code, value);
    }

    /// <summary>
    /// Source code
    /// </summary>
    public string Text { get; }
}
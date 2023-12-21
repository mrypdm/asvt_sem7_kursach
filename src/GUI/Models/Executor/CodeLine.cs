using System;
using GUI.Notifiers;

namespace GUI.Models.Executor;

/// <summary>
/// Model of code line
/// </summary>
public class CodeLine : PropertyChangedNotifier
{
    private bool _breakpoint;

    public CodeLine(ushort address, ushort machineCode, bool breakpoint, string sourceCode = null)
    {
        Address = Convert.ToString(address, 8).PadLeft(6, '0');
        Code = Convert.ToString(machineCode, 8).PadLeft(6, '0');
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
    public string Address { get; }

    /// <summary>
    /// Machine code
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Source code
    /// </summary>
    public string Text { get; }
}
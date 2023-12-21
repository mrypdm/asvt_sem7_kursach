using System;
using Executor.Models;
using GUI.Notifiers;

namespace GUI.Models.Executor;

/// <summary>
/// Model of code line
/// </summary>
public class CodeLine : PropertyChangedNotifier
{
    private bool _breakpoint;
    private ushort _code;

    public static CodeLine FromDto(Command command) =>
        new(command.Address, command.Value, command.BreakPoint, command.Symbol);

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
        set => SetField(ref _code, value, nameof(CodeText));
    }

    /// <summary>
    /// Source code
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Address of code line
    /// </summary>
    public string AddressText => Convert.ToString(Address, 8).PadLeft(6, '0');

    /// <summary>
    /// Machine code
    /// </summary>
    public string CodeText => Convert.ToString(Code, 8).PadLeft(6, '0');
}
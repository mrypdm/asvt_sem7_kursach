using System;

namespace GUI.Models.Executor;

/// <summary>
/// Model of code line
/// </summary>
public class CodeModel
{
    public CodeModel(ushort address, ushort machineCode, string sourceCode = null)
    {
        Address = Convert.ToString(address, 8).PadLeft(6, '0');
        Code = Convert.ToString(machineCode, 8).PadLeft(6, '0');
        Text = sourceCode ?? string.Empty;
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
using System;

namespace GUI.Models.Executor;

/// <summary>
/// Memory cell as word
/// </summary>
public class WordModel : IMemoryModel
{
    public WordModel(ushort address, ushort value)
    {
        Address = Convert.ToString(address, 8).PadLeft(6, '0');
        Value = Convert.ToString(value, 8).PadLeft(6, '0');
    }
    
    /// <summary>
    /// Address of word
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Value of word
    /// </summary>
    public string Value { get; }
}
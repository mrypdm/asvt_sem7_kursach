using System;

namespace GUI.Models.Executor;

/// <summary>
/// Memory cell as byte
/// </summary>
public class ByteModel : IMemoryModel
{
    public ByteModel(ushort address, byte value)
    {
        Address = Convert.ToString(address, 8).PadLeft(6, '0');
        Value = Convert.ToString(value, 8).PadLeft(3, '0');
    }

    /// <summary>
    /// Address of byte
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Value of byte
    /// </summary>
    public string Value { get; }
}
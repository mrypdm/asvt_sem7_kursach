using System;

namespace GUI.Models.Executor;

/// <summary>
/// Model of register
/// </summary>
public class RegisterModel
{
    public RegisterModel(int number, ushort value)
    {
        Name = $"R{number}";
        Value = Convert.ToString(value, 8).PadLeft(6, '0');
    }

    /// <summary>
    /// Name of register
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Value of register
    /// </summary>
    public string Value { get; }
}
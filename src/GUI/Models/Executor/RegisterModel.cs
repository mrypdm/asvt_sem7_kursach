using System;

namespace GUI.Models.Executor;

/// <summary>
/// Model of register
/// </summary>
public class RegisterModel
{
    public RegisterModel(int number, ushort value)
    {
        Name = number switch
        {
            >= 0 and < 6 => $"R{number}",
            6 => "SP",
            7 => "PC",
            _ => throw new ArgumentOutOfRangeException(nameof(number), number,
                "Register number must be in range from 0 to 7")
        };

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
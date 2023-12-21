namespace Executor.Arguments.Abstraction;

/// <summary>
/// Argument referencing a register
/// </summary>
/// <typeparam name="TValue"><see cref="byte"/> or <see cref="ushort"/></typeparam>
public interface IRegisterArgument<TValue> : IArgument
{
    /// <summary>
    /// Number of register
    /// </summary>
    ushort Register { get; }
    
    /// <summary>
    /// Addressing mode
    /// </summary>
    ushort Mode { get; }

    /// <summary>
    /// Value of argument
    /// </summary>
    TValue Value { get; set; }
    
    /// <summary>
    /// Address of argument (null if register is argument)
    /// </summary>
    ushort? Address { get; }
}
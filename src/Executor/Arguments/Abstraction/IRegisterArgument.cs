using System;

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
    /// Returns getter and setter for argument
    /// </summary>
    (Func<TValue> source, Action<TValue> destination) GetSourceAndDestination();
}
using Executor.Arguments.Abstraction;
using Executor.Commands;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

/// <summary>
/// Argument for <see cref="SOB"/>
/// </summary>
public class SobArgument : IArgument
{
    public SobArgument(ushort register, byte offset)
    {
        Register = register;
        Offset = offset;
    }

    /// <inheritdoc />
    public object GetValue() => (Register, Offset);

    /// <inheritdoc />
    public void SetValue(object word) => throw new ReadOnlyArgumentException(typeof(SobArgument));

    /// <summary>
    /// Register number
    /// </summary>
    public ushort Register { get; }

    /// <summary>
    /// Offset value
    /// </summary>
    public byte Offset { get; }
}
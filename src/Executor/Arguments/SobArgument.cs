using Executor.Arguments.Abstraction;
using Executor.Commands;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

/// <summary>
/// Argument for <see cref="SOB"/>
/// </summary>
public class SobArgument : BaseArgument
{
    public SobArgument(IStorage storage, IState state, ushort register, byte offset) : base(storage, state)
    {
        Register = register;
        Offset = offset;
    }

    /// <inheritdoc />
    public override object GetValue() => (Register, Offset);

    /// <inheritdoc />
    public override void SetValue(object word) => throw new ReadOnlyArgumentException(typeof(SobArgument));

    /// <summary>
    /// Register number
    /// </summary>
    public ushort Register { get; }

    /// <summary>
    /// Offset value
    /// </summary>
    public byte Offset { get; }
}
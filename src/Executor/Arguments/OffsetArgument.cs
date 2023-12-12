using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.Storages;

namespace Executor.Arguments;

/// <inheritdoc cref="IOffsetArgument"/>
public class OffsetArgument : BaseArgument, IOffsetArgument
{
    /// <inheritdoc />
    public override object GetValue() => Offset;

    /// <inheritdoc />
    public override void SetValue(object obj) => throw new ReadOnlyArgumentException(typeof(OffsetArgument));

    /// <inheritdoc />
    public sbyte Offset { get; }

    public OffsetArgument(IStorage storage, IState state, sbyte offset) : base(storage, state)
    {
        Offset = offset;
    }
}
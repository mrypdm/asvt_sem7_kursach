using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.Storages;

namespace Executor.Arguments;

public class OffsetArgument : BaseArgument, IOffsetArgument
{
    public override object GetValue() => Offset;

    public override void SetValue(object obj) => throw new ReadOnlyArgumentException(typeof(OffsetArgument));

    public sbyte Offset { get; }

    public OffsetArgument(IStorage storage, IState state, sbyte offset) : base(storage, state)
    {
        Offset = offset;
    }
}
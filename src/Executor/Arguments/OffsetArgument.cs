using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.Storages;

namespace Executor.Arguments;

public class OffsetArgument : BaseArgument, IOffsetArgument
{
    private readonly byte _offset;

    public override object GetValue() => GetOffset();

    public override void SetValue(object obj) => throw new ReadOnlyArgumentException(typeof(OffsetArgument));

    public byte GetOffset() => _offset;

    public OffsetArgument(IStorage storage, IState state, byte offset) : base(storage, state)
    {
        _offset = offset;
    }
}
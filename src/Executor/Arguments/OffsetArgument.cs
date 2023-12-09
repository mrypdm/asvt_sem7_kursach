using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.Storages;

namespace Executor.Arguments;

public class OffsetArgument : BaseArgument, IOffsetArgument
{
    private readonly sbyte _offset;

    public override object GetValue() => GetOffset();

    public override void SetValue(object obj) => throw new ReadOnlyArgumentException(typeof(OffsetArgument));

    public sbyte GetOffset() => _offset;

    public OffsetArgument(IStorage storage, IState state, sbyte offset) : base(storage, state)
    {
        _offset = offset;
    }
}
using Executor.Memories;
using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;

namespace Executor.Arguments;

public class OffsetArgument : BaseArgument, IOffsetArgument
{
    private readonly byte _offset;

    public override object GetValue() => GetOffset();

    public override void SetValue(object obj) => throw new ReadOnlyArgumentException(typeof(OffsetArgument));

    public byte GetOffset() => _offset;

    public OffsetArgument(IMemory memory, IState state, byte offset) : base(memory, state)
    {
        _offset = offset;
    }
}
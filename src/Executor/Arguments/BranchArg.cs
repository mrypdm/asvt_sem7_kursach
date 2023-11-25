using Executor.Memories;
using Executor.States;
using System;

namespace Executor.Arguments;

class BranchArg : BaseArgument
{
    private ushort _offset;

    public override ushort GetValue()
    {
        return _offset;
    }

    public override void SetValue(ushort word)
    {
        throw new InvalidOperationException("Can't set value for this argument!")
    }

    public BranchArg(IMemory memory, IState state, ushort offset) : base(memory, state)
    {
        _offset = offset;
    }
}
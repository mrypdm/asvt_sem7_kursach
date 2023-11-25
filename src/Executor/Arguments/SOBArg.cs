using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

class SOBArg : BaseArgument
{
    private ushort _offset;
    private ushort _register;

    public override ushort GetValue()
    {
        return 0;
    }

    public override void SetValue(ushort word)
    {
        throw new InvalidOperationException("Can't set value for this argument!");
    }

    public SOBArg(IMemory memory, IState state, ushort register, ushort offset) : base(memory, state)
    {
        _register = register;
        _offset = offset;
    }
}
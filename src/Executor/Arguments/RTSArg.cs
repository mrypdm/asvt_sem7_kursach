using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

internal class RTSArg : BaseArgument
{
    private ushort _register;

    public override ushort GetValue()
    {
        return 0;
    }

    public override void SetValue(ushort word)
    {
        throw new InvalidOperationException("Can't set value for this argument!");
    }

    public RTSArg(IMemory memory, IState state, ushort register) : base(memory, state)
    {
        _register = register;
    }
}
using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

class RTSArg : BaseArgument
{
    private ushort _register;

    public override ushort GetValue()
    {
        return 0;
    }

    public override void SetValue(ushort word)
    {
    }

    public RTSArg(IMemory memory, IState state, ushort register) : base(memory, state)
    {
        _register = register;
    }
}
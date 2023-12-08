using Executor.Exceptions;
using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

public class RTSArg : BaseArgument
{
    private ushort _register;

    public override object GetValue()
    {
        return 0;
    }

    public override void SetValue(object obj) => throw new ReadOnlyArgumentException(typeof(RTSArg));

    public RTSArg(IMemory memory, IState state, ushort register) : base(memory, state)
    {
        _register = register;
    }
}
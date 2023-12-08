using Executor.Exceptions;
using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

public class SOBArg : BaseArgument
{
    private ushort _offset;
    private ushort _register;

    public override object GetValue()
    {
        return 0;
    }

    public override void SetValue(object word) => throw new ReadOnlyArgumentException(typeof(SOBArg));

    public SOBArg(IMemory memory, IState state, ushort register, ushort offset) : base(memory, state)
    {
        _register = register;
        _offset = offset;
    }
}
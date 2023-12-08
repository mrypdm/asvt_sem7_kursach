using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

public class RTSArg : BaseArgument
{
    private ushort _register;

    public override object GetValue()
    {
        return 0;
    }

    public override void SetValue(object obj) => throw new ReadOnlyArgumentException(typeof(RTSArg));

    public RTSArg(IStorage storage, IState state, ushort register) : base(storage, state)
    {
        _register = register;
    }
}
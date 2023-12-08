using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

public abstract class BaseArgument : IArgument
{
    protected IMemory Memory { get; }

    protected IState State { get; }

    public abstract ushort GetValue();

    public abstract void SetValue(ushort word);

    protected BaseArgument(IMemory memory, IState state)
    {
        Memory = memory;
        State = state;
    }
}
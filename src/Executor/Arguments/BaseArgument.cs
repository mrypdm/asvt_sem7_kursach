using Executor.Arguments.Abstraction;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

public abstract class BaseArgument : IArgument
{
    protected IStorage Storage { get; }

    protected IState State { get; }

    public abstract object GetValue();

    public abstract void SetValue(object value);

    protected BaseArgument(IStorage storage, IState state)
    {
        Storage = storage;
        State = state;
    }
}
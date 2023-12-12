using Executor.States;
using Executor.Storages;

namespace Executor.Arguments.Abstraction;

/// <summary>
/// Base class of argument
/// </summary>
public abstract class BaseArgument : IArgument
{
    protected IStorage Storage { get; }

    protected IState State { get; }

    /// <inheritdoc />
    public abstract object GetValue();

    /// <inheritdoc />
    public abstract void SetValue(object value);

    protected BaseArgument(IStorage storage, IState state)
    {
        Storage = storage;
        State = state;
    }
}
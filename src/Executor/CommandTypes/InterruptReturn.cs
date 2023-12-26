using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

/// <summary>
/// Base class for return from interrupt commands
/// </summary>
public abstract class InterruptReturn : BaseCommand
{
    protected InterruptReturn(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <summary>
    /// Handle return
    /// </summary>
    protected void HandleReturn()
    {
        State.Registers[7] = Storage.PopFromStack(State);
        State.ProcessorStateWord = Storage.PopFromStack(State);
    }
}
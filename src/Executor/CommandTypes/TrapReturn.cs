using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

/// <summary>
/// Trap return
/// </summary>
public abstract class TrapReturn : BaseCommand
{
    protected TrapReturn(IStorage storage, IState state) : base(storage, state)
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
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class TrapReturn : BaseCommand
{
    protected TrapReturn(IStorage storage, IState state) : base(storage, state)
    {
    }

    protected void HandleReturn()
    {
        State.Registers[7] = Storage.PopFromStack(State);
        State.ProcessorStateWord = Storage.PopFromStack(State);
    }
}
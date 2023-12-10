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
        State.Registers[7] = Storage.GetWord(State.Registers[6]);
        State.Registers[6] += 2;
        State.ProcessorStateWord = Storage.GetWord(State.Registers[6]);
        State.Registers[6] += 2;
    }
}
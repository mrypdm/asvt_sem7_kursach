using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class TrapInstruction : BaseCommand
{
    protected TrapInstruction(IStorage storage, IState state) : base(storage, state)
    {
    }

    protected void HandleTrap(ushort trapVectorAddress)
    {
        var newPc = Storage.GetWord(trapVectorAddress);
        var newPsw = Storage.GetWord((ushort)(trapVectorAddress + 2));

        State.Registers[6] -= 2;
        State.Registers[6] = State.ProcessorStateWord;
        State.Registers[6] -= 2;
        State.Registers[6] = State.Registers[7];

        State.Registers[7] = newPc;
        State.ProcessorStateWord = newPsw;
    }
}
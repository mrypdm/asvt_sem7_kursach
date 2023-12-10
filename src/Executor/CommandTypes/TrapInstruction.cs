using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class TrapInstruction : BaseCommand
{
    protected TrapInstruction(IStorage storage, IState state) : base(storage, state)
    {
    }

    protected void HandleTrap(ushort trapVectorAddress) => HandleInterrupt(Storage, State, trapVectorAddress);

    public static void HandleInterrupt(IStorage storage, IState state, ushort vectorAddress)
    {
        var newPc = storage.GetWord(vectorAddress);
        var newPsw = storage.GetWord((ushort)(vectorAddress + 2));

        state.Registers[6] -= 2;
        state.Registers[6] = state.ProcessorStateWord;
        state.Registers[6] -= 2;
        state.Registers[6] = state.Registers[7];

        state.Registers[7] = newPc;
        state.ProcessorStateWord = newPsw;
    }
}
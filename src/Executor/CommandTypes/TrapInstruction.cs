using Executor.Extensions;
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
        storage.PushToStack(state, state.ProcessorStateWord);
        storage.PushToStack(state, state.Registers[7]);

        state.Registers[7] = storage.GetWord(vectorAddress);
        state.ProcessorStateWord = storage.GetWord((ushort)(vectorAddress + 2));
    }
}
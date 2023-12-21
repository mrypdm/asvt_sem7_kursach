using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

/// <summary>
/// Trap instruction
/// </summary>
public abstract class TrapInstruction : BaseCommand
{
    protected TrapInstruction(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <summary>
    /// Handle trap
    /// </summary>
    /// <param name="trapVectorAddress">Address of interrupt vector</param>
    protected void HandleTrap(ushort trapVectorAddress) => HandleInterrupt(Storage, State, trapVectorAddress);

    /// <summary>
    /// Handle interrupt
    /// </summary>
    /// <param name="storage">Storage</param>
    /// <param name="state">State</param>
    /// <param name="vectorAddress">Address of interrupt vector</param>
    public static void HandleInterrupt(IStorage storage, IState state, ushort vectorAddress)
    {
        storage.PushToStack(state, state.ProcessorStateWord);
        storage.PushToStack(state, state.Registers[7]);

        state.Registers[7] = storage.GetWord(vectorAddress);
        state.ProcessorStateWord = storage.GetWord((ushort)(vectorAddress + 2));
    }
}
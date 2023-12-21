using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class FloatingInstructionSet : BaseCommand
{
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    protected static ushort GetRegister(ushort word) => (ushort)(word & RegisterMask);

    protected FloatingInstructionSet(IStorage storage, IState state) : base(storage, state)
    {
    }
}
using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class FloatingInstructionSet : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_1111_1000;
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    protected ushort GetRegister(ushort word) => (ushort)(word & RegisterMask);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    protected FloatingInstructionSet(IStorage storage, IState state) : base(storage, state)
    {
    }
}
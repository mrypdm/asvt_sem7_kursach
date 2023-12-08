using Executor.Memories;
using Executor.States;

namespace Executor.CommandTypes;

internal abstract class BitOperations : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1110_0000_0000;
    private const ushort Register1Mask = 0b0000_0001_1100_0000;
    private const ushort ModeMask = 0b0000_0000_0011_1000;
    private const ushort Register2Mask = 0b0000_0000_0000_0111;

    protected ushort GetRegister1(ushort word) => (ushort)((word & Register1Mask) >> 6);

    protected ushort GetRegister2(ushort word) => (ushort)(word & Register2Mask);

    protected ushort GetMode(ushort word) => (ushort)(word & ModeMask);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    protected BitOperations(IMemory memory, IState state) : base(memory, state)
    {
    }
}
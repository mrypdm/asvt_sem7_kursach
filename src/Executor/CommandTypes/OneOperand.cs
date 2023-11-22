using Executor.Memories;
using Executor.States;

namespace Executor.CommandTypes;

public abstract class OneOperand : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_1100_0000;
    private const ushort SourceMask = 0b0000_0000_0011_1000;
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    protected ushort GetRegister(ushort word) => (ushort)(word & RegisterMask);

    protected ushort GetMode(ushort word) => (ushort)((word & SourceMask) >> 3);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    protected OneOperand(IMemory memory, IState state) : base(memory, state)
    {
    }
}
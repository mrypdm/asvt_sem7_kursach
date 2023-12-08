using Executor.Memories;
using Executor.States;

namespace Executor.CommandTypes;

internal abstract class FloatingInstructionSet : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_1111_1000;
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    protected ushort GetRegister(ushort word) => (ushort)(word & RegisterMask);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    protected FloatingInstructionSet(IMemory memory, IState state) : base(memory, state)
    {
    }
}
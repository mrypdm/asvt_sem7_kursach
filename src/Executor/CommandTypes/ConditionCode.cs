using Executor.Memories;
using Executor.States;

namespace Executor.CommandTypes;

internal abstract class ConditionCode : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_1111_0000;
    private const ushort FlagMask = 0b0000_0000_0000_1111;

    protected ushort GetRegister(ushort word) => (ushort)(word & FlagMask);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    protected ConditionCode(IMemory memory, IState state) : base(memory, state)
    {
    }
}
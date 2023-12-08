using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class TrapInstruction : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_0000_0000;
    private const ushort OperationCodeMask = 0b0000_0000_1111_1111;

    protected ushort GetOpcode(ushort word) => (ushort)(word & OpcodeMask);

    protected ushort GetOperationCode(ushort word) => (ushort)(word & OperationCodeMask);

    protected TrapInstruction(IStorage storage, IState state) : base(storage, state)
    {
    }
}
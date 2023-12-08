using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class TrapReturn : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_1111_1111;

    protected ushort GetOpcode(ushort word) => (ushort)(word & OpcodeMask);

    protected TrapReturn(IStorage storage, IState state) : base(storage, state)
    {
    }
}
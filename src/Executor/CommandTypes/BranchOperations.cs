using Executor.Arguments;
using Executor.Memories;
using Executor.States;
using Executor.Arguments.Abstraction;

namespace Executor.CommandTypes;

public abstract class BranchOperation : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_0000_0000;
    private const ushort OffsetMask = 0b0000_0000_1111_1111;

    private byte GetOffset(ushort word) => (byte)(word & OffsetMask);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    protected BranchOperation(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new OffsetArgument(_memory, _state, GetOffset(word))
        };
    }
}
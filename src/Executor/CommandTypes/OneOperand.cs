using Executor.Memories;
using Executor.States;
using Executor.Arguments;

namespace Executor.CommandTypes;

internal abstract class OneOperand : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_1100_0000;
    private const ushort SourceMask = 0b0000_0000_0011_1000;
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    protected ushort GetRegister(ushort word) => (ushort)(word & RegisterMask);

    protected ushort GetMode(ushort word) => (ushort)((word & SourceMask) >> 3);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    public override IArgument[] GetArguments(ushort word)
    {
        if ((Opcode & 0b1000_0000_0000_0000) > 0)
        {
            return new IArgument[]
            {
            new RegisterByteArgument(_memory, _state, GetMode(word), GetRegister(word))
            };
        }
        return new IArgument[]
        {
            new RegisterWordArgument(_memory, _state, GetMode(word), GetRegister(word))
        };
    }

    protected OneOperand(IMemory memory, IState state) : base(memory, state)
    {
    }
}
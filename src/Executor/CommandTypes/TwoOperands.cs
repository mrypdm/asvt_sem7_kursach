using Executor.Memories;
using Executor.States;

namespace Executor.CommandTypes;

public abstract class TwoOperands : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_0000_0000_0000;
    private const ushort SourceMask1 = 0b0000_1110_0000_0000;
    private const ushort SourceMask2 = 0b0000_0000_0011_1000;
    private const ushort RegisterMask1 = 0b0000_0001_1100_0000;
    private const ushort RegisterMask2 = 0b0000_0000_0000_0111;

    protected ushort GetRegister1(ushort word) => (ushort)((word & RegisterMask1) >> 6);

    protected ushort GetRegister2(ushort word) => (ushort)(word & RegisterMask2);

    protected ushort GetMode2(ushort word) => (ushort)((word & SourceMask2) >> 3);

    protected ushort GetMode1(ushort word) => (ushort)((word & SourceMask1) >> 9);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    public override IArgument[] GetArguments(ushort word)
    {
        if (Opcode & 0b1000_0000_0000_0000 > 0)
        {
            return new IArgument[]
            {
            new RegisterByteArgument(Memory, State, GetMode1(word), GetRegister1(word)),
            new RegisterByteArgument(Memory, State, GetMode2(word), GetRegister2(word))
            };
        }
        return new IArgument[]
        {
            new RegisterWordArgument(Memory, State, GetMode1(word), GetRegister1(word)),
            new RegisterWordArgument(Memory, State, GetMode2(word), GetRegister2(word))
        };
    }

    protected TwoOperands(IMemory memory, IState state) : base(memory, state)
    {
    }
}
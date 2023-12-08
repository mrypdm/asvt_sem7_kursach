using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

internal class JSR : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1110_0000_0000;
    private const ushort Register1Mask = 0b0000_0001_1100_0000;
    private const ushort ModeMask = 0b0000_0000_0011_1000;
    private const ushort Register2Mask = 0b0000_0000_0000_0111;

    public JSR(IMemory memory, IState state) : base(memory, state)
    {
    }

    protected ushort GetRegister1(ushort word) => (ushort)((word & Register1Mask) >> 6);

    protected ushort GetRegister2(ushort word) => (ushort)(word & Register2Mask);

    protected ushort GetMode(ushort word) => (ushort)(word & ModeMask);

    public ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new JSRnBITArg(_memory, _state, GetRegister1(word), GetMode(word), GetRegister2(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("004000", 8);
}
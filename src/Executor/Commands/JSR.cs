using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands;

public class JSR : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1110_0000_0000;
    private const ushort Register1Mask = 0b0000_0001_1100_0000;
    private const ushort ModeMask = 0b0000_0000_0011_1000;
    private const ushort Register2Mask = 0b0000_0000_0000_0111;

    public JSR(IStorage storage, IState state) : base(storage, state)
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
            new JSRnBITArg(Storage, _state, GetRegister1(word), GetMode(word), GetRegister2(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
        // throw if addressing mode is 0
    }

    public override ushort Opcode => Convert.ToUInt16("004000", 8);
}
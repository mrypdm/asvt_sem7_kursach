using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands;

public class SOB : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1110_0000_0000;
    private const ushort RegisterMask = 0b0000_0001_1100_0000;
    private const ushort OffsetMask = 0b0000_0000_0011_1111;

    public SOB(IStorage storage, IState state) : base(storage, state)
    {
    }

    protected ushort GetRegister(ushort word) => (ushort)((word & RegisterMask) >> 6);

    protected byte GetOffset(ushort word) => (byte)(word & OffsetMask);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    public override IArgument[] GetArguments(ushort word) => new IArgument[]
        { new SobArgument(Storage, State, GetRegister(word), GetOffset(word)) };

    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<SobArgument>(arguments[0]);

        var newValue = --State.Registers[validatedArgument.Register];

        if (newValue != 0)
        {
            State.Registers[7] -= (ushort)(2 * validatedArgument.Offset);
        }
    }

    public override ushort Opcode => Convert.ToUInt16("077000", 8);
}
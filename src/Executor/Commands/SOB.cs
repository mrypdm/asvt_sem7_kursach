using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands;

public sealed class SOB : BaseCommand
{
    private const ushort RegisterMask = 0b0000_0001_1100_0000;
    private const ushort OffsetMask = 0b0000_0000_0011_1111;

    private static ushort GetRegister(ushort word) => (ushort)((word & RegisterMask) >> 6);

    private static byte GetOffset(ushort word) => (byte)(word & OffsetMask);

    public SOB(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => new IArgument[]
        { new SobArgument(Storage, State, GetRegister(word), GetOffset(word)) };

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("077000", 8);
}
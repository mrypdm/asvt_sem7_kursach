using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands;

public sealed class RTS : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_1111_1000;
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    private static ushort GetRegister(ushort word) => (ushort)(word & RegisterMask);

    public RTS(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) =>
        new IArgument[] { new RegisterWordArgument(Storage, State, 0, GetRegister(word)) };

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var argument = ValidateArgument<RegisterWordArgument>(arguments[0]);

        State.Registers[7] = State.Registers[argument.Register];
        State.Registers[argument.Register] = Storage.PopFromStack(State);
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("000200", 8);
}
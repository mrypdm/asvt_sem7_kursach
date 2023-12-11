using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands;

public class RTS : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_1111_1000;
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    public RTS(IStorage storage, IState state) : base(storage, state)
    {
    }

    protected ushort GetRegister(ushort word) => (ushort)(word & RegisterMask);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    public override IArgument[] GetArguments(ushort word) =>
        new IArgument[] { new RegisterWordArgument(Storage, State, 0,GetRegister(word)) };

    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var argument = ValidateArgument<RegisterWordArgument>(arguments[0]);

        State.Registers[7] = State.Registers[argument.Register];
        State.Registers[argument.Register] = Storage.PopFromStack(State);
    }

    public override ushort Opcode => Convert.ToUInt16("000200", 8);
}
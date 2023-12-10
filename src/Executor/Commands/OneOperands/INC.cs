using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class INC : OneOperand
{
    public INC(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<ushort>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();

        var value = (byte)(source() + 1);

        destination(value);
        State.Z = value == 0;
        State.N = (value & 0b1000_0000_0000_0000) > 0;
        State.V = value == Convert.ToUInt16("077777", 8);
    }

    public override ushort Opcode => Convert.ToUInt16("005200", 8);
}
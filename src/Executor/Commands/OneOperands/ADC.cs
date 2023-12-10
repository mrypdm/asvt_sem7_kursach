using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class ADC : OneOperand
{
    public ADC(IStorage storage, IState state) : base(storage, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<ushort>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();

        var delta = State.C ? 1 : 0;
        var oldValue = source();
        var value = (byte)(oldValue + delta);

        destination(value);
        State.Z = value == 0;
        State.N = (value & 0b1000_0000_0000_0000) != 0;
        State.V = oldValue == Convert.ToUInt16("077777", 8) && delta == 1;
        State.C = oldValue == Convert.ToUInt16("177777", 8) && delta == 1;
    }

    public override ushort Opcode => Convert.ToUInt16("005500", 8);
}
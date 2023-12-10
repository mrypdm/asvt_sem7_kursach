using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class ADCB : OneOperand
{
    public ADCB(IStorage storage, IState state) : base(storage, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments[0]);
        var (source, destination) = validatedArgument.GetSourceAndDestination();
        
        var delta = State.C ? 1 : 0;
        var oldValue = source();
        var value = (byte)(oldValue + delta);

        destination(value);
        State.Z = value == 0;
        // TODO byte?
        State.N = (value & 0b1000_0000) != 0;
        State.V = oldValue == 0b111_1111 && delta == 1;
        State.C = oldValue == 0b1111_1111 && delta == 1;
    }

    public override ushort Opcode => Convert.ToUInt16("105500", 8);
}
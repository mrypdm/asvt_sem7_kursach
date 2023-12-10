using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class SDCB : OneOperand
{
    public SDCB(IStorage storage, IState state) : base(storage, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<byte>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();

        var delta = State.C ? 1 : 0;
        var value = (byte)(source() - delta);

        destination(value);
        State.Z = value == 0;
        // TODO byte?
        State.N = (value & 0b1000_0000) > 0;
        State.V = value == 0b1000_0000;
        State.C = value == 0 && delta == 1;
    }

    public override ushort Opcode => Convert.ToUInt16("105600", 8);
}
using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
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
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments[0]);

        var delta = State.C ? 1 : 0;
        var oldValue = validatedArgument.Value;
        var value = (byte)(oldValue - delta);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = oldValue == 0x80; // && delta == 1 ?
        State.C = !(oldValue == 0 && delta == 1); // cleared if (dst) was 0 and C was 1; set otherwise
    }

    public override ushort Opcode => Convert.ToUInt16("105600", 8);
}
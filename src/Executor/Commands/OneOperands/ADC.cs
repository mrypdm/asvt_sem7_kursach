using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
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
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments[0]);

        var delta = State.C ? 1 : 0;
        var oldValue = validatedArgument.Value;
        var value = (ushort)(oldValue + delta);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = oldValue == Convert.ToUInt16("077777", 8) && delta == 1;
        State.C = oldValue == Convert.ToUInt16("177777", 8) && delta == 1;
    }

    public override ushort Opcode => Convert.ToUInt16("005500", 8);
}
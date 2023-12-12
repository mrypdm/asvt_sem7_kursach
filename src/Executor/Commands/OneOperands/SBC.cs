using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class SDC : OneOperand
{
    public SDC(IStorage storage, IState state) : base(storage, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments[0]);

        var delta = State.C ? 1 : 0;
        var value = (ushort)(validatedArgument.Value - delta);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = value == 0b1000_0000_0000_0000;
        State.C = value == 0 && delta == 1;
    }

    public override ushort Opcode => Convert.ToUInt16("005600", 8);
}
using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class DEC : OneOperand
{
    public DEC(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments[0]);
        
        var value = (ushort)(validatedArgument.Value - 1);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = (value & 0b1000_0000_0000_0000) > 0;
        State.V = value == Convert.ToUInt16("100000", 8);
    }

    public override ushort Opcode => Convert.ToUInt16("005300", 8);
}
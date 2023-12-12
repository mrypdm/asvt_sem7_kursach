using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class INCB : OneOperand
{
    public INCB(IStorage storage, IState state) : base(storage, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments[0]);

        var oldValue = validatedArgument.Value;
        var value = (byte)(oldValue + 1);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = oldValue == 0x7F;
    }

    public override ushort Opcode => Convert.ToUInt16("105200", 8);
}
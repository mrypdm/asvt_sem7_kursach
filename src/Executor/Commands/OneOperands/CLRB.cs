using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class CLRB : OneOperand
{
    public CLRB(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments[0]);
        
        validatedArgument.Value = 0;
        State.Z = true;
        State.V = false;
        State.C = false;
        State.N = false;
    }

    public override ushort Opcode => Convert.ToUInt16("105000", 8);
}
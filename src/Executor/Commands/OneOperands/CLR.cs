using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class CLR : OneOperand
{
    public CLR(IStorage storage, IState state) : base(storage, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<ushort>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();
        
        destination(0);
        State.Z = true;
        State.V = false;
        State.C = false;
        State.N = false;
    }

    public override ushort Opcode => Convert.ToUInt16("005000", 8);
}
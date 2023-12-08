using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class CLR : OneOperand
{
    public CLR(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<ushort>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();
        
        destination(0);
        _state.Z = true;
        _state.V = false;
        _state.C = false;
        _state.N = false;
    }

    public override ushort Opcode => Convert.ToUInt16("005000", 8);
}
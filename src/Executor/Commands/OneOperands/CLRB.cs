using System;
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
        var validatedArgument = ValidateArgument<IRegisterArgument<byte>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();
        
        destination(0);
        _state.Z = true;
        _state.V = false;
        _state.C = false;
        _state.N = false;
    }

    public override ushort Opcode => Convert.ToUInt16("105000", 8);
}
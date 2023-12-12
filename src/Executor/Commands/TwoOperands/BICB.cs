using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public class BICB : TwoOperand
{
    public BICB(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArguments = ValidateArguments<RegisterByteArgument>(arguments);
        
        var value = (byte)(~validatedArguments[0].Value & validatedArguments[1].Value);
        
        validatedArguments[1].Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = false;
    }

    public override ushort Opcode => Convert.ToUInt16("140000", 8);
}
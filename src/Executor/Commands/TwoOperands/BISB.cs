using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public class BISB : TwoOperand
{
    public BISB(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArguments = ValidateArguments<RegisterByteArgument>(arguments);

        var value = (byte)(validatedArguments[0].Value | validatedArguments[1].Value);

        validatedArguments[1].SetValue(value);
        State.Z = value == 0;
        State.N = (value & 0b1000_0000) > 0;
        State.V = false;
    }

    public override ushort Opcode => Convert.ToUInt16("150000", 8);
}
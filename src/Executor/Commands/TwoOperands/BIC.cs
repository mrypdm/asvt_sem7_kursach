using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public class BIC : TwoOperand
{
    public BIC(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArguments = ValidateArguments<RegisterWordArgument>(arguments);

        var value = (ushort)(validatedArguments[1].Value & ~validatedArguments[0].Value);

        validatedArguments[1].Value = value;
        State.Z = value == 0;
        State.N = (value & 0b1000_0000_0000_0000) != 0;
        State.V = false;
    }

    public override ushort Opcode => Convert.ToUInt16("040000", 8);
}
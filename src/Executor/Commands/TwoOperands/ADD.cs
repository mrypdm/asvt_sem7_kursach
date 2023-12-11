using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public class ADD : TwoOperand
{
    public ADD(IStorage storage, IState state) : base(storage, state)
    {
    }


    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new RegisterWordArgument(Storage, State, GetMode1(word), GetRegister1(word)),
            new RegisterWordArgument(Storage, State, GetMode2(word), GetRegister2(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArguments = ValidateArguments<RegisterWordArgument>(arguments);

        var value0 = validatedArguments[0].Value;
        var value1 = validatedArguments[1].Value;

        var carry = value1 + value0 > 0b1111111111111111;
        var sign = ((value1 ^ value0) & 0b1000_0000_0000_0000) == 0;

        var value = (ushort)(value1 + value0);

        validatedArguments[1].Value = value;
        State.Z = value == 0;
        State.N = (value & 0b1000_0000_0000_0000) > 0;
        State.V = sign && State.N != (value & 0b1000_0000_0000_0000) < 0;
        State.C = carry;
    }

    public override ushort Opcode => Convert.ToUInt16("060000", 8);
}
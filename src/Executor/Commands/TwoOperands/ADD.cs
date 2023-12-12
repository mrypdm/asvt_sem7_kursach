using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
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
        var (src, dst) = ValidateArguments<RegisterWordArgument>(arguments);

        var value0 = src.Value;
        var value1 = dst.Value;

        var carry = value1 + value0 > 0xFFFF;
        var sameSign = ((value1 ^ value0) & 0x8000) == 0;

        var value = (ushort)(value1 + value0);

        dst.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = sameSign && State.N != ((value0 & 0x8000) != 0);
        State.C = carry;
    }

    public override ushort Opcode => Convert.ToUInt16("060000", 8);
}
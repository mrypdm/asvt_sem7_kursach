using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public class SUB : TwoOperand
{
    public SUB(IStorage storage, IState state) : base(storage, state)
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

        var value = (ushort)(value1 - value0);

        var carry = (uint)(value1 - value0) > 0b1111_1111_1111_1111;
        var sign = ((value1 ^ value0) & 0b1000_0000_0000_0000) != 0;

        dst.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = sign && State.N != (value & 0b1000_0000_0000_0000) < 0;
        // TODO carry
    }

    public override ushort Opcode => Convert.ToUInt16("160000", 8);
}
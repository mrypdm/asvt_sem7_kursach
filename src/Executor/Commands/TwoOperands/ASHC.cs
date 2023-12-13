using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public class ASHC : TwoOperand
{
    public ASHC(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => new IArgument[]
    {
        new RegisterWordArgument(Storage, State, 0, GetRegister1(word)),
        new RegisterWordArgument(Storage, State, GetMode2(word), GetRegister2(word))
    };

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var (reg, src) = ValidateArguments<RegisterWordArgument>(arguments);

        if (reg.Mode != 0)
        {
            throw new ArgumentException("REG argument of ASH must be addressing with mode 0");
        }

        var shift = (byte)(src.Value & 0b11_1111);
        var isNegative = (shift & 0b10_0000) != 0;

        if (isNegative)
        {
            shift = (byte)((~shift + 1) & 0b11_1111);
        }

        var value = (uint)((State.Registers[reg.Register] << 16) | State.Registers[reg.Register | 1]);
        var bit = value & 0x80000000;

        var newC = State.C;
        while (shift-- != 0)
        {
            if (isNegative) // shift right
            {
                newC = value % 2 == 1;
                value >>= 1;
                value |= bit;
            }
            else // shift left
            {
                newC = value.IsNegative();
                value <<= 1;
            }
        }

        State.Registers[reg.Register] = (ushort)((value & 0xFFFF0000) >> 16);
        State.Registers[reg.Register | 1] = (ushort)(value & 0xFFFF);

        State.Z = value == 0;
        State.N = value.IsNegative();
        State.C = newC;
        State.V = bit.IsNegative() != value.IsNegative();
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("072000", 8);
}
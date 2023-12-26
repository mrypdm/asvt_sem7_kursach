using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public class ASH : TwoOperand
{
    public ASH(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => new IArgument[]
    {
        new RegisterWordArgument(Storage, State, 0, GetLeftArgumentRegister(word)),
        new RegisterWordArgument(Storage, State, GetRightArgumentAddressingMode(word), GetRightArgumentRegister(word))
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
        var value = reg.Value;
        var bit = (ushort)(value & 0x8000);

        if (isNegative)
        {
            shift = (byte)((~shift + 1) & 0b11_1111);
        }

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

        reg.Value = value;

        State.Z = value == 0;
        State.N = value.IsNegative();
        State.C = newC;
        State.V = bit.IsNegative() != value.IsNegative();
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("072000", 8);
}
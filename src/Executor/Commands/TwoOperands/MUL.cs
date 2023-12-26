using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

/// <summary>
/// Multiplication of words
/// </summary>
public class MUL : TwoOperand
{
    public MUL(IStorage storage, IState state) : base(storage, state)
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
            throw new ArgumentException("REG argument of MUL must be addressing with mode 0");
        }

        var value = reg.Value * src.Value;
        var high = (ushort)((value & 0xFFFF0000) >> 16);
        var low = (ushort)(value & 0xFFFF);

        State.Registers[reg.Register] = high;
        State.Registers[reg.Register | 1] = low;

        State.Z = value == 0;
        State.V = false;
        State.N = value < 0;
        State.C = value is < -(1 << 15) or >= (1 << 15) - 1;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("070000", 8);
}
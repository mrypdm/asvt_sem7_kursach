using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public sealed class SUB : TwoOperand
{
    public SUB(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new RegisterWordArgument(Storage, State, GetLeftArgumentAddressingMode(word), GetLeftArgumentRegister(word)),
            new RegisterWordArgument(Storage, State, GetRightArgumentAddressingMode(word), GetRightArgumentRegister(word))
        };
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var (src, dst) = ValidateArguments<RegisterWordArgument>(arguments);

        var value0 = src.Value;
        var value1 = dst.Value;

        var value = (ushort)(value1 - value0);

        dst.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = !value0.IsSameSignWith(value1) && value0.IsSameSignWith(value);
        State.C = (uint)(value1 - value0) > 0xFFFF;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("160000", 8);
}
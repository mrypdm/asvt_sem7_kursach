using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public sealed class CMP : TwoOperand
{
    public CMP(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var (src, dst) = ValidateArguments<RegisterWordArgument>(arguments);

        var value0 = src.Value;
        var value1 = dst.Value;

        var value = (ushort)(value1 - value0);

        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = !value0.IsSameSignWith(value1) && value1.IsSameSignWith(value);
        State.C = (uint)(value0 - value1) > 0xFFFF;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("020000", 8);
}
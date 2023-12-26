using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

/// <summary>
/// Compare bytes
/// </summary>
public sealed class CMPB : TwoOperand
{
    public CMPB(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var (src, dst) = ValidateArguments<RegisterByteArgument>(arguments);

        var value0 = src.Value;
        var value1 = dst.Value;

        var value = (byte)(value1 - value0);

        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = !value0.IsSameSignWith(value1) && value1.IsSameSignWith(value);
        State.C = (uint)(value0 - value1) > 0xFF;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("120000", 8);
}
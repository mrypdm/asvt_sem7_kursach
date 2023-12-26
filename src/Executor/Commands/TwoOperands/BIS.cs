using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

/// <summary>
/// Bit set of word
/// </summary>
public sealed class BIS : TwoOperand
{
    public BIS(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var (src, dst) = ValidateArguments<RegisterWordArgument>(arguments);

        var value = (ushort)(src.Value | dst.Value);

        dst.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = false;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("050000", 8);
}
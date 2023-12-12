using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public sealed class MOVB : TwoOperand
{
    public MOVB(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var (src, dst) = ValidateArguments<RegisterByteArgument>(arguments);

        var value = src.Value;

        if (dst.Mode == 0)
        {
            // propagate the sign bit
            var high = value.IsNegative() ? 0xFF : 0;
            State.Registers[dst.Register] = (ushort)((high << 8) | value);
        }
        else
        {
            dst.Value = value;
        }

        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = false;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("110000", 8);
}
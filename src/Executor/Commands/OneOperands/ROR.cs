using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

/// <summary>
/// Circular rotate word right
/// </summary>
public sealed class ROR : OneOperand
{
    public ROR(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments); 

        var value = validatedArgument.Value;
        var newC = value % 2 == 1;
        var oldC = State.C ? 1 : 0;

        value >>= 1;
        value |= (ushort)(oldC << 15);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.C = newC;
        State.V = State.N ^ State.C;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("006000", 8);
}
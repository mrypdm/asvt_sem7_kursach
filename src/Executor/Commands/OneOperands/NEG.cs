using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

/// <summary>
/// Negate word
/// </summary>
public sealed class NEG : OneOperand
{
    public NEG(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments); 

        var value = (ushort)-validatedArgument.Value;

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = value == 0x8000;
        State.C = value != 0;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("005400", 8);
}
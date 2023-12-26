using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

/// <summary>
/// Test byte
/// </summary>
public sealed class TSTB : OneOperand
{
    public TSTB(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments); 

        var value = validatedArgument.Value;

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = false;
        State.C = false;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("105700", 8);
}
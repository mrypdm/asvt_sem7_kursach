using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

/// <summary>
/// Add C to word
/// </summary>
public sealed class ADC : OneOperand
{
    public ADC(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments); 

        var delta = State.C ? 1 : 0;
        var oldValue = validatedArgument.Value;
        var value = (ushort)(oldValue + delta);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = oldValue == Convert.ToUInt16("077777", 8) && delta == 1;
        State.C = oldValue == Convert.ToUInt16("177777", 8) && delta == 1;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("005500", 8);
}
using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public sealed class ADCB : OneOperand
{
    public ADCB(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments[0]);

        var delta = State.C ? 1 : 0;
        var oldValue = validatedArgument.Value;
        var value = (byte)(oldValue + delta);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = oldValue == 0x7F && delta == 1;
        State.C = oldValue == 0xFF && delta == 1;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("105500", 8);
}
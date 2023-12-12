using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public sealed class DEC : OneOperand
{
    public DEC(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments[0]);

        var oldValue = validatedArgument.Value;
        var value = (ushort)(oldValue - 1);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = oldValue == Convert.ToUInt16("100000", 8);
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("005300", 8);
}
using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public sealed class SXT : OneOperand
{
    public SXT(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments[0]);

        var value = State.N ? 0xFFFF : 0;

        validatedArgument.Value = (ushort)value;
        State.Z = value == 0;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("006700", 8);
}
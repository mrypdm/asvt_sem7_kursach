using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public sealed class CLR : OneOperand
{
    public CLR(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments[0]);

        validatedArgument.Value = 0;
        State.Z = true;
        State.V = false;
        State.C = false;
        State.N = false;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("005000", 8);
}
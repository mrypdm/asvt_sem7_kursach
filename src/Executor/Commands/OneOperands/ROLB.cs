using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public sealed class ROLB : OneOperand
{
    public ROLB(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments[0]);

        var value = validatedArgument.Value;
        var newC = value.IsNegative();
        var oldC = (byte)(State.C ? 1 : 0);

        value <<= 1;
        value |= oldC;

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.C = newC;
        State.V = State.N ^ State.C;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("106100", 8);
}
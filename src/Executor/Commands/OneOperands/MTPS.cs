using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public sealed class MTPS : OneOperand
{
    public MTPS(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments[0]);

        var value = validatedArgument.Value;

        // this instruction cannot set the T bit, but it does not say about clearing
        // for now we will completely prohibit changing the T bit
        value &= 0b1110_1111; // clear T bit
        value |= (byte)((State.T ? 1 : 0) << 4); // set original T

        State.ProcessorStateWord = value;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("106400", 8);
}
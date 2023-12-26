using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

/// <summary>
/// Arithmetic shift right of byte
/// </summary>
public sealed class ASRB : OneOperand
{
    public ASRB(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments); 

        var value = validatedArgument.Value;
        var newC = value % 2 == 1;
        var highBit = value.IsNegative() ? 1 : 0;

        value >>= 1;
        value |= (byte)(highBit << 7);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.C = newC;
        State.V = State.N ^ State.C;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("106200", 8);
}
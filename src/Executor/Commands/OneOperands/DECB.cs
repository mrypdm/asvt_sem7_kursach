using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

/// <summary>
/// Decrement byte
/// </summary>
public sealed class DECB : OneOperand
{
    public DECB(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments); 
        
        var oldValue = validatedArgument.Value;
        var value = (byte)(oldValue - 1);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = oldValue == 0x80;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("105300", 8);
}
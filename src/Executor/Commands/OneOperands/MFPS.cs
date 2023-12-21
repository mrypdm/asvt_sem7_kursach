using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public sealed class MFPS : OneOperand
{
    public MFPS(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterByteArgument>(arguments); 

        var value = (byte)State.ProcessorStateWord;

        if (validatedArgument.Mode == 0)
        {
            // propagate the sign bit
            var high = value.IsNegative() ? 0xFF : 0;
            State.Registers[validatedArgument.Register] = (ushort)((high << 8) | value);
        }
        else
        {
            validatedArgument.Value = value;
        }

        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = false;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("106700", 8);
}
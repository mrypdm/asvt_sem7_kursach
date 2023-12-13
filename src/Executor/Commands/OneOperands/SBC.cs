using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public sealed class SBC : OneOperand
{
    public SBC(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments); 

        var delta = State.C ? 1 : 0;
        var oldValue = validatedArgument.Value;
        var value = (ushort)(oldValue - delta);

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = oldValue == 0x8000; // && delta == 1 ?
        State.C = !(oldValue == 0 && delta == 1); // cleared if (dst) was 0 and C was 1; set otherwise
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("005600", 8);
}
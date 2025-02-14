﻿using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

/// <summary>
/// Swap bytes in word
/// </summary>
public sealed class SWAB : OneOperand
{
    public SWAB(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments); 

        var value = validatedArgument.Value;
        var low = (byte)(value & 0xFF);
        var high = (byte)((value & 0xFF00) >> 8);
        value = (ushort)((low << 8) | high);

        validatedArgument.Value = value;
        
        // If I understand correctly, then we set the codes based on the low byte of the result,
        // that is, according to the high byte of the source
        State.Z = high == 0;
        State.N = high.IsNegative();
        State.V = false;
        State.C = false;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("000300", 8);
}
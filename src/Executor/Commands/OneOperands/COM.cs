﻿using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

/// <summary>
/// Complement word
/// </summary>
public sealed class COM : OneOperand
{
    public COM(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments); 

        var value = (ushort)~validatedArgument.Value;

        validatedArgument.Value = value;
        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = false;
        State.C = true;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("005100", 8);
}
﻿using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

/// <summary>
/// Division of words
/// </summary>
public class DIV : TwoOperand
{
    public DIV(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => new IArgument[]
    {
        new RegisterWordArgument(Storage, State, 0, GetLeftArgumentRegister(word)),
        new RegisterWordArgument(Storage, State, GetRightArgumentAddressingMode(word), GetRightArgumentRegister(word))
    };

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var (reg, src) = ValidateArguments<RegisterWordArgument>(arguments);
        
        if (reg.Mode != 0)
        {
            throw new ArgumentException("REG argument of DIV must be addressing with mode 0");
        }

        if (reg.Register % 2 != 0)
        {
            throw new InvalidInstructionException("DIV must be using register with even number");
        }

        var srcValue = src.Value;

        if (State.Registers[reg.Register] > srcValue || srcValue == 0)
        {
            State.V = true;
            return;
        }

        var number = (State.Registers[reg.Register] << 16) | State.Registers[reg.Register + 1];

        var quot = number / srcValue;
        var rem = number % srcValue;

        State.Registers[reg.Register] = (ushort)quot;
        State.Registers[reg.Register | 1] = (ushort)rem;

        State.Z = quot == 0;
        State.N = quot < 0;
        State.V = false;
        State.C = number == 0;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("071000", 8);
}
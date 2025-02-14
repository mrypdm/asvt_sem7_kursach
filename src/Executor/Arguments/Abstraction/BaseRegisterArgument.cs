﻿using System;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments.Abstraction;

/// <inheritdoc cref="IRegisterArgument{TValue}"/>
public abstract class BaseRegisterArgument<TValue> : IRegisterArgument<TValue>
{
    private readonly Lazy<ushort?> _address;

    protected BaseRegisterArgument(IStorage storage, IState state, ushort mode, ushort register)
    {
        Storage = storage;
        State = state;
        Mode = mode;
        Register = register;
        _address = new Lazy<ushort?>(InitAddress);
    }

    /// <inheritdoc />
    public object GetValue() => Value;

    /// <inheritdoc />
    public void SetValue(object obj) => Value = (TValue)obj;

    /// <inheritdoc />
    public ushort Register { get; }

    /// <inheritdoc />
    public ushort Mode { get; }

    /// <inheritdoc />
    public abstract TValue Value { get; set; }

    /// <inheritdoc />
    public ushort? Address => _address.Value;

    protected abstract ushort Delta { get; }
    
    /// <summary>
    /// Storage
    /// </summary>
    protected IStorage Storage { get; }

    /// <summary>
    /// State
    /// </summary>
    protected IState State { get; }

    private ushort? InitAddress()
    {
        ushort offset;
        ushort address;

        switch (Mode)
        {
            case 0:
                return null;
            case 1:
                return State.Registers[Register];
            case 2:
                address = State.Registers[Register];
                State.Registers[Register] += Delta;
                return address;
            case 3:
                address = Storage.GetWord(State.Registers[Register]);
                State.Registers[Register] += 2;
                return address;
            case 4:
                State.Registers[Register] -= Delta;
                return State.Registers[Register];
            case 5:
                State.Registers[Register] -= 2;
                return Storage.GetWord(State.Registers[Register]);
            case 6:
                offset = Storage.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                return (ushort)(State.Registers[Register] + offset);
            case 7:
                offset = Storage.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                return Storage.GetWord((ushort)(State.Registers[Register] + offset));
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }
    }
}
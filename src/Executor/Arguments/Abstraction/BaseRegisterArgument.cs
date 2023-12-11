using System;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments.Abstraction;

public abstract class BaseRegisterArgument<TValue> : BaseArgument, IRegisterArgument<TValue>
{
    private readonly Lazy<ushort?> _address;

    protected BaseRegisterArgument(IStorage storage, IState state, ushort mode, ushort register) : base(storage, state)
    {
        Mode = mode;
        Register = register;
        _address = new Lazy<ushort?>(InitAddress);
    }

    public override object GetValue() => Value;

    public override void SetValue(object obj) => Value = (TValue)obj;

    public ushort Register { get; }

    public ushort Mode { get; }

    public abstract TValue Value { get; set; }

    public ushort? Address => _address.Value;
    
    protected abstract ushort Delta { get; }

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
                State.Registers[Register] += Delta;
                return address;
            case 4:
                State.Registers[Register] -= Delta;
                return State.Registers[Register];
            case 5:
                State.Registers[Register] -= Delta;
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
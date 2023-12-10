using System;
using Executor.Arguments.Abstraction;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

/// <summary>
/// Argument referencing a word
/// </summary>
public class RegisterWordArgument : BaseArgument, IRegisterArgument<ushort>
{
    public RegisterWordArgument(IStorage storage, IState state, ushort mode, ushort register) : base(storage, state)
    {
        Register = register;
        Mode = mode;
    }

    /// <inheritdoc />
    public ushort Mode { get; }

    /// <inheritdoc />
    public ushort Register { get; }

    /// <inheritdoc />
    public override object GetValue() => GetSourceAndDestination();

    /// <inheritdoc />
    public override void SetValue(object obj) =>
        throw new InvalidOperationException($"{GetType().Name} does not support SetValue");

    /// <inheritdoc />
    public (Func<ushort>, Action<ushort>) GetSourceAndDestination()
    {
        ushort addr;
        ushort offset;

        Func<ushort> src;
        Action<ushort> dst;

        switch (Mode)
        {
            case 0:
                src = () => State.Registers[Register];
                dst = value => State.Registers[Register] = value;
                break;
            case 1:
                src = () => Storage.GetWord(State.Registers[Register]);
                dst = value => Storage.SetWord(State.Registers[Register], value);
                break;
            case 2:
                addr = State.Registers[Register];
                src = () => Storage.GetWord(addr);
                dst = value => Storage.SetWord(addr, value);
                State.Registers[Register] += 2;
                break;
            case 3:
                addr = Storage.GetWord(State.Registers[Register]);
                src = () => Storage.GetWord(addr);
                dst = value => Storage.SetWord(addr, value);
                State.Registers[Register] += 2;
                break;
            case 4:
                State.Registers[Register] -= 2;
                addr = State.Registers[Register];
                src = () => Storage.GetWord(addr);
                dst = value => Storage.SetWord(addr, value);
                break;
            case 5:
                State.Registers[Register] -= 2;
                addr = Storage.GetWord(State.Registers[Register]);
                src = () => Storage.GetWord(addr);
                dst = value => Storage.SetWord(addr, value);
                break;
            case 6:
                offset = Storage.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = (ushort)(State.Registers[Register] + offset);
                src = () => Storage.GetWord(addr);
                dst = value => Storage.SetWord(addr, value);
                break;
            case 7:
                offset = Storage.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = Storage.GetWord((ushort)(State.Registers[Register] + offset));
                src = () => Storage.GetWord(addr);
                dst = value => Storage.SetWord(addr, value);
                break;
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }

        return (src, dst);
    }
}
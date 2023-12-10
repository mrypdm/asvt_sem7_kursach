using System;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

/// <summary>
/// Argument referencing the address of the word
/// </summary>
public class RegisterAddressArgument : BaseArgument, IRegisterArgument<ushort>
{
    public RegisterAddressArgument(IStorage storage, IState state, ushort mode, ushort register)
        : base(storage, state)
    {
        Register = register;
        Mode = mode;
    }

    /// <inheritdoc />
    public override object GetValue() => GetSource();

    /// <inheritdoc />
    public override void SetValue(object obj) => throw new ReadOnlyArgumentException(GetType());

    /// <inheritdoc />
    public ushort Register { get; }

    /// <inheritdoc />
    public ushort Mode { get; }

    /// <inheritdoc />
    public (Func<ushort> source, Action<ushort> destination) GetSourceAndDestination()
    {
        return (GetSource(), _ => throw new ReadOnlyArgumentException(typeof(RegisterAddressArgument)));
    }

    /// <summary>
    /// Returns getter for argument
    /// </summary>
    /// <exception cref="InvalidInstructionException">If <see cref="Mode"/> is zero</exception>
    /// <exception cref="InvalidOperationException">If <see cref="Mode"/> is larger than 7</exception>
    public Func<ushort> GetSource()
    {
        ushort addr;
        ushort offset;

        switch (Mode)
        {
            case 0:
                throw new InvalidInstructionException("Cannot address to register");
            case 1:
                return () => State.Registers[Register];
            case 2:
                addr = State.Registers[Register];
                State.Registers[Register] += 2;
                return () => addr;
            case 3:
                addr = Storage.GetWord(State.Registers[Register]);
                State.Registers[Register] += 2;
                return () => addr;
            case 4:
                State.Registers[Register] -= 2;
                addr = State.Registers[Register];
                return () => addr;
            case 5:
                State.Registers[Register] -= 2;
                addr = Storage.GetWord(State.Registers[Register]);
                return () => addr;
            case 6:
                offset = Storage.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = (ushort)(State.Registers[Register] + offset);
                return () => addr;
            case 7:
                offset = Storage.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = Storage.GetWord((ushort)(State.Registers[Register] + offset));
                return () => addr;
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }
    }
}
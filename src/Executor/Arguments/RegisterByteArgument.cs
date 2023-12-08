using System;
using Executor.Arguments.Abstraction;
using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

public class RegisterByteArgument : BaseArgument, IRegisterArgument<byte>
{
    public RegisterByteArgument(IMemory memory, IState state, ushort mode, ushort register) : base(memory, state)
    {
        Register = register;
        Mode = mode;
    }

    public ushort Mode { get; }

    public ushort Register { get; }

    private ushort Delta => (ushort)(Register < 6 ? 1 : 2);

    public override object GetValue() => throw new InvalidOperationException();

    public override void SetValue(object obj) => throw new InvalidOperationException();

    public (Func<byte>, Action<byte>) GetSourceAndDestination()
    {
        ushort addr;
        ushort offset;

        Func<byte> src;
        Action<byte> dst;

        switch (Mode)
        {
            case 0:
                src = () => (byte)(State.Registers[Register] & 0xFF);
                dst = value => State.Registers[Register] = (ushort)((State.Registers[Register] & 0xFF00) | value);
                break;
            case 1:
                src = () => Memory.GetByte(State.Registers[Register]);
                dst = value => Memory.SetByte(State.Registers[Register], value);
                break;
            case 2:
                addr = State.Registers[Register];
                src = () => Memory.GetByte(addr);
                dst = value => Memory.SetByte(addr, value);
                State.Registers[Register] += Delta;
                break;
            case 3:
                addr = Memory.GetWord(State.Registers[Register]);
                src = () => Memory.GetByte(addr);
                dst = value => Memory.SetByte(addr, value);
                State.Registers[Register] += Delta;
                break;
            case 4:
                State.Registers[Register] -= Delta;
                addr = State.Registers[Register];
                src = () => Memory.GetByte(addr);
                dst = value => Memory.SetByte(addr, value);
                break;
            case 5:
                State.Registers[Register] -= Delta;
                addr = Memory.GetWord(State.Registers[Register]);
                src = () => Memory.GetByte(addr);
                dst = value => Memory.SetByte(addr, value);
                break;
            case 6:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = (ushort)(State.Registers[Register] + offset);
                src = () => Memory.GetByte(addr);
                dst = value => Memory.SetByte(addr, value);
                break;
            case 7:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = Memory.GetWord((ushort)(State.Registers[Register] + offset));
                src = () => Memory.GetByte(addr);
                dst = value => Memory.SetByte(addr, value);
                break;
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }

        return (src, dst);
    }
}
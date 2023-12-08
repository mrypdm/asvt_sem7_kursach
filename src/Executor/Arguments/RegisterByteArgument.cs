using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

public class RegisterByteArgument : BaseArgument
{
    public RegisterByteArgument(IMemory memory, IState state, ushort mode, ushort register) : base(memory, state)
    {
        Register = register;
        Mode = mode;
    }

    public ushort Mode { get; }

    public ushort Register { get; }

    private ushort Delta => (ushort)(Register < 6 ? 1 : 2);

    public override ushort GetValue()
    {
        ushort addr;
        ushort offset;

        switch (Mode)
        {
            case 0:
                return (ushort)(State.Registers[Register] & 0xFF);
            case 1:
                return Memory.GetByte(State.Registers[Register]);
            case 2:
                var value = Memory.GetByte(State.Registers[Register]);
                State.Registers[Register] += Delta;
                return value;
            case 3:
                addr = Memory.GetWord(State.Registers[Register]);
                State.Registers[Register] += Delta;
                return Memory.GetByte(addr);
            case 4:
                State.Registers[Register] -= Delta;
                return Memory.GetByte(State.Registers[Register]);
            case 5:
                State.Registers[Register] -= Delta;
                addr = Memory.GetWord(State.Registers[Register]);
                return Memory.GetByte(State.Registers[addr]);
            case 6:
                offset = Memory.GetByte(State.Registers[7]);
                State.Registers[7] += 2;
                return Memory.GetByte((ushort)(State.Registers[Register] + offset));
            case 7:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = Memory.GetWord((ushort)(State.Registers[Register] + offset));
                return Memory.GetByte(State.Registers[addr]);
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }
    }

    public override void SetValue(ushort word)
    {
        var value = (byte)word;
        
        ushort addr;
        ushort offset;

        switch (Mode)
        {
            case 0:
                State.Registers[Register] = (ushort)((State.Registers[Register] & 0xFF00) | value);
                break;
            case 1:
                Memory.SetByte(State.Registers[Register], value);
                break;
            case 2:
                Memory.SetByte(State.Registers[Register], value);
                State.Registers[Register] += Delta;
                break;
            case 3:
                addr = Memory.GetWord(State.Registers[Register]);
                Memory.SetByte(addr, value);
                State.Registers[Register] += Delta;
                break;
            case 4:
                State.Registers[Register] -= Delta;
                Memory.SetByte(State.Registers[Register], value);
                break;
            case 5:
                State.Registers[Register] -= Delta;
                addr = Memory.GetWord(State.Registers[Register]);
                Memory.SetByte(State.Registers[addr], value);
                break;
            case 6:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                Memory.SetByte((ushort)(State.Registers[Register] + offset), value);
                break;
            case 7:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = Memory.GetWord((ushort)(State.Registers[Register] + offset));
                Memory.SetByte(State.Registers[addr], value);
                break;
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }
    }
}
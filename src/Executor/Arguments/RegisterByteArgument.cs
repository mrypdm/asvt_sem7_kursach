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

    public override ushort GetValue()
    {
        byte addr;
        byte offset;
        switch (Mode)
        {
            case 0:
                return (byte)State.Registers[Register];
            case 1:
                return Memory.GetByte(State.Registers[Register]);
            case 2:
                var value = Memory.GetByte(State.Registers[Register]);
                State.Registers[Register] += 1;
                return value;
            case 3:
                addr = Memory.GetByte(State.Registers[Register]);
                State.Registers[Register] += 1;
                return Memory.GetByte(addr);
            case 4:
                State.Registers[Register] -= 1;
                return Memory.GetByte(State.Registers[Register]);
            case 5:
                State.Registers[Register] -= 1;
                addr = Memory.GetByte(State.Registers[Register]);
                return Memory.GetByte(State.Registers[addr]);
            case 6:
                State.Registers[7] += 1;
                offset = Memory.GetByte(State.Registers[7]);
                return Memory.GetByte((ushort)(State.Registers[Register] + offset));
            case 7:
                State.Registers[7] += 1;
                offset = Memory.GetByte(State.Registers[7]);
                addr = Memory.GetByte((ushort)(State.Registers[Register] + offset));
                return Memory.GetByte(State.Registers[addr]);
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }
    }

    public override void SetValue(ushort word)
    {
        byte addr;
        byte offset;
        switch (Mode)
        {
            case 0:
                State.Registers[Register] = (byte)word;
                return;
            case 1:
                Memory.SetByte(State.Registers[Register], (byte)word);
                return;
            case 2:
                Memory.SetByte(State.Registers[Register], (byte)word);
                State.Registers[Register] += 1;
                return;
            case 3:
                addr = Memory.GetByte(State.Registers[Register]);
                Memory.SetByte(addr, (byte)word);
                State.Registers[Register] += 1;
                return;
            case 4:
                State.Registers[Register] -= 1;
                Memory.SetByte(State.Registers[Register], (byte)word);
                return;
            case 5:
                State.Registers[Register] -= 1;
                addr = Memory.GetByte(State.Registers[Register]);
                Memory.SetByte(State.Registers[addr], (byte)word);
                return;
            case 6:
                State.Registers[7] += 1;
                offset = Memory.GetByte(State.Registers[7]);
                Memory.SetByte((ushort)(State.Registers[Register] + offset), (byte)word);
                return;
            case 7:
                State.Registers[7] += 1;
                offset = Memory.GetByte(State.Registers[7]);
                addr = Memory.GetByte((ushort)(State.Registers[Register] + offset));
                Memory.SetByte(State.Registers[addr], (byte)word);
                return;
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }
    }
}
using Executor.Arguments.Abstraction;
using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

public class RegisterWordArgument : BaseArgument, IWordRegisterArgument
{
    public RegisterWordArgument(IMemory memory, IState state, ushort mode, ushort register) : base(memory, state)
    {
        Register = register;
        Mode = mode;
    }

    public ushort Mode { get; }

    public ushort Register { get; }

    public override object GetValue() => GetWord();

    public override void SetValue(object obj) => SetWord((ushort)obj);

    public ushort GetWord()
    {
        ushort addr;
        ushort offset;

        switch (Mode)
        {
            case 0:
                return State.Registers[Register];
            case 1:
                return Memory.GetWord(State.Registers[Register]);
            case 2:
                var value = Memory.GetWord(State.Registers[Register]);
                State.Registers[Register] += 2;
                return value;
            case 3:
                addr = Memory.GetWord(State.Registers[Register]);
                State.Registers[Register] += 2;
                return Memory.GetWord(addr);
            case 4:
                State.Registers[Register] -= 2;
                return Memory.GetWord(State.Registers[Register]);
            case 5:
                State.Registers[Register] -= 2;
                addr = Memory.GetWord(State.Registers[Register]);
                return Memory.GetWord(State.Registers[addr]);
            case 6:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                return Memory.GetWord((ushort)(State.Registers[Register] + offset));
            case 7:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = Memory.GetWord((ushort)(State.Registers[Register] + offset));
                return Memory.GetWord(State.Registers[addr]);
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }
    }

    public void SetWord(ushort value)
    {
        ushort addr;
        ushort offset;

        switch (Mode)
        {
            case 0:
                State.Registers[Register] = value;
                break;
            case 1:
                Memory.SetWord(State.Registers[Register], value);
                break;
            case 2:
                Memory.SetWord(State.Registers[Register], value);
                State.Registers[Register] += 2;
                break;
            case 3:
                addr = Memory.GetWord(State.Registers[Register]);
                Memory.SetWord(addr, value);
                State.Registers[Register] += 2;
                break;
            case 4:
                State.Registers[Register] -= 2;
                Memory.SetWord(State.Registers[Register], value);
                break;
            case 5:
                State.Registers[Register] -= 2;
                addr = Memory.GetWord(State.Registers[Register]);
                Memory.SetWord(State.Registers[addr], value);
                break;
            case 6:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                Memory.SetWord((ushort)(State.Registers[Register] + offset), value);
                break;
            case 7:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = Memory.GetWord((ushort)(State.Registers[Register] + offset));
                Memory.SetWord(State.Registers[addr], value);
                break;
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }
    }
}
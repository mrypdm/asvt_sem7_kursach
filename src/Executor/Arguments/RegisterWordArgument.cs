using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

public class RegisterWordArgument : BaseArgument
{
    public RegisterWordArgument(IMemory memory, IState state, ushort mode, ushort register) : base(memory, state)
    {
        Register = register;
        Mode = mode;
    }
    
    public ushort Mode { get; }
    
    public ushort Register { get; }

    public override ushort GetValue()
    {
        ushort word;
        ushort addr;
        ushort offset;
        switch (Mode)
        {
            case 0:
                return State.Registers[Register];
            case 1:
                return Memory.GetWord(State.Registers[Register]);
            case 2:
                word = Memory.GetWord(State.Registers[Register]);
                State.Registers[Register] += 2;
                return word;
            case 3:
                addr = Memory.GetWord(State.Registers[Register]);
                word = Memory.GetWord(addr);
                State.Registers[Register] += 2;
                return word;
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
        }

        throw new InvalidOperationException("Invalid Mode!");
    }

    public override void SetValue(ushort word)
    {
        ushort addr;
        ushort offset;
        switch (Mode)
        {
            case 0:
                State.Registers[Register] = word;
                return;
            case 1:
                Memory.SetWord(State.Registers[Register], word);
                return;
            case 2:
                Memory.SetWord(State.Registers[Register], word);
                State.Registers[Register] += 2;
                return;
            case 3:
                addr = Memory.GetWord(State.Registers[Register]);
                Memory.SetWord(addr, word);
                State.Registers[Register] += 2;
                return;
            case 4:
                State.Registers[Register] -= 2;
                Memory.SetWord(State.Registers[Register], word);
                return;
            case 5:
                State.Registers[Register] -= 2;
                addr = Memory.GetWord(State.Registers[Register]);
                Memory.SetWord(State.Registers[addr], word);
                return;
            case 6:
                State.Registers[7] += 2;
                offset = Memory.GetWord(State.Registers[7]);
                Memory.SetWord((ushort)(State.Registers[Register] + offset), word);
                return;
            case 7:
                State.Registers[7] += 2;
                offset = Memory.GetWord(State.Registers[7]);
                addr = Memory.GetWord((ushort)(State.Registers[Register] + offset));
                Memory.SetWord(State.Registers[addr], word);
                return;
        }

        throw new InvalidOperationException("Invalid Mode!");
    }
}
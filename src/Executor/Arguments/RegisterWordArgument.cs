using Executor.Arguments.Abstraction;
using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

public class RegisterWordArgument : BaseArgument, IRegisterArgument<ushort>
{
    public RegisterWordArgument(IMemory memory, IState state, ushort mode, ushort register) : base(memory, state)
    {
        Register = register;
        Mode = mode;
    }

    public ushort Mode { get; }

    public ushort Register { get; }

    public override object GetValue() => throw new InvalidOperationException();

    public override void SetValue(object obj) => throw new InvalidOperationException();

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
                src = () => Memory.GetWord(State.Registers[Register]);
                dst = value => Memory.SetWord(State.Registers[Register], value);
                break;
            case 2:
                addr = State.Registers[Register];
                src = () => Memory.GetWord(addr);
                dst = value => Memory.SetWord(addr, value);
                State.Registers[Register] += 2;
                break;
            case 3:
                addr = Memory.GetWord(State.Registers[Register]);
                src = () => Memory.GetWord(addr);
                dst = value => Memory.SetWord(addr, value);
                State.Registers[Register] += 2;
                break;
            case 4:
                State.Registers[Register] -= 2;
                addr = State.Registers[Register];
                src = () => Memory.GetWord(addr);
                dst = value => Memory.SetWord(addr, value);
                break;
            case 5:
                State.Registers[Register] -= 2;
                addr = Memory.GetWord(State.Registers[Register]);
                src = () => Memory.GetWord(addr);
                dst = value => Memory.SetWord(addr, value);
                break;
            case 6:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = (ushort)(State.Registers[Register] + offset);
                src = () => Memory.GetWord(addr);
                dst = value => Memory.SetWord(addr, value);
                break;
            case 7:
                offset = Memory.GetWord(State.Registers[7]);
                State.Registers[7] += 2;
                addr = Memory.GetWord((ushort)(State.Registers[Register] + offset));
                src = () => Memory.GetWord(addr);
                dst = value => Memory.SetWord(addr, value);
                break;
            default:
                throw new InvalidOperationException("Invalid addressing mode");
        }

        return (src, dst);
    }
}
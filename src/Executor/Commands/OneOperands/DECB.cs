using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class DECB : OneOperand
{
    public DECB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var value = arguments[0].GetValue();
        arguments[0].SetValue((ushort)(value - 1));
        _state.SetFlag(Flag.Z, arguments[0].GetValue() == 0);
        _state.SetFlag(Flag.N, (arguments[0].GetValue() & 0b1000_0000_0000_0000) > 0);
        _state.SetFlag(Flag.V, arguments[0].GetValue() == Convert.ToUInt16("100000", 8));
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("105300", 8);
}
using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class SDC : OneOperand
{
    public SDC(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var value = arguments[0].GetValue();
        arguments[0].SetValue((ushort)(value - (_state.GetFlag(Flag.C) ? 1 : 0)));
        _state.SetFlag(Flag.Z, arguments[0].GetValue() == 0);
        _state.SetFlag(Flag.N, (arguments[0].GetValue() & 0b1000_0000_0000_0000) > 0);
        _state.SetFlag(Flag.V, arguments[0].GetValue() == 0b1000_0000_0000_0000);
        _state.SetFlag(Flag.C, arguments[0].GetValue() == 0 && _state.GetFlag(Flag.C));
    }

    public override ushort Opcode => Convert.ToUInt16("005600", 8);
}
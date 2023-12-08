using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class ADC : OneOperand
{
    public ADC(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var flag_c = _state.GetFlag(Flag.C) && arguments[0].GetValue() == Convert.ToUInt16("177777", 8);
        var flag_v = _state.GetFlag(Flag.C) && arguments[0].GetValue() == Convert.ToUInt16("077777", 8);
        var value = arguments[0].GetValue();
        arguments[0].SetValue((ushort)(value - (_state.GetFlag(Flag.C) ? 1 : 0)));
        _state.SetFlag(Flag.Z, arguments[0].GetValue() == 0);
        _state.SetFlag(Flag.N, (arguments[0].GetValue() & 0b1000_0000_0000_0000) > 0);
        _state.SetFlag(Flag.V, flag_v);
        _state.SetFlag(Flag.C, flag_c);
    }

    public override ushort Opcode => Convert.ToUInt16("005500", 8);
}
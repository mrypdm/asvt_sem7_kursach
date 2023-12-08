using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class BICB : TwoOperands
{
    public BICB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        arguments[1].SetValue((ushort)(arguments[1].GetValue() & (ushort)(~arguments[0].GetValue())));
        _state.SetFlag(Flag.Z, arguments[1].GetValue() == 0);
        _state.SetFlag(Flag.N, (arguments[1].GetValue() & 0b1000_0000_0000_0000) > 0);
        _state.SetFlag(Flag.V, false);
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("140000", 8);
}
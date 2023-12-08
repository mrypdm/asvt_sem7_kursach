using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class CLR : OneOperand
{
    public CLR(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        arguments[0].SetValue(0);
        _state.SetFlag(Flag.Z, true);
        _state.SetFlag(Flag.V, false);
        _state.SetFlag(Flag.C, false);
        _state.SetFlag(Flag.N, false);
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("005000", 8);
}
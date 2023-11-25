using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class BEQ : OneOperand
{
    public BEQ(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        if (!_state.GetFlag(Flag.Z))
        {
            _state.R[7] = ushort(_state.R[7] + 2 * arguments[0].GetValue());
        }
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("001200", 8);
}
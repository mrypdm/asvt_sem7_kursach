using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class BVS : OneOperand
{
    public BVS(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        if (_state.GetFlag(Flag.V))
        {
            _state.R[7] = ushort(_state.R[7] + 2 * arguments[0].GetValue());
        }
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("102200", 8);
};
}
using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

internal class BEQ : BranchOperation
{
    public BEQ(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        if (_state.GetFlag(Flag.Z))
        {
            int value = (int)(arguments[0].GetValue());
            value = (value & 128) > 0 ? -(127 & value) : value;
            _state.Registers[7] = (ushort)(_state.Registers[7] + 2 * value);
        }
    }

    public override ushort Opcode => Convert.ToUInt16("001400", 8);
}

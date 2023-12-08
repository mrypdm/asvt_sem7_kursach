using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

internal class BCC : BranchOperation
{
    public BCC(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        if (!_state.GetFlag(Flag.C))
        {
            int value = (int)(arguments[0].GetValue());
            value = (value & 128) > 0 ? -(127 & value) : value;
            _state.Registers[7] = (ushort)(_state.Registers[7] + 2 * value);
        }
    }

    public override ushort Opcode => Convert.ToUInt16("103000", 8);
}
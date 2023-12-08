using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.BranchOperations;

public class BR : BranchOperation
{
    public BR(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        int value = (int)(arguments[0].GetValue());
        value = (value & 128) > 0 ? -(127 & value) : value;
        _state.Registers[7] = (ushort)(_state.Registers[7] + 2 * value);
    }

    public override ushort Opcode => Convert.ToUInt16("000400", 8);
}
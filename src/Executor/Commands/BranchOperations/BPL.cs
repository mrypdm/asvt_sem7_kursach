using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.BranchOperations;

public class BPL : BranchOperation
{
    public BPL(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IOffsetArgument>(arguments);
        if (!_state.N)
        {
            int value = validatedArgument.GetOffset();;
            value = (value & 128) > 0 ? -(127 & value) : value;
            _state.Registers[7] = (ushort)(_state.Registers[7] + 2 * value);
        }
    }

    public override ushort Opcode => Convert.ToUInt16("100000", 8);
}
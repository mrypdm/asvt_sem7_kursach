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

    public override void Execute(IArgument[] arguments) => UpdateProgramCounter(arguments);

    public override ushort Opcode => Convert.ToUInt16("000400", 8);
}
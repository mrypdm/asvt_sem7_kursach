using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class CLR : OneOperand
{
    public CLR(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        arguments[0].SetValue(0);
    }

    public override ushort Opcode => Convert.ToUInt16("005000", 8);
}
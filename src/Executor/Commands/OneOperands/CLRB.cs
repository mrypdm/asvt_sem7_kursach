using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

internal class CLRB : OneOperand
{
    public CLRB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        arguments[0].SetValue(0);
    }

    public override ushort Opcode => Convert.ToUInt16("105000", 8);
}
using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class INC : OneOperand
{
    public INC(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var value = arguments[0].GetValue();
        arguments[0].SetValue((ushort)(value+1));
    }

    public override ushort Opcode => Convert.ToUInt16("005200", 8);
}
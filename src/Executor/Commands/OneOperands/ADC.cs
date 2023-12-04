using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class ADC : OneOperand
{
    public ADC(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var value = arguments[0].GetValue();
        arguments[0].SetValue((ushort)(value - (_state.GetFlag(Flag.C) ? 1 : 0)));
    }

    public override ushort Opcode => Convert.ToUInt16("005500", 8);
}
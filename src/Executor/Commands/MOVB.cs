using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class MOVB : TwoOperands
{
    public MOVB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var value = arguments[0].GetValue();
        arguments[1].SetValue(value);
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("110000", 8);
}
using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class INCB : OneOperand
{
    public INCB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new RegisterByteArgument(Memory, State, GetMode(word), GetRegister(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
        var value = arguments[0].GetValue();
        arguments[0].SetValue(value+1);
    }

    public override ushort Opcode => 0b1010_1000_0000;
}
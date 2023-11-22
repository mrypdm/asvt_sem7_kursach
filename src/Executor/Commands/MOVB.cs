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

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new RegisterByteArgument(Memory, State, GetMode1(word), GetRegister1(word)),
            new RegisterByteArgument(Memory, State, GetMode2(word), GetRegister2(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
        var value = arguments[0].GetValue();
        arguments[1].SetValue(value);
    }

    public override ushort Opcode => 0b1001_0000_0000_0000;
}
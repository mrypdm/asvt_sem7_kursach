using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class MOV : TwoOperands
{
    public MOV(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new RegisterWordArgument(Memory, State, GetMode1(word), GetRegister1(word)),
            new RegisterWordArgument(Memory, State, GetMode2(word), GetRegister2(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
        var value = arguments[0].GetValue();
        arguments[1].SetValue(value);
    }

    public override ushort Opcode => 0b1_0000_0000_0000;
}
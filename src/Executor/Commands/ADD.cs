using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class ADD : TwoOperands
{
    public ADD(IMemory memory, IState state) : base(memory, state)
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
        arguments[1].SetValue((ushort)(arguments[1].GetValue() + arguments[0].GetValue()));
    }

    public override ushort Opcode => 0b110_0000_0000_0000;
}